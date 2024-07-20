using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGM
{
    public class HorizontalCardHolder : MonoBehaviour
    {
        [SerializeField] private Canvas _cardCanvas;
        [SerializeField] private Card _selectedCard;
        [SerializeReference] private Card _hoveredCard;

        [SerializeField] private GameObject _slotPrefab;
        private RectTransform _rectTrm;

        [Header("Spawn Settings")]
        [SerializeField] private int _cardToSpawn = 5;
        [SerializeField] private Transform _visualHolderTrm;

        public List<Card> cards;

        private bool _isCrossing = false;//카드 교체중

        private void Start()
        {
            for(int i = 0; i < _cardToSpawn; i++)
            {
                Instantiate(_slotPrefab, transform);
            }

            _rectTrm = transform as RectTransform;
            cards = GetComponentsInChildren<Card>().ToList();
            int cardIdx = 0;
            cards.ForEach(card =>
            {
                card.PointerEnterEvent.AddListener(CardPointerEnter);
                card.PointerExitEvent.AddListener(CardPointerExit);
                card.BeginDragEvent.AddListener(BeginDrag);
                card.EndDragEvent.AddListener(EndDrag);
                card.name = $"Card_{cardIdx}";
                card.Initialize(_cardCanvas, _visualHolderTrm);
                cardIdx++;
            });

            StartCoroutine(Frame());

            IEnumerator Frame()
            {
                yield return new WaitForEndOfFrame();
                cards.ForEach(card => card.cardVisual?.UpdateIndex());
            }

        }

        private void Update()
        {
            HandlePlayerInput();
            MoveCardIfSelected();
        }

        private void MoveCardIfSelected()
        {
            if (_selectedCard == null) return;
            if (_isCrossing) return;

            float selectX = _selectedCard.transform.position.x;

            for(int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                Vector3 pos = card.transform.position;

                if(selectX > pos.x && _selectedCard.SlotIndex < card.SlotIndex)
                {
                    Swap(i);
                    break;
                    
                }else if(selectX < pos.x && _selectedCard.SlotIndex > card.SlotIndex)
                {
                    Swap(i);
                    break;
                }
            }
        }

        private void Swap(int i)
        {
            _isCrossing = true;
            Transform focusedParent = _selectedCard.transform.parent; //움직이는 애의 슬롯
            Transform crossedParent = cards[i].transform.parent; //교환될 녀석의 슬롯

            cards[i].transform.SetParent(focusedParent);
            cards[i].transform.localPosition 
                = cards[i].selected ? new Vector3(0, cards[i].selectionOffset) : Vector3.zero;
            _selectedCard.transform.SetParent(crossedParent);

            _isCrossing = false;

            //비쥬얼 교체부분
            if (cards[i].cardVisual == null) return;

            bool swapIsRight = cards[i].SlotIndex > _selectedCard.SlotIndex;
            //이녀석으로 오른쪽으로 스왑시키는거
            cards[i].cardVisual.Swap(swapIsRight ? -1 : 1); 
            cards.ForEach(card => card.cardVisual?.UpdateIndex());
        }

        private void HandlePlayerInput()
        {
            if(Input.GetKeyDown(KeyCode.Delete) && _hoveredCard != null)
            {
                cards.Remove(_hoveredCard);
                Destroy(_hoveredCard.transform.parent.gameObject); //슬롯을 파괴
            }

            if(Input.GetMouseButtonDown(1)) //우클릭시 모든 카드 비선택으로
            { 
                foreach(Card card in cards)
                {
                    card.DeSelectCard();
                }
            }
        }

        public void CardPointerEnter(Card card)
        {
            _hoveredCard = card;
        }

        public void CardPointerExit(Card card)
        {
            _hoveredCard = null;
        }

        public void BeginDrag(Card card)
        {
            _selectedCard = card;
        }

        public void EndDrag(Card card)
        {
            if (_selectedCard == null) return;

            Vector3 destination
                = _selectedCard.selected ? new Vector3(0, _selectedCard.selectionOffset) : Vector3.zero;
            _selectedCard.transform.DOLocalMove(destination, 0); //지금은 트윈 없고 0초로 처리
            _selectedCard = null;
        }

    }

}
