using Unity.Netcode;
using UnityEngine;
public abstract class Skill : NetworkBehaviour
{
    [SerializeField] private bool isUiUse;
    public bool GetIsUIUse => isUiUse;
    [SerializeField] private SO_CardAsset so_Card;
    public SO_CardAsset GetCardSO => so_Card;
    public GameObject effect;

    [Header("-")]
    [SerializeField] private int endTurnAmount;
    [SerializeField] private int endTurnAmountMax;
    public Transform stone;
    
    protected virtual void Awake()
    {
        endTurnAmount = endTurnAmountMax;
        if(name == "_")
            name = ToString();
    }
    public virtual void UIUse(NetPlayerStone netPlayerStone)
    {
        //logic
    }
    public void ThrowableInit()
    {
        NetCPlayer.ProjectileToShoot = so_Card.projectileSO;
        print("proj init");
    }
    #region bf
    public void TryActivateSkill(NetPlayerStone netPlayerStone)
    {
        //ActivateSkill(netPlayerStone);
        
    }
    
    protected virtual void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        if(endTurnAmount == endTurnAmountMax)
        {
            //netPlayerStone.Actions += OnEndTurn;
        }
        else if (endTurnAmount <= 0) endTurnAmount = endTurnAmountMax;
        
        GameObject _effect = Instantiate(effect, netPlayerStone.transform.position, Quaternion.identity);
        ParticleSystem particleSystem = _effect.GetComponent<ParticleSystem>();
        particleSystem.Play();
        
        Destroy(_effect, 3);
    }
    protected virtual void OnEndTurn()
    {
        void Deregister()
        {
            //netPlayerStone.Actions -= OnEndTurn;
            print("ended skill effect");
            //OnDeregisterEvent(netPlayerStone);
        }
        endTurnAmount--;
        if(endTurnAmount <= 0)
        {
            Deregister();
        }
    }
    protected virtual void OnDeregisterEvent(NetPlayerStone netPlayerStone)
    {

    }
    #endregion
}
