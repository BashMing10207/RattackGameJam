using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
public class Player : MonoBehaviour
{
    public List<NetPlayerStone> stones = new List<NetPlayerStone>();
    public CinemachineVirtualCamera camera2;
    public Camera mainCam;
    public bool isActionSelected = false;

    public ProjectileSO fireball;//임시 테스트용

    public int currentNum = 0;
    public enum ActivedSkill
    {
        move,
        fireball,
        arrow,
        throwBox
    };

    ActivedSkill activedSkill;

    #region mouseForceMove
    Vector3 tempMousePos;
    public LineRenderer lineRenderer;
    #endregion

    void OnEnable()
    {
        NetGameMana.Instance.playerOff = this;
        //OLDGameMana.instance.player = this;
        mainCam = Camera.main;

    }
    private void Start()
    {
        Camchange();    
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activedSkill = ActivedSkill.move;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            activedSkill = ActivedSkill.fireball;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {

            Camchange();
        }
        PlayerActionMing();
    }

    void Camchange()
    {
        

        currentNum = (currentNum + 1) % stones.Count;
        //인덱스 번호 바꾸기
        camera2.LookAt = stones[currentNum].pivot;
        camera2.Follow = stones[currentNum].pivot;
        //카메라 팔로우-룩앳 바꾸기
    }

    private void PlayerActionMing()
    {
        if (isActionSelected)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tempMousePos = Input.mousePosition;
                lineRenderer.enabled = true;
                Vector3 a = mainCam.WorldToScreenPoint(stones[currentNum].transform.position);
                lineRenderer.SetPosition(0, a + Vector3.back * a.z);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 mousepos = (Input.mousePosition - tempMousePos);
                float distance = Mathf.Clamp(mousepos.magnitude,0,1000);

                Vector3 a = mainCam.WorldToScreenPoint(stones[currentNum].transform.position);
                lineRenderer.SetPosition(1, mousepos.normalized * distance + a + Vector3.back * a.z);
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Vector3 forceInput = (Input.mousePosition - tempMousePos);
                float magnitude = forceInput.magnitude;
                magnitude = Mathf.Clamp(magnitude,0,1000);
                WhatAction(forceInput,magnitude);
                print(forceInput.normalized);
                lineRenderer.enabled = false;
            }
        }

    }
    void WhatAction(Vector3 forceInput,float magnitude)
    {
        switch ((activedSkill))
        {
            case ActivedSkill.move:
                stones[currentNum].ForceMove(new Vector3(forceInput.x, 0, forceInput.y).normalized, -magnitude, 1);
                break;
            case ActivedSkill.fireball:
                OLDGameMana.instance.pool.Give(fireball, transform).GetComponent<Projectile>()
                    .Init(new Vector3(forceInput.x,0, forceInput.y).normalized+Vector3.up*0.5f, stones[currentNum].transform.position + Vector3.up*1.5f,
                    magnitude/600);

                break;
            case ActivedSkill.arrow:
                break;
            case ActivedSkill.throwBox:
                break;
        };
    }
}
