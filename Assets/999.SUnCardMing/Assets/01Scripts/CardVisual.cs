using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGM
{
    public class CardVisual : MonoBehaviour
    {
        private bool _initialize = false;

        [Header("Card")]
        public Card parentCard;
        private Transform _cardTrm;
        private Vector3 _rotationDelta;
        private int _savedIndex;
        private Vector3 _movementDelta;
        private Canvas _visualCanvas;

        [Header("Reference")]
        public Transform visualShadowTrm;
        private float _shadowOffset;
        private Vector2 _shadowDistance;
        private Canvas _shadowCanvas;
        [SerializeField] private Transform _shakeParentTrm, _tiltParentTrm;
        [SerializeField] private Image _cardImage;

        [Header("Follow parameter")]
        [SerializeField] private float _followSpeed = 30f;

        [Header("Curve")]
        [SerializeField] private CurveParamSO _curveParam;
        private float _curveYOffset, _curveRotationOffset;

        [Header("Rotation parameter")]
        [SerializeField] private float _rotationAmount = 20, _rotationSpeed = 20;
        [SerializeField] private float _autoTiltAmount = 30, _manualTiltAmount = 20, _tiltSpeed = 20;

        [Header("Swap Parameter")]
        [SerializeField] private bool _swapAnimation = true;
        [SerializeField] private float _swapRotationAngle = 30f, _swapDuration = 0.15f;
        [SerializeField] private int _swapVibrato = 5;

        [Header("Selection param")]
        [SerializeField] private float _selectPunchAmount = 20f;

        [Header("Scale param")]
        [SerializeField] private bool _scaleAnimation = true;
        [SerializeField] private float _defaultScale = 2f, _scaleOnHover = 1.15f, _scaleOnSelect = 1.25f, _scaleDuration = 0.15f;
        [SerializeField] private Ease _scaleEase = Ease.OutBack;

        [Header("Hover param")]
        [SerializeField] private float _hoverPunchAngle = 5f;
        [SerializeField] private float _hoverTransition = 0.15f;

        private void Start()
        {
            //_shadowDistance = visualShadowTrm.localPosition; //나중에 컴포넌트 값 받아와서 처리하는걸로 변경
        }

        private void Update()
        {
            if (!_initialize || parentCard == null) return;
            

            HandPositioning();
            SmoothFollow();
            FollowRotation();
            CardTilt();
        }



        private void CardTilt()
        {
            _savedIndex = parentCard.isDragging ? _savedIndex : parentCard.SlotIndex;
            float sine = Mathf.Sin(Time.time + _savedIndex) * (parentCard.isHovering ? 0.2f : 1);
            float cosine = Mathf.Cos(Time.time + _savedIndex) * (parentCard.isHovering ? 0.2f : 1);

            Vector3 angle = _tiltParentTrm.eulerAngles;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = transform.position - mousePos; //마우스 위치와 나 사이의 오프셋

            float tiltX = parentCard.isHovering ? (offset.y * -1 * _manualTiltAmount) : 0;
            float tiltY = parentCard.isHovering ? (offset.x * _manualTiltAmount) : 0;
            float tiltZ = parentCard.isDragging ? angle.z : (_curveRotationOffset * _curveParam.rotationInfluence * parentCard.SiblingAmount);

            float lerpX = Mathf.LerpAngle(
                angle.x, tiltX + (sine * _autoTiltAmount), _tiltSpeed * Time.deltaTime);

            float lerpY = Mathf.LerpAngle(
                angle.y, tiltY + (cosine * _autoTiltAmount), _tiltSpeed * Time.deltaTime);

            float lerpZ = Mathf.LerpAngle(
                angle.z, tiltZ , _tiltSpeed * 0.5f * Time.deltaTime);


            _tiltParentTrm.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
        }

        public void UpdateIndex()
        {
            transform.SetSiblingIndex(parentCard.SlotIndex);
        }

        //오른쪽 교환은 -1, 왼쪽 교환은 1인거야.
        public void Swap(float dir = 1)
        {
            if (!_swapAnimation) return;

            DOTween.Kill(2, true); //2번으로 지정하는건 차후에 흔들기 효과를 2번으로 넣을꺼야.
            _shakeParentTrm.DOPunchRotation((Vector3.forward * _swapRotationAngle) * dir, _swapDuration, _swapVibrato).SetId(3);
        }

        private void FollowRotation()
        {
            Vector3 movement = (transform.position - _cardTrm.position); //부모의 이동을 가져온다.
            _movementDelta = Vector3.Lerp(_movementDelta, movement, 25 * Time.deltaTime);

            //드래그 하는 아이템은 스무스하게, 재정렬되는 애들은 빠르게
            Vector3 movementRot = (parentCard.isDragging ? _movementDelta : movement) * _rotationAmount;
            _rotationDelta = Vector3.Lerp(_rotationDelta, movementRot, _rotationSpeed * Time.deltaTime);
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles 
                = new Vector3(angles.x, angles.y, Mathf.Clamp(_rotationDelta.x, -60f, 60f));
        }

        private void HandPositioning()
        {
            float indexNormal = parentCard.NormalizedPosition; //0~1까지의 위치 값
            int siblingCnt = parentCard.SiblingAmount;
            _curveYOffset = _curveParam.positioning.Evaluate(indexNormal)
                            * siblingCnt * _curveParam.positionInfluence;

            _curveYOffset = siblingCnt < 5 ? 0 : _curveYOffset;
            _curveRotationOffset = _curveParam.rotation.Evaluate(indexNormal);
        }

        private void SmoothFollow()
        {
            Vector3 verticalOffset = Vector3.up * (parentCard.isDragging ? 0 : _curveYOffset);
            transform.position = Vector3.Lerp(
                transform.position, _cardTrm.position + verticalOffset, Time.deltaTime * _followSpeed);
        }

        public void Initialize(Card target)
        {
            parentCard = target;
            _cardTrm = target.transform;
            _visualCanvas = GetComponent<Canvas>();
            //_shadowCanvas = GetComponent<Canvas>();

            parentCard.PointerEnterEvent.AddListener(PointerEnter);
            parentCard.PointerExitEvent.AddListener(PointerExit);
            parentCard.BeginDragEvent.AddListener(BeginDrag);
            parentCard.EndDragEvent.AddListener(EndDrag);
            parentCard.PointerDownEvent.AddListener(PointerDown);
            parentCard.PointerUpEvent.AddListener(PointerUp);
            parentCard.SelectEvent.AddListener(Select);

            _initialize = true; //이거 한 줄 빼먹었으니 꼭 쳐!
            transform.DOScale(_defaultScale, _scaleDuration).SetEase(_scaleEase);
        }

        private void Select(Card card, bool state)
        {
            DOTween.Kill(2, true); //재생중이던 애니메이션 다 완료짓고 죽어\
            float dir = state ? 1 : 0;

            _shakeParentTrm.DOPunchPosition(_shakeParentTrm.up * _selectPunchAmount * dir, _scaleDuration);
            _shakeParentTrm.DOPunchRotation(Vector3.forward * (_hoverPunchAngle * 0.5f), _hoverTransition, 20).SetId(2);

            if(_scaleAnimation)
            {
                transform.DOScale(_scaleOnHover, _scaleDuration).SetEase(_scaleEase);
            }
        }

        private void PointerUp(Card card, bool isLongPress)
        {
            
        }

        private void PointerDown(Card card)
        {
            
        }

        private void EndDrag(Card card)
        {
            _visualCanvas.overrideSorting = false;
            transform.DOScale(_defaultScale, _scaleDuration).SetEase(_scaleEase);
        }

        private void BeginDrag(Card card)
        {
            if (_scaleAnimation)
                transform.DOScale(_scaleOnSelect, _scaleDuration).SetEase(_scaleEase);

            _visualCanvas.overrideSorting = true;
        }

        private void PointerExit(Card card)
        {
            if(!parentCard.wasDragged)
            {
                transform.DOScale(_defaultScale, _scaleDuration).SetEase(_scaleEase);
            }
        }

        private void PointerEnter(Card card)
        {
            if(_scaleAnimation)
            {
                transform.DOScale(_scaleOnHover, _scaleDuration).SetEase(_scaleEase);
            }

            DOTween.Kill(2, true);
            _shakeParentTrm.DOPunchRotation(Vector3.forward * _hoverPunchAngle, _hoverTransition, 20).SetId(2);
        }
    }

}
