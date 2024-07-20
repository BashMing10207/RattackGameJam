using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
public class CardSelectPanel : MonoBehaviour
{
    public SO_CardList skillList;
    public GameObject cardPrefab;
    public Transform pivot;

    private List<GameObject> cards = new List<GameObject>();
    
    private void OnEnable()
    {
        CreateCard();
    }
    [ContextMenu("Test")]
    void CreateCard()
    {
        CardClear();
        
        int posX = 325;
                
        for (int i = -1; i < 2; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, pivot.transform);
            newCard.GetComponent<RectTransform>().DOAnchorPosX(posX * i, 2f).SetEase(Ease.OutExpo);
            int randomCard = Random.Range(0 , skillList.list.Count);
            newCard.GetComponent<SkillCardUI>().SetCardData(skillList.list[randomCard]);
            cards.Add(newCard);
        }
    }

    private void CardClear()
    {
        if (cards.Count > 0)
        {
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                Destroy(cards[i]);
            }
        }
    }
}
