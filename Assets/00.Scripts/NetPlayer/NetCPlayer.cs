using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public enum ActivedSkill
{
    move,
    create,
    fireball,
    arrow,
    throwBox
};
public class NetCPlayer : NetworkBehaviour
{
    public static NetworkVariable<bool> isHostTurn = new NetworkVariable<bool>(value: true);
    public static NetworkVariable<int> currentNum = new NetworkVariable<int>(value: 0);
    public static NetPlayerStone GetCurrentStone
    {
        get
        {
            return stones[isHostTurn.Value ? 0 : 1][currentNum.Value];
        }
    }
    public static List<NetPlayerStone>[] stones = new List<NetPlayerStone>[2] { new List<NetPlayerStone>(), new List<NetPlayerStone>() };
    public static NetworkVariable <int>[] extraLifeCount = new NetworkVariable<int>[2] {new NetworkVariable<int>(value:6), new NetworkVariable<int>(value: 6) };
    public static event Action OnTurnEnd;
    public CinemachineVirtualCamera vCamera;
    public Camera mainCam;
    public bool isActionSelected = false;

    public Transform[] StonePrefs;

    //public ProjectileSO fireball;//임시 테스트용
    public static ProjectileSO ProjectileToShoot { get; set; }
    ActivedSkill activedSkill;

    public int extraLife = 3;

    #region mouseForceMove
    Vector3 tempMousePos;
    public LineRenderer lineRenderer;
    #endregion

    private GameObject playerHand;
    
    
    void Awake()
    {
        
        NetControlUI.INSTANCE.OnJoin(TestLobby.CODE);
        vCamera = NetGameMana.Instance.GetComponentInChildren<CinemachineVirtualCamera>();
        //if (NetGameMana.INSTANCE.player != null)
        //{
        //    Destroy(vCamera);
        //    Destroy(Camera.main.GetComponent<CinemachineBrain>());
        //}
        mainCam = Camera.main;
  
      
        
        lineRenderer = mainCam.GetComponentInChildren<LineRenderer>();

      
    }

    private void Start()
    {
        if(IsOwner)
        {
            NetGameMana.Instance.player = this;
            NetGameMana.Instance.playerHand.GetComponent<PlayerHand>().playerInventory = GetComponent<PlayerInventory>();
            NetGameMana.Instance.playerHand.GetComponent<PlayerHand>().StartCreateCard();
            WasdServerRpc();
        }
        
        playerHand = NetGameMana.Instance.playerHand;
        if(IsHost)
        {
            
        }
    }
    [ServerRpc]
    void WasdServerRpc()
    {
        
    }
    void Update()
    {
        if (!IsOwner)
            return;
        
        
        NetworkUpdate();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        if(stones[isHostTurn.Value ? 0 : 1].Count > 0)
        {
        vCamera.LookAt = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        vCamera.Follow = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        }

        //NetGameMana.Instance.lifeUI.ChangeLife();
    }

