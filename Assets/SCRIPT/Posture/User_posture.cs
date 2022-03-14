using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_posture : MonoBehaviour
{
    /// <summary>
    /// 使用向量來更改關節位置
    /// </summary>
    [SerializeField]
    private bool boolChangeToVectorMode = true;

    /// <summary>
    /// 使用JSON資料來更改關節位置
    /// </summary>
    [SerializeField]
    private bool boolChangeToJsonData = true;


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




    /*****************取出資料中的關節部位資訊*************************/
    //儲存CSV資料的關節位置
    private float[][] FloatArray = null;

    //儲存JSON資料的關節位置
    private bodiesList m_bodyList = null;

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
        //是否為讀取JSON格式資料 不是的話就是CSV資料 CSV資料為一行X行Y
        if (!boolChangeToJsonData)
        {
            //FloatArray收到資訊 代表開始進行動作 clock歸0 方可開始進行判斷
            if (FloatArray != null && clock <= -1)
            {
                clock = 0;
            }
            //將得到的資料輸入進身體資訊 同時進行平滑化(沒更改位置但給值)
            if (FloatArray != null)
            {
                //1貞+1 CHECK(資料行數) 每2個(以X為準)CHECK進行一次位置處理
                if (clock >= 1)
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

        }
        //若為JSON 則讀取傳入的BodyData資料
        else
        {

            //m_bodyList收到資訊 代表開始進行動作 clock歸0 方可開始進行判斷
            if (m_bodyList != null && clock <= -1)
            {
                clock = 0;
            }
            //將得到的資料輸入進身體資訊 同時進行平滑化(沒更改位置但給值)
            if (m_bodyList != null)
            {
                
                if (clock >= 1)
                {
                    if (m_bodyList.bodies.Length  > check)
                    {
                        if (check == 0)
                        {
                            //設定關節位置
                            this.SetAbdomenPostion(new Vector3(m_bodyList.bodies[check].MidHip.x, m_bodyList.bodies[check].MidHip.y, m_bodyList.bodies[check].MidHip.z));
                            this.SetChestPostion(new Vector3(m_bodyList.bodies[check].Neck.x, m_bodyList.bodies[check].Neck.y, m_bodyList.bodies[check].Neck.z));
                            this.SetRightArmPostion(new Vector3(m_bodyList.bodies[check].RShoulder.x, m_bodyList.bodies[check].RShoulder.y, m_bodyList.bodies[check].RShoulder.z));
                            this.SetRightForeArmPostion(new Vector3(m_bodyList.bodies[check].RElbow.x, m_bodyList.bodies[check].RElbow.y, m_bodyList.bodies[check].RElbow.z));
                            this.SetRightHandPostion(new Vector3(m_bodyList.bodies[check].RWrist.x, m_bodyList.bodies[check].RWrist.y, m_bodyList.bodies[check].RWrist.z));
                            this.SetLeftArmPostion(new Vector3(m_bodyList.bodies[check].LShoulder.x, m_bodyList.bodies[check].LShoulder.y, m_bodyList.bodies[check].LShoulder.z));
                            this.SetLeftForeArmPostion(new Vector3(m_bodyList.bodies[check].LElbow.x, m_bodyList.bodies[check].LElbow.y, m_bodyList.bodies[check].LElbow.z));
                            this.SetLeftHandPostion(new Vector3(m_bodyList.bodies[check].LWrist.x, m_bodyList.bodies[check].LWrist.y, m_bodyList.bodies[check].LWrist.z));
                            this.SetRightUpLegPostion(new Vector3(m_bodyList.bodies[check].RHip.x, m_bodyList.bodies[check].RHip.y, m_bodyList.bodies[check].RHip.z));
                            this.SetRightLegPostion(new Vector3(m_bodyList.bodies[check].RKnee.x, m_bodyList.bodies[check].RKnee.y, m_bodyList.bodies[check].RKnee.z));
                            this.SetRightFootPostion(new Vector3(m_bodyList.bodies[check].RAnkle.x, m_bodyList.bodies[check].RAnkle.y, m_bodyList.bodies[check].RAnkle.z));
                            this.SetLeftUpLegPostion(new Vector3(m_bodyList.bodies[check].LHip.x, m_bodyList.bodies[check].LHip.y, m_bodyList.bodies[check].LHip.z));
                            this.SetLeftLegPostion(new Vector3(m_bodyList.bodies[check].LKnee.x, m_bodyList.bodies[check].LKnee.y, m_bodyList.bodies[check].LKnee.z));
                            this.SetLeftFootPostion(new Vector3(m_bodyList.bodies[check].LAnkle.x, m_bodyList.bodies[check].LAnkle.y, m_bodyList.bodies[check].LAnkle.z));
                        }
                        else
                        {
                            if (m_bodyList.bodies[check - 1].MidHip.c != 0)
                                this.SetAbdomenPostion(Vector3.Lerp(vector3abdomen, new Vector3(m_bodyList.bodies[check].MidHip.x, m_bodyList.bodies[check].MidHip.y, m_bodyList.bodies[check].MidHip.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].Neck.c != 0)
                                this.SetChestPostion(Vector3.Lerp(vector3Chest, new Vector3(m_bodyList.bodies[check].Neck.x, m_bodyList.bodies[check].Neck.y, m_bodyList.bodies[check].Neck.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RShoulder.c != 0)
                                this.SetRightArmPostion(Vector3.Lerp(vector3RightArm, new Vector3(m_bodyList.bodies[check].RShoulder.x, m_bodyList.bodies[check].RShoulder.y, m_bodyList.bodies[check].RShoulder.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RElbow.c != 0)
                                this.SetRightForeArmPostion(Vector3.Lerp(vector3RightForeArm, new Vector3(m_bodyList.bodies[check].RElbow.x, m_bodyList.bodies[check].RElbow.y, m_bodyList.bodies[check].RElbow.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RWrist.c != 0)
                                this.SetRightHandPostion(Vector3.Lerp(vector3RightHand, new Vector3(m_bodyList.bodies[check].RWrist.x, m_bodyList.bodies[check].RWrist.y, m_bodyList.bodies[check].RWrist.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LShoulder.c != 0)
                                this.SetLeftArmPostion(Vector3.Lerp(vector3LeftArm, new Vector3(m_bodyList.bodies[check].LShoulder.x, m_bodyList.bodies[check].LShoulder.y, m_bodyList.bodies[check].LShoulder.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LElbow.c != 0)
                                this.SetLeftForeArmPostion(Vector3.Lerp(vector3LeftForeArm, new Vector3(m_bodyList.bodies[check].LElbow.x, m_bodyList.bodies[check].LElbow.y, m_bodyList.bodies[check].LElbow.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LWrist.c != 0)
                                this.SetLeftHandPostion(Vector3.Lerp(vector3LeftHand, new Vector3(m_bodyList.bodies[check].LWrist.x, m_bodyList.bodies[check].LWrist.y, m_bodyList.bodies[check].LWrist.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RHip.c != 0)
                                this.SetRightUpLegPostion(Vector3.Lerp(vector3RightUpLeg, new Vector3(m_bodyList.bodies[check].RHip.x, m_bodyList.bodies[check].RHip.y, m_bodyList.bodies[check].RHip.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RKnee.c != 0)
                                this.SetRightLegPostion(Vector3.Lerp(vector3RightLeg, new Vector3(m_bodyList.bodies[check].RKnee.x, m_bodyList.bodies[check].RKnee.y, m_bodyList.bodies[check].RKnee.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RAnkle.c != 0)
                                this.SetRightFootPostion(Vector3.Lerp(vector3RightFoot, new Vector3(m_bodyList.bodies[check].RAnkle.x, m_bodyList.bodies[check].RAnkle.y, m_bodyList.bodies[check].RAnkle.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LHip.c != 0)
                                this.SetLeftUpLegPostion(Vector3.Lerp(vector3LeftUpLeg, new Vector3(m_bodyList.bodies[check].LHip.x, m_bodyList.bodies[check].LHip.y, m_bodyList.bodies[check].LHip.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LKnee.c != 0)
                                this.SetLeftLegPostion(Vector3.Lerp(vector3LeftLeg, new Vector3(m_bodyList.bodies[check].LKnee.x, m_bodyList.bodies[check].LKnee.y, m_bodyList.bodies[check].LKnee.z), 50f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LAnkle.c != 0)
                                this.SetLeftFootPostion(Vector3.Lerp(vector3LeftFoot, new Vector3(m_bodyList.bodies[check].LAnkle.x, m_bodyList.bodies[check].LAnkle.y, m_bodyList.bodies[check].LAnkle.z), 50f * Time.deltaTime));

                            /*
                            if (m_bodyList.bodies[check - 1].MidHip.c != 0) 
                                this.SetAbdomenPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check - 1].MidHip.x, m_bodyList.bodies[check - 1].MidHip.y, m_bodyList.bodies[check - 1].MidHip.z), new Vector3(m_bodyList.bodies[check].MidHip.x, m_bodyList.bodies[check].MidHip.y, m_bodyList.bodies[check].MidHip.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].Neck.c != 0)
                                this.SetChestPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check - 1].Neck.x, m_bodyList.bodies[check - 1].Neck.y, m_bodyList.bodies[check - 1].Neck.z), new Vector3(m_bodyList.bodies[check].Neck.x, m_bodyList.bodies[check].Neck.y, m_bodyList.bodies[check].Neck.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RShoulder.c != 0)
                                this.SetRightArmPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check - 1].RShoulder.x, m_bodyList.bodies[check - 1].RShoulder.y, m_bodyList.bodies[check - 1].RShoulder.z), new Vector3(m_bodyList.bodies[check].RShoulder.x, m_bodyList.bodies[check].RShoulder.y, m_bodyList.bodies[check].RShoulder.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RElbow.c != 0)
                                this.SetRightForeArmPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check - 1].RElbow.x, m_bodyList.bodies[check - 1].RElbow.y, m_bodyList.bodies[check - 1].RElbow.z), new Vector3(m_bodyList.bodies[check].RElbow.x, m_bodyList.bodies[check].RElbow.y, m_bodyList.bodies[check].RElbow.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RWrist.c != 0)
                                this.SetRightHandPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check - 1].RWrist.x, m_bodyList.bodies[check - 1].RWrist.y, m_bodyList.bodies[check - 1].RWrist.z), new Vector3(m_bodyList.bodies[check].RWrist.x, m_bodyList.bodies[check].RWrist.y, m_bodyList.bodies[check].RWrist.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LShoulder.c != 0)
                                this.SetLeftArmPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].LShoulder.x, m_bodyList.bodies[check-1].LShoulder.y, m_bodyList.bodies[check-1].LShoulder.z), new Vector3(m_bodyList.bodies[check].LShoulder.x, m_bodyList.bodies[check].LShoulder.y, m_bodyList.bodies[check].LShoulder.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LElbow.c != 0)
                                this.SetLeftForeArmPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].LElbow.x, m_bodyList.bodies[check-1].LElbow.y, m_bodyList.bodies[check-1].LElbow.z), new Vector3(m_bodyList.bodies[check].LElbow.x, m_bodyList.bodies[check].LElbow.y, m_bodyList.bodies[check].LElbow.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LWrist.c != 0)
                                this.SetLeftHandPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].LWrist.x, m_bodyList.bodies[check-1].LWrist.y, m_bodyList.bodies[check-1].LWrist.z), new Vector3(m_bodyList.bodies[check].LWrist.x, m_bodyList.bodies[check].LWrist.y, m_bodyList.bodies[check].LWrist.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RHip.c != 0)
                                this.SetRightUpLegPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].RHip.x, m_bodyList.bodies[check-1].RHip.y, m_bodyList.bodies[check-1].RHip.z), new Vector3(m_bodyList.bodies[check].RHip.x, m_bodyList.bodies[check].RHip.y, m_bodyList.bodies[check].RHip.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RKnee.c != 0)
                                this.SetRightLegPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].RKnee.x, m_bodyList.bodies[check-1].RKnee.y, m_bodyList.bodies[check-1].RKnee.z), new Vector3(m_bodyList.bodies[check].RKnee.x, m_bodyList.bodies[check].RKnee.y, m_bodyList.bodies[check].RKnee.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].RAnkle.c != 0)
                                this.SetRightFootPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].RAnkle.x, m_bodyList.bodies[check-1].RAnkle.y, m_bodyList.bodies[check-1].RAnkle.z), new Vector3(m_bodyList.bodies[check].RAnkle.x, m_bodyList.bodies[check].RAnkle.y, m_bodyList.bodies[check].RAnkle.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LHip.c != 0)
                                this.SetLeftUpLegPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].LHip.x, m_bodyList.bodies[check-1].LHip.y, m_bodyList.bodies[check-1].LHip.z), new Vector3(m_bodyList.bodies[check].LHip.x, m_bodyList.bodies[check].LHip.y, m_bodyList.bodies[check].LHip.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LKnee.c != 0)
                                this.SetLeftLegPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].LKnee.x, m_bodyList.bodies[check-1].LKnee.y, m_bodyList.bodies[check-1].LKnee.z), new Vector3(m_bodyList.bodies[check].LKnee.x, m_bodyList.bodies[check].LKnee.y, m_bodyList.bodies[check].LKnee.z), 10f * Time.deltaTime));
                            if (m_bodyList.bodies[check - 1].LAnkle.c != 0)
                                this.SetLeftFootPostion(Vector3.Lerp(new Vector3(m_bodyList.bodies[check-1].LAnkle.x, m_bodyList.bodies[check-1].LAnkle.y, m_bodyList.bodies[check-1].LAnkle.z), new Vector3(m_bodyList.bodies[check].LAnkle.x, m_bodyList.bodies[check].LAnkle.y, m_bodyList.bodies[check].LAnkle.z), 10f * Time.deltaTime));
                            */
                        }



                       




                    }


                    check += 1;
                    clock = 0;

                    //CHECK每變換一次做一次計算
                    //CompareData(FloatArray);
                }


                if (clock >= 0)
                    clock += 1;
            }
            
        }


















        //計算給定位置間的向量改變人體位置
        if (boolChangeToVectorMode)
        {

            
            Vector3 AbdomenDirection = vector3Chest - vector3abdomen;
            //Vector3 AbdomenNewDirection = Vector3.RotateTowards(transformAbdomen.transform.up,new Vector3(AbdomenDirection.x, AbdomenDirection.y, AbdomenDirection.z) , 360, 0.0f);
            //transformAbdomen.up = AbdomenDirection;
            
            transformAbdomen.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), AbdomenDirection);
            //Debug.Log(Vector3.Cross(new Vector3(0, 0, 1),new Vector3(1, 0, 0)));

            //因為用向量計算無法得到CHEST TARGET 所以目前先用CHEST
            //transformChest.right = new Vector3(1, 0, 0);



            Vector3 ChestDirection = vector3RightArm - vector3Chest;
            //Vector3 ChestNewDirection = Vector3.RotateTowards(transformChest.transform.right,new Vector3(ChestDirection.x, ChestDirection.y-10, ChestDirection.z), 360, 0.0f);
            //Vector3.Cross(AbdomenDirection, ChestDirection);
            transformChest.rotation = Quaternion.LookRotation(Vector3.Cross(ChestDirection , AbdomenDirection), AbdomenDirection);
            
            Vector3 RightArmDirection = vector3RightForeArm - vector3RightArm;
            //Vector3 RightArmNewDirection = Vector3.RotateTowards(transformRightArm.transform.right, RightArmDirection, 180, 0.0f);
            transformRightArm.right = RightArmDirection;

            Vector3 RightForeArmDirection = vector3RightHand - vector3RightForeArm;
            //Vector3 RightForeArmNewDirection = Vector3.RotateTowards(transformRightForeArm.transform.right, RightForeArmDirection, 180, 0.0f);
            transformRightForeArm.right = RightForeArmDirection;
            
            Vector3 LeftArmDirection = vector3LeftForeArm - vector3LeftArm;
            //Vector3 LeftArmNewDirection = Vector3.RotateTowards(transformLeftArm.transform.right * -1, LeftArmDirection, 180, 0.0f);
            transformLeftArm.right = LeftArmDirection * -1;

            Vector3 LeftForeArmDirection = vector3LeftHand - vector3LeftForeArm;
            //Vector3 LeftForeArmNewDirection = Vector3.RotateTowards(transformLeftForeArm.transform.right * -1, LeftForeArmDirection, 180, 0.0f);
            transformLeftForeArm.right = LeftForeArmDirection * -1;
            
            Vector3 RightUpLegDirection = vector3RightLeg - vector3RightUpLeg;
            //Vector3 RightUpLegNewDirection = Vector3.RotateTowards(transformRightUpLeg.transform.up * -1, RightUpLegDirection, 180, 0.0f);
            transformRightUpLeg.up = RightUpLegDirection * -1;

            Vector3 RightLegDirection = vector3RightFoot - vector3RightLeg;
            //Vector3 RightLegNewDirection = Vector3.RotateTowards(transformRightLeg.transform.up * -1, RightLegDirection, 180, 0.0f);
            transformRightLeg.up = RightLegDirection * -1;

            Vector3 LeftUpLegDirection = vector3LeftLeg - vector3LeftUpLeg;
            //Vector3 LeftUpLegNewDirection = Vector3.RotateTowards(transformLeftUpLeg.transform.up * -1, LeftUpLegDirection, 180, 0.0f);
            transformLeftUpLeg.up = LeftUpLegDirection * -1;

            Vector3 LeftLegDirection = vector3LeftFoot - vector3LeftLeg;
            //Vector3 LeftLegNewDirection = Vector3.RotateTowards(transformLeftLeg.transform.up * -1, LeftLegDirection, 180, 0.0f);
            transformLeftLeg.up = LeftLegDirection * -1;
            
        }
        //用給定位置改變人體模型位置
        else
        {
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
        }



        
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
    //CSV資料
    public void SetFloatArray(float[][] Array)
    {
        FloatArray = Array;
    }

    //JSON資料
    public void SetJsonBodyData(bodiesList bodyList)
    {
        m_bodyList = bodyList;
    }
    /*****************取出資料中的關節部位資訊*************************/
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



    private void CompareJsonData(float[][] array)
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

    /**********未來資料精準或換成智慧衣後就可不必使用此部分之函式 此部分用於資料校準 將位置相差較大的部分篩選並進行更改或刪除**********/

    /*****************取出資料中的關節部位資訊*************************/

}
