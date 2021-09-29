using System.Collections;
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
    [SerializeField] [Header("左手")]
    private Transform transformLeftArm = null;
    [SerializeField] [Header("左手前臂")]
    private Transform transformLeftForeArm = null;
    [SerializeField] [Header("右大腿")]
    private Transform transformRightUpLeg = null;
    [SerializeField] [Header("右小腿")]
    private Transform transformRightLeg = null;
    [SerializeField] [Header("左大腿")]
    private Transform transformLeftUpLeg = null;
    [SerializeField] [Header("左小腿")]
    private Transform transformLeftLeg = null;


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
    private Vector3 vector3LeftArm;
    [SerializeField]
    private Vector3 vector3LeftForeArm;
    [SerializeField]
    private Vector3 vector3RightUpLeg;
    [SerializeField]
    private Vector3 vector3RightLeg;
    [SerializeField]
    private Vector3 vector3LeftUpLeg;
    [SerializeField]
    private Vector3 vector3LeftLeg;




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
        //使腹部與胸部相接(腹部比較特殊 無法單純用上下位置判定 看之後智慧衣是否會提供角向量)

        //this.ComputeAbdomenRotation();
        //transformChest.position = vector3Chest;
        transformAbdomen.position = vector3abdomen;

        //使胸部與右手相接
        this.ComputeChestRotation();

        transformChest.position = vector3Chest;
        transformRightArm.position = vector3RightArm;
        transformLeftArm.position = vector3LeftArm;

        //使右手臂朝向右前臂
        this.ComputeRightArmRotation();

        transformRightForeArm.position = vector3RightForeArm;

        //使左手臂朝向左前臂
        this.ComputeLeftArmRotation();

        transformLeftForeArm.position = vector3LeftForeArm;

        //使右大腿朝向右腿
        this.ComputeRightUpLeg();

        //使左大腿朝向左腿
        this.ComputeLeftUpLeg();



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
        //更改POSITION(位置要比ROTATION慢更改 因為身體有父子關係)
        





        transformRightUpLeg.position = vector3RightUpLeg;
        transformRightLeg.position = vector3RightLeg;
        transformLeftUpLeg.position = vector3LeftUpLeg;
        transformLeftLeg.position = vector3LeftLeg;
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
        Vector3 AbdomenNewDirection = Vector3.RotateTowards(transformChest.transform.up, AbdomenDirection, 1, 0.0f);
        transformAbdomen.up = AbdomenNewDirection;
    }
    /// <summary>
    /// 計算胸口與"右手臂"間的ROTATION
    /// </summary>
    private void ComputeChestRotation()
    {
        Vector3 ChestDirection = vector3RightArm - transformChestTarget.position;
        Vector3 ChestNewDirection = Vector3.RotateTowards(transformChest.transform.right, ChestDirection, 1, 0.0f);
        transformChest.right = ChestNewDirection;
    }
    /// <summary>
    /// 計算右手臂與"右前臂"間的ROTATION
    /// </summary>
    private void ComputeRightArmRotation()
    {
        Vector3 RightArmDirection = vector3RightForeArm - transformRightArm.position;
        Vector3 RightArmNewDirection = Vector3.RotateTowards(transformRightArm.transform.right, RightArmDirection, 1, 0.0f);
        transformRightArm.right = RightArmNewDirection;
    }
    /// <summary>
    /// 計算左手臂與"左前臂"間的ROTATION
    /// </summary>
    private void ComputeLeftArmRotation()
    {
        Vector3 LeftArmDirection = vector3LeftForeArm - transformLeftArm.position;
        Vector3 LeftArmNewDirection = Vector3.RotateTowards(transformLeftArm.transform.right * -1, LeftArmDirection, 1, 0.0f);
        transformLeftArm.right = LeftArmNewDirection * -1;
    }
    /// <summary>
    /// 計算右大腿與"右小腿"間的ROTATION
    /// </summary>
    private void ComputeRightUpLeg()
    {
        Vector3 RightUpLegDirection = vector3RightLeg - transformRightUpLeg.position;
        Vector3 RightUpLegNewDirection = Vector3.RotateTowards(transformRightUpLeg.transform.up * -1, RightUpLegDirection, 1, 0.0f);
        transformRightUpLeg.up = RightUpLegNewDirection * -1;
    }
    /// <summary>
    /// 計算左大腿與"左小腿"間的ROTATION
    /// </summary>
    private void ComputeLeftUpLeg()
    {
        Vector3 LeftUpLegDirection = vector3LeftLeg - transformLeftUpLeg.position;
        Vector3 LeftUpLegNewDirection = Vector3.RotateTowards(transformLeftUpLeg.transform.up * -1, LeftUpLegDirection, 1, 0.0f);
        transformLeftUpLeg.up = LeftUpLegNewDirection * -1;
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
    public Transform GetLeftArmPostion()
    {
        return transformLeftArm;
    }
    public Transform GetLeftForeArmPostion()
    {
        return transformLeftForeArm;
    }
    public Transform GetRightUpLegPostion()
    {
        return transformRightUpLeg;
    }
    public Transform GetRightLegPostion()
    {
        return transformRightLeg;
    }
    public Transform GetLeftUpLegPostion()
    {
        return transformLeftUpLeg;
    }
    public Transform GetLeftLegPostion()
    {
        return transformLeftLeg;
    }

    /////////////////////////傳遞各個關節位置/////////////////////////////////////


}
