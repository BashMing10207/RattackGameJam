using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInSelectPanel : MonoBehaviour
{
    public Skills skillEnum;
    
    public Image cardSprite;
    public TextMeshProUGUI cardText;
    
    private Skill _skill;

    private void CardSetting()
    {
        _skill = NetGameMana.Instance.skillManager.GetSkill(skillEnum);
        SO_CardAsset cardData = _skill.GetCardSO;
        
        cardSprite.sprite = cardData.cardImage;
        cardText.SetText(cardData.description);
        
        GetComponent<Button>().onClick.AddListener(SelectBtn);
    }

    public void SetSkillData(Skills newSkill)
    {
        skillEnum = newSkill;
        
        CardSetting();
    }
    private void SelectBtn()
    {
        
        
        Transform grandParent = transform.parent.transform.parent;
        
        #region Dotween
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            transform.SetAsLastSibling();
        });
        seq.Append(GetComponent<RectTransform>().DOLocalRotate(new Vector3(0, -720f, 0), 1.2f, RotateMode.FastBeyond360)
            .SetRelative(true).SetEase(Ease.Linear));
        seq.Join(GetComponent<RectTransform>().DOScale(Vector3.one * 1.5f  , 2f));
        seq.Join(GetComponent<RectTransform>().DOAnchorPos3D(Vector3.zero, 2f));
        seq.AppendCallback(() =>
        {
            grandParent.gameObject.SetActive(false);
        });
        

        #endregion

        
    }

}
