using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHand : MonoBehaviour
{
    public PlayerInventory PlayerInventory;
    
    public GameObject cardInHandPrefab;
    public float startPosX;
        
    private void Start()
    {
        CreateCard();
    }
    
    private void CreateCard()
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject newCard = Instantiate(cardInHandPrefab, transform);

            Array values = System.Enum.GetValues(typeof(Skills));
            Skills newSkill;
            bool isDuplicate;

            do
            {
                int randomIndex = Random.Range(0, values.Length - 1);
                newSkill = (Skills)values.GetValue(randomIndex);
                isDuplicate = false;

                foreach (var skil in PlayerInventory.GetSkills)
                {
                    if (NetGameMana.Instance.skillManager.GetSkill(newSkill) == skil)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            } while (isDuplicate);

            newCard.GetComponent<RectTransform>().DOAnchorPosX(startPosX * i, 1.2f);
            newCard.GetComponent<CardInHand>().Skills = newSkill;
            
            SettingCardUI(newSkill, newCard);

            PlayerInventory.TryAddSkill(NetGameMana.Instance.skillManager.GetSkill(newSkill));
        }
    }

    private static void SettingCardUI(Skills newSkill, GameObject newCard)
    {
        SO_CardAsset cardSo = NetGameMana.Instance.skillManager.GetSkill(newSkill).GetCardSO;
        newCard.GetComponent<CardInHand>().SetCard(cardSo);
    }
}