    void NetworkUpdate()
    {
        if (!(IsHost ^ isHostTurn.Value))
        {
            BaseAction();//숙청
            PlayerActionMing();

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            EndTurnServerRpc();
        }
        }
    }

    void BaseAction()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activedSkill = ActivedSkill.move;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activedSkill = ActivedSkill.create;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activedSkill = ActivedSkill.fireball;
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CamChange();
        }
    }
    [ServerRpc]
    void CamChangeServerRpc()
    {
        SetOutline(false);
        currentNum.Value = (currentNum.Value + 1) % stones[isHostTurn.Value? 0:1].Count;
        //vCamera.LookAt = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        //vCamera.Follow = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        SetOutline(true);
        //인덱스 번호 바꾸기
    }

    void FuckeCode()
    {

        //vCamera.LookAt = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        //vCamera.Follow = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        SetOutline(true);
    }

    void CamChange()
    {
        SetOutline(false);
        CamChangeServerRpc();

        Invoke(nameof(FuckeCode), 0.1f);
        //카메라 팔로우-룩앳 바꾸기
    }
    [ServerRpc]
    void EndTurnServerRpc()
    {
        OnTurnEnd?.Invoke();
        ProjectileToShoot = null;
        isHostTurn.Value = !isHostTurn.Value;
        currentNum.Value = 0;
        CamChange();
    }
    [ServerRpc]
    void AddExtraLifeServerRpc(int index,int num)//돌 생성 시 index는 0:검돌1:흰돌, num은 -1로 호출해야뒤~
    {
        extraLifeCount[index].Value+=num;
    }

    void PlayerActionMing()
    {
        
        if (IsOwner)
        {
            if(stones[isHostTurn.Value ? 0 : 1].Count > 0)
            {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tempMousePos = Input.mousePosition;
                lineRenderer.enabled = true;
                Vector3 a = mainCam.WorldToScreenPoint(stones[isHostTurn.Value ? 0 : 1][currentNum.Value].transform.position);
                lineRenderer.SetPosition(0, a + Vector3.back * a.z);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 mousepos = (Input.mousePosition - tempMousePos);
                float distance = Mathf.Clamp(mousepos.magnitude, 0, 1000);

                Vector3 a = mainCam.WorldToScreenPoint(stones[isHostTurn.Value ? 0 : 1][currentNum.Value].transform.position);
                lineRenderer.SetPosition(1, mousepos.normalized * distance + a + Vector3.back * a.z);
            }
            }


            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector3 forceInput = (Input.mousePosition - tempMousePos);
                float magnitude = forceInput.magnitude;
                magnitude = Mathf.Clamp(magnitude, 0, 1000);

                RaycastHit hit;
                if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit))
                {
                }
                    Vector3 mousepos = hit.point;
                    WhatActionServerRpc(mousepos, forceInput, magnitude, activedSkill);
                //print(forceInput.normalized);
                lineRenderer.enabled = false;
            }
        }

    }
    [ServerRpc]
    void WhatActionServerRpc(Vector3 inputpos,Vector3 forceInput, float magnitude,ActivedSkill whatSkill)
    {
        switch ((whatSkill))
        {
            case ActivedSkill.move:
                stones[isHostTurn.Value ? 0 : 1][currentNum.Value].ForceMove(new Vector3(forceInput.x, 0, forceInput.y).normalized, -magnitude, 1);
                break;
                
            case ActivedSkill.create:

                if (extraLifeCount[isHostTurn.Value ? 0 : 1].Value > 0)
                {
                inputpos = new Vector3(inputpos.x, 10, inputpos.z);
                Transform spawnedObj = Instantiate(StonePrefs[isHostTurn.Value ? 0 : 1], inputpos, Quaternion.identity);
                spawnedObj.GetComponent<NetworkObject>().Spawn(true);
                AddExtraLifeServerRpc(isHostTurn.Value ? 0 : 1, -1);
                }

                break;
            case ActivedSkill.fireball:

                //NetGameMana.INSTANCE.pool.GiveServerRpc(fireball, transform).GetComponent<Projectile>()
                print("n3");
                if(ProjectileToShoot != null)
                {
                    print("prok is not null");
                    GameObject projectile1 = Instantiate(ProjectileToShoot.gameObj);
                    projectile1.GetComponent<Projectile>()
                        .Init(new Vector3(forceInput.x, 0, forceInput.y).normalized + Vector3.up * 0.5f, stones[isHostTurn.Value ? 0 : 1][currentNum.Value].transform.position + Vector3.up * 1.5f,
                        magnitude / 600);
                    projectile1.GetComponent<NetworkObject>().Spawn(true);
                }
                else
                {
                    print("proj is null");
                }

                //GameObject projectile1 = Instantiate(fireball.gameObj);
                //projectile1.GetComponent<Projectile>()
                //    .Init(new Vector3(forceInput.x, 0, forceInput.y).normalized + Vector3.up * 0.5f, stones[isHostTurn.Value ? 0 : 1][currentNum.Value].transform.position + Vector3.up * 1.5f,
                //    magnitude / 600);
                //projectile1.GetComponent<NetworkObject>().Spawn(true);

                break;
            case ActivedSkill.arrow:
                break;
            case ActivedSkill.throwBox:
                break;
        };
    }

    private void SetOutline(bool active)
    {
        NetPlayerStone chooseStone = stones[isHostTurn.Value ? 0 : 1][currentNum.Value];
        chooseStone.outLine.SetActive(active);
    }
}
