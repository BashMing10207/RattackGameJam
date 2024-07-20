using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHand : MonoBehaviour
{
    public PlayerInventory playerInventory;
    
    public GameObject cardInHandPrefab;
    public List<RectTransform> cardPosList = new List<RectTransform>();
    
    private void OnEnable()
    {
        PlayerInventory.OnInventoryChange += HandleSortCardInHand;
    }

    private void OnDisable()
    {
        PlayerInventory.OnInventoryChange -= HandleSortCardInHand;
    }

    public void HandleSortCardInHand()
    {
        for (int i = -1; i < cardPosList.Count - 1; i++)
        {
            cardPosList[i + 1].GetComponent<RectTransform>().DOAnchorPosX(i * 325, 1.2f);
        }
    }
    public void StartCreateCard()
    {
        for (int i = 0; i < 3; i++)
        {
            Array values = System.Enum.GetValues(typeof(Skills));
            Skills newSkill;
            bool isDuplicate;
            
            do
            {
                int randomIndex = Random.Range(0, values.Length - 1);
                newSkill = (Skills)values.GetValue(randomIndex);
                isDuplicate = false;
                
                foreach (var skill in playerInventory.GetSkills)
                {
                    if (NetGameMana.Instance.skillManager.GetSkill(newSkill) == skill)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            } while (isDuplicate);
            
            CreateCard(newSkill);
        }
    }

    public void CreateCard(Skills newSkill)
    {
        GameObject newCard = Instantiate(cardInHandPrefab, transform);
        CardInHand cardInHand = newCard.GetComponent<CardInHand>();
        
        cardInHand.Skills = newSkill;
        cardInHand.onRemove += playerInventory.TryRemoveSkill;
        cardInHand.posList = cardPosList;
        
        AddCard(newSkill, newCard);
    }

    private static void SettingCardUI(Skills newSkill, GameObject newCard)
    {
        SO_CardAsset cardSo = NetGameMana.Instance.skillManager.GetSkill(newSkill).GetCardSO;
        newCard.GetComponent<CardInHand>().SetCard(cardSo);
    }
    
    private void AddCard(Skills newSkill,GameObject newCard)
    {
        SettingCardUI(newSkill, newCard);
        cardPosList.Add(newCard.GetComponent<RectTransform>());
        
        playerInventory.TryAddSkill(NetGameMana.Instance.skillManager.GetSkill(newSkill));
    }
    
    
}
