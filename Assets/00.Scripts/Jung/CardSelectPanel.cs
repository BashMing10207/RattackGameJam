using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.UI;

public struct skillAndCard
{
    public Skills Skill;
    public GameObject Card;
}

public class CardSelectPanel : MonoBehaviour
{
    public PlayerInventory PlayerInventory;
    public GameObject cardPrefab;
    public Transform pivot;

    private List<skillAndCard> cards = new List<skillAndCard>();
    private void Start()
    {
        CreateCard();
    }
    
    [ContextMenu("Test")]
    private void CreateCard()
    {
        CardClear();
        
        int posX = 325;
                
        for (int i = -1; i < 2; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, pivot.transform);
            
            System.Array values = System.Enum.GetValues(typeof(Skills));
            Skills newSkill;
            bool isDuplicate;

            do
            {
                int randomIndex = Random.Range(0, values.Length - 1);
                newSkill = (Skills)values.GetValue(randomIndex);
                isDuplicate = false;

                foreach (var card in cards)
                {
                    if (card.Skill == newSkill)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            } while (isDuplicate);

            newCard.GetComponent<SkillCardUI>().SetSkillData(newSkill);
            newCard.GetComponent<RectTransform>().DOAnchorPosX(posX * i, 1.2f);
            
            skillAndCard newSkillAndCard;
            newSkillAndCard.Skill = newSkill;
            newSkillAndCard.Card = newCard;
            
            cards.Add(newSkillAndCard);
        }
    }
    private void CardClear()
    {
        if (cards.Count > 0)
        {
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                Destroy(cards[i].Card);
            }
        }
        
        cards.Clear();
    }
  
}
