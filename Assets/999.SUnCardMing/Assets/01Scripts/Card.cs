using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGM
{
    public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Header("Visual")]
        [SerializeField] private CardVisual _cardVisualPrefab;
        [HideInInspector] public CardVisual cardVisual;

        [Header("Selection")]
        public bool selected;
        public float selectionOffset = 50;
        private float _pointerDownTime;
        private float _pointUpTime;

        public int SlotIndex => transform.parent.GetSiblingIndex(); //내가 몇번째 자식인지 알 수 있다.
        public int SiblingAmount => transform.parent.parent.childCount; //홀더의 자식 갯수
        public float NormalizedPosition => ((float)SlotIndex).Remap(0, SiblingAmount - 1, 0, 1);

        [Header("Movement")]
        [SerializeField] private float _moveSpeedLimit = 50f;

        [Header("States")]
        public bool isHovering;
        public bool isDragging;
        [HideInInspector] public bool wasDragged;

        [Header("Events")]
        [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
        [HideInInspector] public UnityEvent<Card> PointerExitEvent;
        [HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;
        [HideInInspector] public UnityEvent<Card> PointerDownEvent;
        [HideInInspector] public UnityEvent<Card> BeginDragEvent;
        [HideInInspector] public UnityEvent<Card> EndDragEvent;
        [HideInInspector] public UnityEvent<Card, bool> SelectEvent;

        private Canvas _cardCanvas;
        private Image _imageCompo;
        private Vector3 _offset;
        private GraphicRaycaster _canvasRaycaster;
        private Rect _screenRect;
        private Camera _mainCam;

        public void Initialize(Canvas canvas, Transform visualHolderTrm)
        {
            _cardCanvas = canvas;
            _imageCompo = GetComponent<Image>();
            _canvasRaycaster = _cardCanvas.GetComponent<GraphicRaycaster>();
            _mainCam = Camera.main;
            CalculateScreenRect();

            cardVisual = Instantiate(_cardVisualPrefab, visualHolderTrm);
            cardVisual.Initialize(this); //카드와 카드비쥬얼이 연결된다.
        }

        private void CalculateScreenRect()
        {
            Vector3 camPosition = _mainCam.transform.position;
            Vector2 topLeft = _mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height, camPosition.z));
            Vector2 bottomRight = _mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, camPosition.z));

            Vector2 size = bottomRight - topLeft;
            _screenRect = new Rect(topLeft, size);
        }

        private void Update()
        {
            ClampPosition();
            DragFollow();
        }

        private void ClampPosition()
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, _screenRect.xMin, _screenRect.xMax);
            position.y = Mathf.Clamp(position.y, _screenRect.yMax, _screenRect.yMin);
            transform.position = position;
        }

        private void DragFollow()
        {
            if (!isDragging) return;

            Vector2 targetPos = _mainCam.ScreenToWorldPoint(Input.mousePosition) - _offset;
            Vector2 delta = (targetPos - (Vector2)transform.position);

            Vector2 velocity = delta.normalized * Mathf.Min(_moveSpeedLimit, delta.magnitude / Time.deltaTime);

            transform.Translate(velocity * Time.deltaTime);

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke(this);
            Vector2 mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            _offset = mousePosition - (Vector2)transform.position;
            isDragging = true;
            _canvasRaycaster.enabled = false;
            _imageCompo.raycastTarget = false;
            wasDragged = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke(this);
            isDragging = false;
            _canvasRaycaster.enabled = true;
            _imageCompo.raycastTarget = true;

            StartCoroutine(FrameWait());

            IEnumerator FrameWait()
            {
                yield return new WaitForEndOfFrame();
                wasDragged = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            
            PointerDownEvent?.Invoke(this);
            _pointerDownTime = Time.time;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterEvent?.Invoke(this);
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExitEvent?.Invoke(this);
            isHovering = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            float pointDownThreshold = 0.2f;
            _pointUpTime = Time.time;

            bool isDrag = _pointUpTime - _pointerDownTime > pointDownThreshold;
            PointerUpEvent?.Invoke(this, isDrag);

            if (isDrag) return;
            if (wasDragged) return;

            selected = !selected; //선택 반전
            SelectEvent?.Invoke(this, selected);

            if (selected)
                transform.localPosition += transform.up * selectionOffset;
            else
                transform.localPosition = Vector3.zero;

        }

        public void DeSelectCard()
        {
            if (selected)
                selected = false;
        }
    }
}

