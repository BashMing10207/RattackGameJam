using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardInHand : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Skills Skills;
    private RectTransform rectTransform;

    public Image cardImage;
    public TextMeshProUGUI description;
    
    [SerializeField] private float maxYPos;
    [SerializeField] private float minYPos;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOAnchorPosY(maxYPos , 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOAnchorPosY(minYPos , 0.4f);
    }

    public void SetCard(SO_CardAsset soCardAsset)
    {
        cardImage.sprite = soCardAsset.cardImage;
        description.SetText(soCardAsset.description);
    }
    
    
}
