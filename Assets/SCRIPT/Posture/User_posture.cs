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

    private Transform transformLeftChesttoShoulderBone = null;
    private Transform transformRightChesttoShoulderBone = null;


    int counter = 0, double_num = 1;


    /*****************取出資料中的關節部位資訊*************************/

    private float[][] FloatArray = null;

    /// <summary>
    /// 人物關節位置 資料中的INDEX 奇數為X 偶數為Y 0為部位名稱
    /// </summary>
    private int check = 0;
    private int clock = -1;

    /// <summary>
    /// 控制手的Z軸
    /// </summary>
    private float tempRightHandPostion = 55;
    private float LeftForeArmPostion = 0;
    private float RightForeArmPostion = 30;

    private float L_foreArmNum = 1;
    private float R_foreArmNum = -1;
    private float HandNum = 1;

    //用於關節滑順移動的位置參數
    Vector2 Abdomen = Vector2.zero;
    Vector2 Chest = Vector2.zero;
    Vector2 RA = Vector2.zero;
    Vector2 RFA = Vector2.zero;
    Vector2 RH = Vector2.zero;
    Vector2 LA = Vector2.zero;
    Vector2 LFA = Vector2.zero;
    Vector2 LH = Vector2.zero;
    Vector2 RUL = Vector2.zero;
    Vector2 RL = Vector2.zero;
    Vector2 RF = Vector2.zero;
    Vector2 LUL = Vector2.zero;
    Vector2 LL = Vector2.zero;
    Vector2 LF = Vector2.zero;

    /// <summary>
    /// 傳進來的資料對照User_posture各個部位，m_DataToBodyPart第0個要填入相對於DATA中腹部的欄位 以此類推。
    /// </summary>
    private int[] m_DataToBodyPart = { 9, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15 };


    /*****************取出資料中的關節部位資訊*************************/



    // Start is called before the first frame update
    void Start()
    {
        if (vector3Chest!=null)
        {
            transformLeftChesttoShoulderBone = transformChest.transform.Find("LeftShoulder_Cube");
            transformRightChesttoShoulderBone = transformChest.transform.Find("RightShoulder_Cube");

        }
        
        
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

        //FloatArray收到資訊 代表開始進行動作 clock歸0 方可開始進行判斷
        if (FloatArray != null && clock <= -1)
        {
            clock = 0;
        }

        if (FloatArray != null)
        {
            //10貞+1 CHECK(資料行數) 每2個(以X為準)CHECK進行一次位置處理
            if (clock >= 10)
            {

                //
                //Length-2 是因為CSV會多2行
                //
                if (FloatArray.Length - 2 > check)
                {

                    if (check % 2 == 0 && check != 0 && check < 4)
                    {

                        //使Z軸進行移動
                        if (LeftForeArmPostion > 30) L_foreArmNum *= -1;
                        else if (LeftForeArmPostion < 0) L_foreArmNum *= -1;
                        LeftForeArmPostion += 1 * L_foreArmNum;

                        if (RightForeArmPostion > 30) R_foreArmNum *= -1;
                        else if (RightForeArmPostion < 0) R_foreArmNum *= -1;
                        RightForeArmPostion += 1 * R_foreArmNum;


                        if (tempRightHandPostion > 60) HandNum *= -1;
                        else if (tempRightHandPostion < 55) HandNum *= -1;
                        tempRightHandPostion += 1 * HandNum;

                        //移動平滑化
                        Abdomen = new Vector2(FloatArray[check - 1][m_DataToBodyPart[0]], FloatArray[check][m_DataToBodyPart[0]]);
                        Chest = new Vector2(FloatArray[check - 1][m_DataToBodyPart[1]], FloatArray[check][m_DataToBodyPart[1]]);
                        RA = new Vector2(FloatArray[check - 1][m_DataToBodyPart[2]], FloatArray[check][m_DataToBodyPart[2]]);
                        RFA = new Vector2(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]]);
                        RH = new Vector2(FloatArray[check - 1][m_DataToBodyPart[4]], FloatArray[check][m_DataToBodyPart[4]]);
                        LA = new Vector2(FloatArray[check - 1][m_DataToBodyPart[5]], FloatArray[check][m_DataToBodyPart[5]]);
                        LFA = new Vector2(FloatArray[check - 1][m_DataToBodyPart[6]], FloatArray[check][m_DataToBodyPart[6]]);
                        LH = new Vector2(FloatArray[check - 1][m_DataToBodyPart[7]], FloatArray[check][m_DataToBodyPart[7]]);
                        RUL = new Vector2(FloatArray[check - 1][m_DataToBodyPart[8]], FloatArray[check][m_DataToBodyPart[8]]);
                        RL = new Vector2(FloatArray[check - 1][m_DataToBodyPart[9]], FloatArray[check][m_DataToBodyPart[9]]);
                        RF = new Vector2(FloatArray[check - 1][m_DataToBodyPart[10]], FloatArray[check][m_DataToBodyPart[10]]);
                        LUL = new Vector2(FloatArray[check - 1][m_DataToBodyPart[11]], FloatArray[check][m_DataToBodyPart[11]]);
                        LL = new Vector2(FloatArray[check - 1][m_DataToBodyPart[12]], FloatArray[check][m_DataToBodyPart[12]]);
                        LF = new Vector2(FloatArray[check - 1][m_DataToBodyPart[13]], FloatArray[check][m_DataToBodyPart[13]]);


                        //設定關節位置
                        this.SetAbdomenPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[0]], FloatArray[check][m_DataToBodyPart[0]], 0));
                        this.SetChestPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[1]], FloatArray[check][m_DataToBodyPart[1]], 5));
                        this.SetRightArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[2]], FloatArray[check][m_DataToBodyPart[2]], 0));
                        this.SetRightForeArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]], RightForeArmPostion));
                        this.SetRightHandPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[4]], FloatArray[check][m_DataToBodyPart[4]], tempRightHandPostion));
                        this.SetLeftArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[5]], FloatArray[check][m_DataToBodyPart[5]], 0));
                        this.SetLeftForeArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[6]], FloatArray[check][m_DataToBodyPart[6]], LeftForeArmPostion));
                        this.SetLeftHandPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[7]], FloatArray[check][m_DataToBodyPart[7]], tempRightHandPostion));
                        this.SetRightUpLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[8]], FloatArray[check][m_DataToBodyPart[8]], 0));
                        this.SetRightLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[9]], FloatArray[check][m_DataToBodyPart[9]], 0));
                        this.SetRightFootPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[10]], FloatArray[check][m_DataToBodyPart[10]], 5));
                        this.SetLeftUpLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[11]], FloatArray[check][m_DataToBodyPart[11]], 0));
                        this.SetLeftLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[12]], FloatArray[check][m_DataToBodyPart[12]], 0));
                        this.SetLeftFootPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[13]], FloatArray[check][m_DataToBodyPart[13]], 5));
                        /*
                        m_UserPosture.SetAbdomenPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[0]]), float.Parse(Array[check][m_DataToBodyPart[0]]), 0));
                        m_UserPosture.SetChestPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[1]]), float.Parse(Array[check][m_DataToBodyPart[1]]), 0));
                        m_UserPosture.SetRightArmPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[2]]), float.Parse(Array[check][m_DataToBodyPart[2]]), 0));
                        m_UserPosture.SetRightForeArmPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[3]]), float.Parse(Array[check][m_DataToBodyPart[3]]), 15));
                        m_UserPosture.SetRightHandPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[4]]), float.Parse(Array[check][m_DataToBodyPart[4]]), 60));
                        m_UserPosture.SetLeftArmPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[5]]), float.Parse(Array[check][m_DataToBodyPart[5]]), 0));
                        m_UserPosture.SetLeftForeArmPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[6]]), float.Parse(Array[check][m_DataToBodyPart[6]]), 15));
                        m_UserPosture.SetLeftHandPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[7]]), float.Parse(Array[check][m_DataToBodyPart[7]]), 60));
                        m_UserPosture.SetRightUpLegPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[8]]), float.Parse(Array[check][m_DataToBodyPart[8]]), 0));
                        m_UserPosture.SetRightLegPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[9]]), float.Parse(Array[check][m_DataToBodyPart[9]]), 0));
                        m_UserPosture.SetRightFootPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[10]]), float.Parse(Array[check][m_DataToBodyPart[10]]), 0));
                        m_UserPosture.SetLeftUpLegPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[11]]), float.Parse(Array[check][m_DataToBodyPart[11]]), 0));
                        m_UserPosture.SetLeftLegPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[12]]), float.Parse(Array[check][m_DataToBodyPart[12]]), 0));
                        m_UserPosture.SetLeftFootPostion(new Vector3(float.Parse(Array[check - 1][m_DataToBodyPart[13]]), float.Parse(Array[check][m_DataToBodyPart[13]]), 0));
                         */
                    }
                    else if (check % 2 == 0 && check != 0)
                    {
                        //使Z軸進行移動
                        if (LeftForeArmPostion > 30) L_foreArmNum *= -1;
                        else if (LeftForeArmPostion < 0) L_foreArmNum *= -1;
                        LeftForeArmPostion += 1 * L_foreArmNum;

                        if (RightForeArmPostion > 30) R_foreArmNum *= -1;
                        else if (RightForeArmPostion < 0) R_foreArmNum *= -1;
                        RightForeArmPostion += 1 * R_foreArmNum;


                        if (tempRightHandPostion > 60) HandNum *= -1;
                        else if (tempRightHandPostion < 55) HandNum *= -1;
                        tempRightHandPostion += 1 * HandNum;

                        //RFA= Vector2.Lerp( new Vector2(FloatArray[check - 3][m_DataToBodyPart[3]], FloatArray[check - 2][m_DataToBodyPart[3]]), new Vector2(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]]), 1f * Time.deltaTime);
                        //LFA = Vector2.Lerp(new Vector2(FloatArray[check - 3][m_DataToBodyPart[6]], FloatArray[check - 2][m_DataToBodyPart[6]]), new Vector2(FloatArray[check - 1][m_DataToBodyPart[6]], FloatArray[check][m_DataToBodyPart[6]]), 1f * Time.deltaTime);


                    }
                }
                else
                {
                    check = 0;
                    tempRightHandPostion = 55;
                    RightForeArmPostion = 30;
                    LeftForeArmPostion = 0;

                    L_foreArmNum = 1;
                    R_foreArmNum = -1;
                    HandNum = 1;
                }

                check += 1;
                clock = 0;

                //CHECK每變換一次做一次計算
                CompareData(FloatArray);
            }


            //每楨都取SLERP CHECK單雙要注意  
            if (check % 2 == 0 && check >= 4 && check != 0)
            {



                //關節移動平滑
                Abdomen = Vector2.Lerp(Abdomen, new Vector2(FloatArray[check - 1][m_DataToBodyPart[0]], FloatArray[check][m_DataToBodyPart[0]]), 10f * Time.deltaTime);
                Chest = Vector2.Lerp(Chest, new Vector2(FloatArray[check - 1][m_DataToBodyPart[1]], FloatArray[check][m_DataToBodyPart[1]]), 10f * Time.deltaTime);
                RA = Vector2.Lerp(RA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[2]], FloatArray[check][m_DataToBodyPart[2]]), 10f * Time.deltaTime);
                RFA = Vector2.Lerp(RFA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]]), 10f * Time.deltaTime);
                RH = Vector2.Lerp(RH, new Vector2(FloatArray[check - 1][m_DataToBodyPart[4]], FloatArray[check][m_DataToBodyPart[4]]), 10f * Time.deltaTime);
                LA = Vector2.Lerp(LA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[5]], FloatArray[check][m_DataToBodyPart[5]]), 10f * Time.deltaTime);
                LFA = Vector2.Lerp(LFA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[6]], FloatArray[check][m_DataToBodyPart[6]]), 10f * Time.deltaTime);
                LH = Vector2.Lerp(LH, new Vector2(FloatArray[check - 1][m_DataToBodyPart[7]], FloatArray[check][m_DataToBodyPart[7]]), 10f * Time.deltaTime);
                RUL = Vector2.Lerp(RUL, new Vector2(FloatArray[check - 1][m_DataToBodyPart[8]], FloatArray[check][m_DataToBodyPart[8]]), 10f * Time.deltaTime);
                RL = Vector2.Lerp(RL, new Vector2(FloatArray[check - 1][m_DataToBodyPart[9]], FloatArray[check][m_DataToBodyPart[9]]), 10f * Time.deltaTime);
                RF = Vector2.Lerp(RF, new Vector2(FloatArray[check - 1][m_DataToBodyPart[10]], FloatArray[check][m_DataToBodyPart[10]]), 10f * Time.deltaTime);
                LUL = Vector2.Lerp(LUL, new Vector2(FloatArray[check - 1][m_DataToBodyPart[11]], FloatArray[check][m_DataToBodyPart[11]]), 10f * Time.deltaTime);
                LL = Vector2.Lerp(LL, new Vector2(FloatArray[check - 1][m_DataToBodyPart[12]], FloatArray[check][m_DataToBodyPart[12]]), 10f * Time.deltaTime);
                LF = Vector2.Lerp(LF, new Vector2(FloatArray[check - 1][m_DataToBodyPart[13]], FloatArray[check][m_DataToBodyPart[13]]), 10f * Time.deltaTime);

                //設定關節位置
                /*
                 * TODO 未來可能需要設定不同身體的SCALE 因此可能得考慮在開始前站立測量
                 * 
                m_UserPosture.SetAbdomenPostion(new Vector3(Abdomen.x/200f, Abdomen.y / 200f, 0));
                m_UserPosture.SetChestPostion(new Vector3(Chest.x / 200f, Chest.y / 200f, 0));
                m_UserPosture.SetRightArmPostion(new Vector3(RA.x / 200f, RA.y / 200f, 0));
                m_UserPosture.SetRightForeArmPostion(new Vector3(RFA.x / 200f, RFA.y / 200f, 0.1f));
                m_UserPosture.SetRightHandPostion(new Vector3(RH.x / 200f, RH.y / 200f, 0.5f));
                m_UserPosture.SetLeftArmPostion(new Vector3(LA.x / 200f, LA.y / 200f, 0));
                m_UserPosture.SetLeftForeArmPostion(new Vector3(LFA.x / 200f, LFA.y / 200f, 0.1f));
                m_UserPosture.SetLeftHandPostion(new Vector3(LH.x / 200f, LH.y / 200f, 0.5f));
                m_UserPosture.SetRightUpLegPostion(new Vector3(RUL.x / 200f, RUL.y / 200f, 0));
                m_UserPosture.SetRightLegPostion(new Vector3(RL.x / 200f, RL.y / 200f, 0));
                m_UserPosture.SetRightFootPostion(new Vector3(RF.x / 200f, RF.y / 200f, 0));
                m_UserPosture.SetLeftUpLegPostion(new Vector3(LUL.x / 200f, LUL.y / 200f, 0));
                m_UserPosture.SetLeftLegPostion(new Vector3(LL.x / 200f, LL.y / 200f, 0));
                m_UserPosture.SetLeftFootPostion(new Vector3(LF.x / 200f, LF.y / 200f, 0));
                */

                this.SetAbdomenPostion(new Vector3(Abdomen.x, Abdomen.y, 0));
                this.SetChestPostion(new Vector3(Chest.x, Chest.y, 5));
                this.SetRightArmPostion(new Vector3(RA.x, RA.y, 0));
                this.SetRightForeArmPostion(new Vector3(RFA.x, RFA.y, RightForeArmPostion));
                this.SetRightHandPostion(new Vector3(RH.x, RH.y, RightForeArmPostion + 50));
                this.SetLeftArmPostion(new Vector3(LA.x, LA.y, 0));
                this.SetLeftForeArmPostion(new Vector3(LFA.x, LFA.y, LeftForeArmPostion));
                this.SetLeftHandPostion(new Vector3(LH.x, LH.y, LeftForeArmPostion + 50));
                this.SetRightUpLegPostion(new Vector3(RUL.x, RUL.y, 0));
                this.SetRightLegPostion(new Vector3(RL.x, RL.y, 0));
                this.SetRightFootPostion(new Vector3(RF.x, RF.y, 5));
                this.SetLeftUpLegPostion(new Vector3(LUL.x, LUL.y, 0));
                this.SetLeftLegPostion(new Vector3(LL.x, LL.y, 0));
                this.SetLeftFootPostion(new Vector3(LF.x, LF.y, 5));
            }
            else if (check % 2 == 1 && check >= 4 && check != 0)
            {


                //關節移動平滑
                Abdomen = Vector2.Lerp(Abdomen, new Vector2(FloatArray[check - 2][m_DataToBodyPart[0]], FloatArray[check - 1][m_DataToBodyPart[0]]), 10f * Time.deltaTime);
                Chest = Vector2.Lerp(Chest, new Vector2(FloatArray[check - 2][m_DataToBodyPart[1]], FloatArray[check - 1][m_DataToBodyPart[1]]), 10f * Time.deltaTime);
                RA = Vector2.Lerp(RA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[2]], FloatArray[check - 1][m_DataToBodyPart[2]]), 10f * Time.deltaTime);
                RFA = Vector2.Lerp(RFA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[3]], FloatArray[check - 1][m_DataToBodyPart[3]]), 10f * Time.deltaTime);
                RH = Vector2.Lerp(RH, new Vector2(FloatArray[check - 2][m_DataToBodyPart[4]], FloatArray[check - 1][m_DataToBodyPart[4]]), 10f * Time.deltaTime);
                LA = Vector2.Lerp(LA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[5]], FloatArray[check - 1][m_DataToBodyPart[5]]), 10f * Time.deltaTime);
                LFA = Vector2.Lerp(LFA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[6]], FloatArray[check - 1][m_DataToBodyPart[6]]), 10f * Time.deltaTime);
                LH = Vector2.Lerp(LH, new Vector2(FloatArray[check - 2][m_DataToBodyPart[7]], FloatArray[check - 1][m_DataToBodyPart[7]]), 10f * Time.deltaTime);
                RUL = Vector2.Lerp(RUL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[8]], FloatArray[check - 1][m_DataToBodyPart[8]]), 10f * Time.deltaTime);
                RL = Vector2.Lerp(RL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[9]], FloatArray[check - 1][m_DataToBodyPart[9]]), 10f * Time.deltaTime);
                RF = Vector2.Lerp(RF, new Vector2(FloatArray[check - 2][m_DataToBodyPart[10]], FloatArray[check - 1][m_DataToBodyPart[10]]), 10f * Time.deltaTime);
                LUL = Vector2.Lerp(LUL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[11]], FloatArray[check - 1][m_DataToBodyPart[11]]), 10f * Time.deltaTime);
                LL = Vector2.Lerp(LL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[12]], FloatArray[check - 1][m_DataToBodyPart[12]]), 10f * Time.deltaTime);
                LF = Vector2.Lerp(LF, new Vector2(FloatArray[check - 2][m_DataToBodyPart[13]], FloatArray[check - 1][m_DataToBodyPart[13]]), 10f * Time.deltaTime);

                //設定關節位置

                /*
                 * TODO 未來可能需要設定不同身體的SCALE 因此可能得考慮在開始前站立測量
                 * 
                m_UserPosture.SetAbdomenPostion(new Vector3(Abdomen.x / 200f, Abdomen.y / 200f, 0));
                m_UserPosture.SetChestPostion(new Vector3(Chest.x / 200f, Chest.y / 200f, 0));
                m_UserPosture.SetRightArmPostion(new Vector3(RA.x / 200f, RA.y / 200f, 0));
                m_UserPosture.SetRightForeArmPostion(new Vector3(RFA.x / 200f, RFA.y / 200f, 0.1f));
                m_UserPosture.SetRightHandPostion(new Vector3(RH.x / 200f, RH.y / 200f, 0.5f));
                m_UserPosture.SetLeftArmPostion(new Vector3(LA.x / 200f, LA.y / 200f, 0));
                m_UserPosture.SetLeftForeArmPostion(new Vector3(LFA.x / 200f, LFA.y / 200f, 0.1f));
                m_UserPosture.SetLeftHandPostion(new Vector3(LH.x / 200f, LH.y / 200f, 0.5f));
                m_UserPosture.SetRightUpLegPostion(new Vector3(RUL.x / 200f, RUL.y / 200f, 0));
                m_UserPosture.SetRightLegPostion(new Vector3(RL.x / 200f, RL.y / 200f, 0));
                m_UserPosture.SetRightFootPostion(new Vector3(RF.x / 200f, RF.y / 200f, 0));
                m_UserPosture.SetLeftUpLegPostion(new Vector3(LUL.x / 200f, LUL.y / 200f, 0));
                m_UserPosture.SetLeftLegPostion(new Vector3(LL.x / 200f, LL.y / 200f, 0));
                m_UserPosture.SetLeftFootPostion(new Vector3(LF.x / 200f, LF.y / 200f, 0));
                */

                this.SetAbdomenPostion(new Vector3(Abdomen.x, Abdomen.y, 0));
                this.SetChestPostion(new Vector3(Chest.x, Chest.y, 5));
                this.SetRightArmPostion(new Vector3(RA.x, RA.y, 0));
                this.SetRightForeArmPostion(new Vector3(RFA.x, RFA.y, RightForeArmPostion));
                this.SetRightHandPostion(new Vector3(RH.x, RH.y, RightForeArmPostion + 50));
                this.SetLeftArmPostion(new Vector3(LA.x, LA.y, 0));
                this.SetLeftForeArmPostion(new Vector3(LFA.x, LFA.y, LeftForeArmPostion));
                this.SetLeftHandPostion(new Vector3(LH.x, LH.y, LeftForeArmPostion + 50));
                this.SetRightUpLegPostion(new Vector3(RUL.x, RUL.y, 0));
                this.SetRightLegPostion(new Vector3(RL.x, RL.y, 0));
                this.SetRightFootPostion(new Vector3(RF.x, RF.y, 5));
                this.SetLeftUpLegPostion(new Vector3(LUL.x, LUL.y, 0));
                this.SetLeftLegPostion(new Vector3(LL.x, LL.y, 0));
                this.SetLeftFootPostion(new Vector3(LF.x, LF.y, 5));

            }

            if (clock >= 0)
                clock += 1;
        }




















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
        
        //使胸口骨骼與兩手臂的位置相接
        this.ComputeRightShoulderBoneRotation();
        this.ComputeLeftShoulderBoneRotation();

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
        transformAbdomen.up =  AbdomenDirection;
    }
    /// <summary>
    /// 計算胸口與"右手臂"間的ROTATION
    /// </summary>
    private void ComputeChestRotation()
    {
        //因為胸口會被腹部旋轉所更動ROTATION(當然其他的部位也會) 但由於胸部的ROTATION計算是由transformChestTarget的位置進行 且該位置不在胸部原點
        //因此在每次計算時 要將胸部ROTATION歸回初始
        transformChest.right = new Vector3(1, 0, 0);
        Vector3 ChestDirection = vector3RightArm - transformChestTarget.position;
        Vector3 ChestNewDirection = Vector3.RotateTowards(transformChest.transform.right, ChestDirection, 360, 0.0f);
        transformChest.right = ChestNewDirection;
    }
    /// <summary>
    /// 計算右手胸口骨骼與"右手肩膀"間的ROTATION
    /// </summary>
    private void ComputeRightShoulderBoneRotation()
    {

        Vector3 RightShoulderBoneDirection = vector3RightArm - transformChest.position;
        Vector3 RightShoulderBoneNewDirection = Vector3.RotateTowards(transformRightChesttoShoulderBone.transform.right, RightShoulderBoneDirection, 180, 0.0f);
        transformRightChesttoShoulderBone.right = RightShoulderBoneNewDirection;
        transformRightChesttoShoulderBone.position = transformChest.position + RightShoulderBoneDirection / 2f;
    }
    /// <summary>
    /// 計算左手胸口骨骼與"右手臂"間的ROTATION
    /// </summary>
    private void ComputeLeftShoulderBoneRotation()
    {
        Vector3 LeftShoulderBoneirection = vector3LeftArm - transformChest.position;
        Vector3 LeftShoulderBoneNewDirection = Vector3.RotateTowards(transformLeftChesttoShoulderBone.transform.right * -1, LeftShoulderBoneirection, 180, 0.0f);
        transformLeftChesttoShoulderBone.right = LeftShoulderBoneNewDirection * -1;
        transformLeftChesttoShoulderBone.position = transformChest.position + LeftShoulderBoneirection / 2f;
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

    /////////////////////////輸入各個關節位置/////////////////////////////////////
    public void SetAbdomenPostion(Vector3 position)
    {
        vector3abdomen = position;
    }
    public void SetChestPostion(Vector3 position)
    {
        vector3Chest = position;
    }
    public void SetRightArmPostion(Vector3 position)
    {
        vector3RightArm = position;
    }
    public void SetRightForeArmPostion(Vector3 position)
    {
        vector3RightForeArm = position;
    }
    public void SetRightHandPostion(Vector3 position)
    {
        vector3RightHand = position;
    }
    public void SetLeftArmPostion(Vector3 position)
    {
        vector3LeftArm = position;
    }
    public void SetLeftForeArmPostion(Vector3 position)
    {
        vector3LeftForeArm = position;
    }
    public void SetLeftHandPostion(Vector3 position)
    {
        vector3LeftHand = position;
    }
    public void SetRightUpLegPostion(Vector3 position)
    {
        vector3RightUpLeg = position;
    }
    public void SetRightLegPostion(Vector3 position)
    {
        vector3RightLeg = position;
    }
    public void SetRightFootPostion(Vector3 position)
    {
        vector3RightFoot = position;
    }
    public void SetLeftUpLegPostion(Vector3 position)
    {
        vector3LeftUpLeg = position;
    }
    public void SetLeftLegPostion(Vector3 position)
    {
        vector3LeftLeg = position;
    }
    public void SetLeftFootPostion(Vector3 position)
    {
        vector3LeftFoot = position;
    }


    /////////////////////////輸入各個關節位置/////////////////////////////////////

    /*****************取出資料中的關節部位資訊*************************/
    public void SetFloatArray(float[][] Array)
    {
        FloatArray = Array;
    }

    /**********未來資料精準或換成智慧衣後就可不必使用此部分之函式 此部分用於資料校準 將位置相差較大的部分篩選並進行更改或刪除**********/

    /// <summary>
    /// 若資料與前一筆資料相差過大，作對矩陣資料進行更改(因為為CALL BY REF 所以不回傳值)
    /// </summary>
    /// <param name="array"></param>
    private void CompareData(float[][] array)
    {
        //Debug.Log("check= " + check);
        if (check <= 2)
        {
            //第一筆資料 不需比較
        }
        else
        {
            //若與前次位置相差過大 進行數據調整 使改變不一下過大
            for (int i = 0; i < m_DataToBodyPart.Length; i++)
            {
                //0代表沒有資料 保持原樣
                if (array[check][m_DataToBodyPart[i]] == 0)
                {
                    array[check][m_DataToBodyPart[i]] = array[check - 2][m_DataToBodyPart[i]];
                }
                else if (array[check - 2][m_DataToBodyPart[i]] == 0)
                {
                    //前一次為0 則數據直接等於此次
                }
                //若當前位置小於前次位置 將當前之資料增加一點
                else if (array[check][m_DataToBodyPart[i]] - array[check - 2][m_DataToBodyPart[i]] < -5)
                {
                    array[check][m_DataToBodyPart[i]] += 3;
                }
                //若當前位置大於前次位置 將當前之資料減少一點
                else if (array[check][m_DataToBodyPart[i]] - array[check - 2][m_DataToBodyPart[i]] > 5)
                {
                    array[check][m_DataToBodyPart[i]] -= 3;
                }
            }
        }
    }



    /**********未來資料精準或換成智慧衣後就可不必使用此部分之函式 此部分用於資料校準 將位置相差較大的部分篩選並進行更改或刪除**********/

    /*****************取出資料中的關節部位資訊*************************/

}
