using Unity.Netcode;
using UnityEngine;

public abstract class Skill : NetworkBehaviour
{
    public Transform stone;
    [SerializeField] private SO_CardAsset so_Card;
    public SO_CardAsset GetCardSO => so_Card;

    public GameObject effect;
    [SerializeField] private int endTurnAmount;
    [SerializeField] private int endTurnAmountMax;
    
    protected virtual void Awake()
    {
        endTurnAmount = endTurnAmountMax;
        if(name == "_")
            name = ToString();
    }
    public void TryActivateSkill(NetPlayerStone netPlayerStone)
    {
        ActivateSkill(netPlayerStone);
                
        
    }
    
    protected virtual void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        if(endTurnAmount == endTurnAmountMax)
        {
            netPlayerStone.Actions += OnEndTurn;
        }
        else if (endTurnAmount <= 0) endTurnAmount = endTurnAmountMax;
        
        GameObject _effect = Instantiate(effect, netPlayerStone.transform.position, Quaternion.identity);
        ParticleSystem particleSystem = _effect.GetComponent<ParticleSystem>();
        particleSystem.Play();
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

}
