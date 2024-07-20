using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CardInHand : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public Skills Skills;
    private RectTransform rectTransform;
    
    public Image cardImage;
    public TextMeshProUGUI description;
    
    [SerializeField] private float maxYPos;
    [SerializeField] private float minYPos;

    public Action<Skill> onRemove;
    public List<RectTransform> posList;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        onRemove.Invoke(NetGameMana.Instance.skillManager.GetSkill(Skills));
        
        posList.Remove(this.GetComponent<RectTransform>());

        Skill skill = NetGameMana.Instance.skillManager.GetSkill(Skills);
        //skill.TryActivateSkill();
        
        
        Destroy(gameObject);
    }
}
