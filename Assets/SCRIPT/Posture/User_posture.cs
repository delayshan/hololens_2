﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_posture : MonoBehaviour
{

    [SerializeField] [Header("腹部")]
    private Transform transformAbdomen = null;
    [SerializeField] [Header("胸口")]
    private Transform transformChest = null;
    [SerializeField] [Header("胸口目標")]
    private Transform transformChestTarget = null;
    [SerializeField] [Header("右手")]
    private Transform transformRightArm = null;
    [SerializeField] [Header("右手前臂")]
    private Transform transformRightForeArm = null;
    [SerializeField][Header("右手掌")]
    private Transform transformRightHand = null;
    [SerializeField] [Header("左手")]
    private Transform transformLeftArm = null;
    [SerializeField] [Header("左手前臂")]
    private Transform transformLeftForeArm = null;
    [SerializeField][Header("左手掌")]
    private Transform transformLeftHand = null;
    [SerializeField] [Header("右大腿")]
    private Transform transformRightUpLeg = null;
    [SerializeField] [Header("右小腿")]
    private Transform transformRightLeg = null;
    [SerializeField][Header("右腳踝")]
    private Transform transformRightFoot = null;
    [SerializeField] [Header("左大腿")]
    private Transform transformLeftUpLeg = null;
    [SerializeField] [Header("左小腿")]
    private Transform transformLeftLeg = null;
    [SerializeField] [Header("左腳踝")]
    private Transform transformLeftFoot = null;


    //假設這些座標給的都是世界座標
    [SerializeField]
    private Vector3 vector3abdomen;
    [SerializeField]
    private Vector3 vector3Chest;
    [SerializeField]
    private Vector3 vector3RightArm;
    [SerializeField]
    private Vector3 vector3RightForeArm;
    [SerializeField]
    private Vector3 vector3RightHand;
    [SerializeField]
    private Vector3 vector3LeftArm;
    [SerializeField]
    private Vector3 vector3LeftForeArm;
    [SerializeField]
    private Vector3 vector3LeftHand;
    [SerializeField]
    private Vector3 vector3RightUpLeg;
    [SerializeField]
    private Vector3 vector3RightLeg;
    [SerializeField]
    private Vector3 vector3RightFoot;
    [SerializeField]
    private Vector3 vector3LeftUpLeg;
    [SerializeField]
    private Vector3 vector3LeftLeg;
    [SerializeField]
    private Vector3 vector3LeftFoot;




    int counter = 0, double_num = 1;






    // Start is called before the first frame update
    void Start()
    {
        /*
        Vector3 TempForeArm = transformRightForeArm.position;
        Vector3 RightArmDirection = transformRightForeArm.position - transformRightArm.position;
        Vector3 RightArmNewDirection = Vector3.RotateTowards(transformRightArm.transform.right, RightArmDirection, 1, 0.0f);
        transformRightArm.right = RightArmNewDirection;
        transformRightForeArm.position = TempForeArm;
        */
    }

    // Update is called once per frame
    void Update()
    {
        //右手位置變化
        if (counter >= 60)
        {
            double_num *= -1;
            counter = -60;
        }
        else
        {
            counter += 1;
        }
        //vector3RightForeArm.y += (float)0.005 * double_num;
        //vector3RightArm.y += (float)0.001 * double_num;

        //第二關節目標位置-第一關節現在位置得出 第一關節在第二關節就定位時應該要有的ROTATION
        //在每個部位計算旋轉角度前 先固定該部位的當前位置(使用者給定位置座標)
        //使腹部與胸部相接(看之後智慧衣是否會提供角向量)
        transformAbdomen.position = vector3abdomen;
        this.ComputeAbdomenRotation();

        //使胸部與右手相接
        transformChest.position = vector3Chest;
        this.ComputeChestRotation();

        //使右手臂朝向右前臂
        transformRightArm.position = vector3RightArm;
        this.ComputeRightArmRotation();

        //使右前臂朝向右手腕
        transformRightForeArm.position = vector3RightForeArm;
        this.ComputeRightForeArmRotation();
        transformRightHand.position = vector3RightHand;        

        //使左手臂朝向左前臂
        transformLeftArm.position = vector3LeftArm;
        this.ComputeLeftArmRotation();

        //使左前臂朝向左手腕
        transformLeftForeArm.position = vector3LeftForeArm;
        this.ComputeLeftForeArmRotation();
        transformLeftHand.position = vector3LeftHand;

        //使右大腿朝向右腿
        transformRightUpLeg.position = vector3RightUpLeg;
        this.ComputeRightUpLegRotation();

        //使右腿朝向右腳踝
        transformRightLeg.position = vector3RightLeg;
        this.ComputeRightLegRotation();
        transformRightFoot.position = vector3RightFoot;

        //使左大腿朝向左腿
        transformLeftUpLeg.position = vector3LeftUpLeg;
        this.ComputeLeftUpLegRotation();

        //使左腿朝向左腳踝
        transformLeftLeg.position = vector3LeftLeg;
        this.ComputeLeftLegRotation();
        transformLeftFoot.position = vector3LeftFoot;

        /*
        //搞成WORLD
        Vector3 RightArmDirection = transformRightForeArm.TransformPoint(vector3RightForeArm) - transformRightArm.position;
        Vector3 RightArmNewDirection= Vector3.RotateTowards(transformRightArm.transform.forward, RightArmDirection, 1 * Time.deltaTime, 0.0f);
        transformRightArm.rotation = Quaternion.LookRotation(RightArmNewDirection);

        transformRightForeArm.localPosition = vector3RightForeArm;
        */
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {

        








    }

    /////////////////////////計算各個關節ROTATION/////////////////////////////////////
    //在計算關節時 會先記住該關節改變位置前的POSTION再進行計算
    //第二關節"目標"位置-第一關節現在位置得出 第一關節在第二關節就定位時應該要有的ROTATION

    /// <summary>
    /// 計算腹部與"胸口"間的ROTATION
    /// </summary>
    private void ComputeAbdomenRotation()
    {
        Vector3 AbdomenDirection = vector3Chest - transformAbdomen.position;
        Vector3 AbdomenNewDirection = Vector3.RotateTowards(transformAbdomen.transform.up, AbdomenDirection, 360, 0.0f);
        transformAbdomen.up = AbdomenNewDirection;
    }
    /// <summary>
    /// 計算胸口與"右手臂"間的ROTATION
    /// </summary>
    private void ComputeChestRotation()
    {
        //因為胸可會被腹部旋轉所更動ROTATION(當然其他的部位也會) 但由於胸部的ROTATION計算是由transformChestTarget的位置進行 且該位置不在胸部原點
        //因此在每次計算時 要將胸部ROTATION歸回初始
        transformChest.right = new Vector3(1, 0, 0);
        Vector3 ChestDirection = vector3RightArm - transformChestTarget.position;
        Vector3 ChestNewDirection = Vector3.RotateTowards(transformChest.transform.right, ChestDirection, 360, 0.0f);
        transformChest.right = ChestNewDirection;
    }
    /// <summary>
    /// 計算右手臂與"右前臂"間的ROTATION
    /// </summary>
    private void ComputeRightArmRotation()
    {
        Vector3 RightArmDirection = vector3RightForeArm - transformRightArm.position;
        Vector3 RightArmNewDirection = Vector3.RotateTowards(transformRightArm.transform.right, RightArmDirection, 180, 0.0f);
        transformRightArm.right = RightArmNewDirection;
    }
    /// <summary>
    /// 計算右前臂與"右手腕"間的ROTATION
    /// </summary>
    private void ComputeRightForeArmRotation()
    {
        Vector3 RightForeArmDirection = vector3RightHand - transformRightForeArm.position;
        Vector3 RightForeArmNewDirection = Vector3.RotateTowards(transformRightForeArm.transform.right, RightForeArmDirection, 180, 0.0f);
        transformRightForeArm.right = RightForeArmNewDirection;
    }
    /// <summary>
    /// 計算左手臂與"左前臂"間的ROTATION
    /// </summary>
    private void ComputeLeftArmRotation()
    {
        Vector3 LeftArmDirection = vector3LeftForeArm - transformLeftArm.position;
        Vector3 LeftArmNewDirection = Vector3.RotateTowards(transformLeftArm.transform.right * -1, LeftArmDirection, 180, 0.0f);
        transformLeftArm.right = LeftArmNewDirection * -1;
    }
    /// <summary>
    /// 計算左前臂與"左手腕"間的ROTATION
    /// </summary>
    private void ComputeLeftForeArmRotation()
    {
        Vector3 LeftForeArmDirection = vector3LeftHand - transformLeftForeArm.position;
        Vector3 LeftForeArmNewDirection = Vector3.RotateTowards(transformLeftForeArm.transform.right * -1, LeftForeArmDirection, 180, 0.0f);
        transformLeftForeArm.right = LeftForeArmNewDirection * -1;
    }
    /// <summary>
    /// 計算右大腿與"右小腿"間的ROTATION
    /// </summary>
    private void ComputeRightUpLegRotation()
    {
        Vector3 RightUpLegDirection = vector3RightLeg - transformRightUpLeg.position;
        Vector3 RightUpLegNewDirection = Vector3.RotateTowards(transformRightUpLeg.transform.up * -1, RightUpLegDirection, 180, 0.0f);
        transformRightUpLeg.up = RightUpLegNewDirection * -1;
    }
    /// <summary>
    /// 計算右小腿與"右腳踝"間的ROTATION
    /// </summary>
    private void ComputeRightLegRotation()
    {
        Vector3 RightLegDirection = vector3RightFoot - transformRightLeg.position;
        Vector3 RightLegNewDirection = Vector3.RotateTowards(transformRightLeg.transform.up * -1, RightLegDirection, 180, 0.0f);
        transformRightLeg.up = RightLegNewDirection * -1;
    }
    /// <summary>
    /// 計算左大腿與"左小腿"間的ROTATION
    /// </summary>
    private void ComputeLeftUpLegRotation()
    {
        Vector3 LeftUpLegDirection = vector3LeftLeg - transformLeftUpLeg.position;
        Vector3 LeftUpLegNewDirection = Vector3.RotateTowards(transformLeftUpLeg.transform.up * -1, LeftUpLegDirection, 180, 0.0f);
        transformLeftUpLeg.up = LeftUpLegNewDirection * -1;
    }
    /// <summary>
    /// 計算左小腿與"左腳踝"間的ROTATION
    /// </summary>
    private void ComputeLeftLegRotation()
    {
        Vector3 LeftLegDirection = vector3LeftFoot - transformLeftLeg.position;
        Vector3 LeftLegNewDirection = Vector3.RotateTowards(transformLeftLeg.transform.up * -1, LeftLegDirection, 180, 0.0f);
        transformLeftLeg.up = LeftLegNewDirection * -1;
    }
    /////////////////////////計算各個關節ROTATION/////////////////////////////////////

    /////////////////////////傳遞各個關節位置/////////////////////////////////////

    public Transform GetAbdomenPostion(){
        return transformAbdomen;
    }
    public Transform GetChestPostion()
    {
        return transformChest;
    }
    public Transform GetRightArmPostion()
    {
        return transformRightArm;
    }
    public Transform GetRightForeArmPostion()
    {
        return transformRightForeArm;
    }
    public Transform GetRightHandPostion()
    {
        return transformRightHand;
    }
    public Transform GetLeftArmPostion()
    {
        return transformLeftArm;
    }
    public Transform GetLeftForeArmPostion()
    {
        return transformLeftForeArm;
    }
    public Transform GetLeftHandPostion()
    {
        return transformLeftHand;
    }
    public Transform GetRightUpLegPostion()
    {
        return transformRightUpLeg;
    }
    public Transform GetRightLegPostion()
    {
        return transformRightLeg;
    }
    public Transform GetRightFootPostion()
    {
        return transformRightFoot;
    }
    public Transform GetLeftUpLegPostion()
    {
        return transformLeftUpLeg;
    }
    public Transform GetLeftLegPostion()
    {
        return transformLeftLeg;
    }
    public Transform GetLeftFootPostion()
    {
        return transformLeftFoot;
    }

    /////////////////////////傳遞各個關節位置/////////////////////////////////////


}
