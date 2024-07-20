using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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
    public static NetworkVariable<bool> isHostTurn = new NetworkVariable<bool>(value:true);
    public static NetworkVariable<int> currentNum = new NetworkVariable<int>(value:0);
    public List<NetPlayerStone>[] stones = new List<NetPlayerStone>[2] {new List<NetPlayerStone>(), new List<NetPlayerStone>()};
    public CinemachineVirtualCamera vCamera;
    public Camera mainCam;
    public bool isActionSelected = false;

    public Transform[] StonePrefs;


    public ProjectileSO fireball;//임시 테스트용


    ActivedSkill activedSkill;

    #region mouseForceMove
    Vector3 tempMousePos;
    public LineRenderer lineRenderer;
    #endregion

    void Awake()
    {
        //JoinEvent.Instance.SetActive(false);
        NetControlUI.INSTANCE.OnJoin(TestLobby.CODE);

        if(NetGameMana.Instance.player != null)
        {
            stones = NetGameMana.Instance.player.stones;
        }
        NetGameMana.Instance.player = this;
        mainCam = Camera.main;
        vCamera = NetGameMana.Instance.GetComponentInChildren<CinemachineVirtualCamera>();
        lineRenderer = mainCam.GetComponentInChildren<LineRenderer>();

    }

    void Update()
    {
        if (!IsOwner)
            return;

        Updeat();
    }
  
    public void Updeat()
    {

        NetworkUpdate();

    }

    void NoNetWOrkUpdate()
    {

    }

    void NetworkUpdate()
    {
        if (!(IsHost ^ isHostTurn.Value))
        {
            BaseAction();//숙청
            PlayerActionMing();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EndTurnServerRpc();
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
        //인덱스 번호 바꾸기
    }

    void CamChange()
    {
        CamChangeServerRpc();
        vCamera.LookAt = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        vCamera.Follow = stones[isHostTurn.Value ? 0 : 1][currentNum.Value].pivot;
        //카메라 팔로우-룩앳 바꾸기

        SetOutline(true);
    }
    [ServerRpc]
    void EndTurnServerRpc()
    {
        isHostTurn.Value = !isHostTurn.Value;
        currentNum.Value = 0;
        CamChange();
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
                WhatActionServerRpc(Input.mousePosition,forceInput, magnitude,activedSkill);
                //print(forceInput.normalized);
                lineRenderer.enabled = false;
            }
        }

    }
    [ServerRpc]
    void WhatActionServerRpc(Vector3 Inputpos,Vector3 forceInput, float magnitude,ActivedSkill whatSkill)
    {
        switch ((whatSkill))
        {
            case ActivedSkill.move:
                stones[isHostTurn.Value ? 0 : 1][currentNum.Value].ForceMove(new Vector3(forceInput.x, 0, forceInput.y).normalized, -magnitude, 1);
                break;
                
            case ActivedSkill.create:

                RaycastHit hit;
                if (Physics.Raycast(mainCam.ScreenPointToRay(Inputpos), out hit))
                {
                Vector3 mousepos = hit.point;
                mousepos = new Vector3(mousepos.x, 10, mousepos.z);
                Transform spawnedObj = Instantiate(StonePrefs[isHostTurn.Value ? 0 : 1], mousepos, Quaternion.identity);
                spawnedObj.GetComponent<NetworkObject>().Spawn(true);
                }
                break;
            case ActivedSkill.fireball:
                NetGameMana.Instance.pool.Give(fireball, transform).GetComponent<Projectile>()
                    .Init(new Vector3(forceInput.x, 0, forceInput.y).normalized + Vector3.up * 0.5f, stones[isHostTurn.Value ? 0 : 1][currentNum.Value].transform.position + Vector3.up * 1.5f,
                    magnitude / 600);

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
