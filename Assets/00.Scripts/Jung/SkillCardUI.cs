using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCardUI : MonoBehaviour
{
    public SO_CardAsset cardData;
    
    public Image cardSprite;
    public TextMeshProUGUI cardText;



    private void CardSetting()
    {
        cardSprite.sprite = cardData.cardImage;
        cardText.SetText(cardData.description);
    }

    public void SetCardData(SO_CardAsset newCardData)
    {
        cardData = newCardData;
        CardSetting();
    }
}
