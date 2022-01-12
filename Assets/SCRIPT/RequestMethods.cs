using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class RequestMethods : MonoBehaviour
{

    public Text m_TestID;

    public string m_RequestName = "";

    public string savePath = "C:/Users/Ethan_lab/Documents/GitHub/MRProject/Assets/FBX/m_fbx.fbx";

    public Transform prefab;

    public User_posture m_UserPosture = null;

    /// <summary>
    /// 資料中的INDEX 奇數為X 偶數為Y 0為部位名稱
    /// </summary>
    private int check = 0;
    private int clock=-1;

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

    private float[][] FloatArray;


    /// <summary>
    /// 傳進來的資料對照User_posture各個部位，m_DataToBodyPart第0個要填入相對於DATA中腹部的欄位 以此類推。
    /// </summary>
    private int[] m_DataToBodyPart = { 9, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 15 };


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("A = " + Application.persistentDataPath + "/Test.MP4");
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.K))
        {
            GetModelData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            using (FileStream fs = new FileStream("C:/Users/Ethan_lab/Desktop/unity_projects/FBX_TESTING/Assets/FBX/m_fbx.fbx", FileMode.Open))
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fs, prefab);

                //byte[] data = request.downloadHandler.data;
                //fs.Read(data, 0, data.Length);
                Instantiate(prefab);
            }
            GetModelData();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            clock = 0;
            //讀取csv二進位制檔案
            TextAsset binAsset = Resources.Load("data", typeof(TextAsset)) as TextAsset;
            //讀取每一行的內容
            string[] lineArray = binAsset.text.Split('\r');
            //建立二維陣列
            string[][] Array = new string[lineArray.Length][];
            //建立浮點數二維陣列
            FloatArray = new float[lineArray.Length][];


            //把csv中的資料儲存在二位陣列中
            //Array[0]會是各部位的名稱
            //Array[][0]會是各部位的ID 因此都不使用
            for (int i = 0; i < lineArray.Length; i++)
            {

                //Split以逗號分開所有的字串 因此現在得到的資料會是[列][行]
                Array[i] = lineArray[i].Split(',');
                float[] temp = new float[Array[i].Length];


                for (int j = 0; j < Array[i].Length; j++)
                {
                    if (i == 0)
                    {
                        temp[j] = 0;
                    }
                    else if (j == 0)
                    {
                        temp[j] = 0;
                    }
                    else
                    {
                        temp[j] = float.Parse(Array[i][j]);
                    }


                    //FloatArray[i][j] = float.Parse(Array[i][j]);
                }
                FloatArray[i] = temp;
                //Debug.Log("A = " + FloatArray[i][1]);

            }

        }




        //10貞+1 CHECK(資料行數) 每2個(以X為準)CHECK進行一次位置處理
        if (clock >= 10)
        {

            //
            //Length-2 是因為CSV會多2行
            //
            if (FloatArray.Length - 2 > check)
            {
                
                if (check % 2 == 0 && check != 0 && check<4)
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
                    Chest =new Vector2(FloatArray[check - 1][m_DataToBodyPart[1]], FloatArray[check][m_DataToBodyPart[1]]);
                    RA = new Vector2(FloatArray[check - 1][m_DataToBodyPart[2]], FloatArray[check][m_DataToBodyPart[2]]);
                    RFA = new Vector2(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]]);
                    RH =new Vector2(FloatArray[check - 1][m_DataToBodyPart[4]], FloatArray[check][m_DataToBodyPart[4]]);
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
                    m_UserPosture.SetAbdomenPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[0]], FloatArray[check][m_DataToBodyPart[0]], 0));
                    m_UserPosture.SetChestPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[1]], FloatArray[check][m_DataToBodyPart[1]], 0));
                    m_UserPosture.SetRightArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[2]], FloatArray[check][m_DataToBodyPart[2]], 0));
                    m_UserPosture.SetRightForeArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]], RightForeArmPostion));
                    m_UserPosture.SetRightHandPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[4]], FloatArray[check][m_DataToBodyPart[4]], tempRightHandPostion));
                    m_UserPosture.SetLeftArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[5]], FloatArray[check][m_DataToBodyPart[5]], 0));
                    m_UserPosture.SetLeftForeArmPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[6]], FloatArray[check][m_DataToBodyPart[6]], LeftForeArmPostion));
                    m_UserPosture.SetLeftHandPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[7]], FloatArray[check][m_DataToBodyPart[7]], tempRightHandPostion));
                    m_UserPosture.SetRightUpLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[8]], FloatArray[check][m_DataToBodyPart[8]], 0));
                    m_UserPosture.SetRightLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[9]], FloatArray[check][m_DataToBodyPart[9]], 0));
                    m_UserPosture.SetRightFootPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[10]], FloatArray[check][m_DataToBodyPart[10]], 0));
                    m_UserPosture.SetLeftUpLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[11]], FloatArray[check][m_DataToBodyPart[11]], 0));
                    m_UserPosture.SetLeftLegPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[12]], FloatArray[check][m_DataToBodyPart[12]], 0));
                    m_UserPosture.SetLeftFootPostion(new Vector3(FloatArray[check - 1][m_DataToBodyPart[13]], FloatArray[check][m_DataToBodyPart[13]], 0));
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
        if(check % 2 == 0 && check >= 4 && check != 0)
        {



            //關節移動平滑
            Abdomen = Vector2.Lerp(Abdomen, new Vector2(FloatArray[check - 1][m_DataToBodyPart[0]], FloatArray[check][m_DataToBodyPart[0]]), 10f * Time.deltaTime);
            Chest = Vector2.Lerp(Chest, new Vector2(FloatArray[check - 1][m_DataToBodyPart[1]], FloatArray[check][m_DataToBodyPart[1]]), 10f * Time.deltaTime);
            RA = Vector2.Lerp(RA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[2]], FloatArray[check][m_DataToBodyPart[2]]), 10f * Time.deltaTime);
            RFA = Vector2.Lerp(RFA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[3]], FloatArray[check][m_DataToBodyPart[3]]), 10f * Time.deltaTime);
            RH = Vector2.Lerp(RH, new Vector2(FloatArray[check - 1][m_DataToBodyPart[4]], FloatArray[check][m_DataToBodyPart[4]]), 10f * Time.deltaTime);
            LA= Vector2.Lerp(LA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[5]], FloatArray[check][m_DataToBodyPart[5]]), 10f * Time.deltaTime);
            LFA = Vector2.Lerp(LFA, new Vector2(FloatArray[check - 1][m_DataToBodyPart[6]], FloatArray[check][m_DataToBodyPart[6]]), 10f * Time.deltaTime);
            LH= Vector2.Lerp(LH, new Vector2(FloatArray[check - 1][m_DataToBodyPart[7]], FloatArray[check][m_DataToBodyPart[7]]), 10f * Time.deltaTime);
            RUL= Vector2.Lerp(RUL, new Vector2(FloatArray[check - 1][m_DataToBodyPart[8]], FloatArray[check][m_DataToBodyPart[8]]), 10f * Time.deltaTime);
            RL = Vector2.Lerp(RL, new Vector2(FloatArray[check - 1][m_DataToBodyPart[9]], FloatArray[check][m_DataToBodyPart[9]]), 10f * Time.deltaTime);
            RF= Vector2.Lerp(RF, new Vector2(FloatArray[check - 1][m_DataToBodyPart[10]], FloatArray[check][m_DataToBodyPart[10]]), 10f * Time.deltaTime);
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
            
            m_UserPosture.SetAbdomenPostion(new Vector3(Abdomen.x, Abdomen.y, 0));
            m_UserPosture.SetChestPostion(new Vector3(Chest.x, Chest.y, 0));
            m_UserPosture.SetRightArmPostion(new Vector3(RA.x, RA.y, 0));
            m_UserPosture.SetRightForeArmPostion(new Vector3(RFA.x, RFA.y, RightForeArmPostion));
            m_UserPosture.SetRightHandPostion(new Vector3(RH.x, RH.y, RightForeArmPostion + 50));
            m_UserPosture.SetLeftArmPostion(new Vector3(LA.x, LA.y, 0));
            m_UserPosture.SetLeftForeArmPostion(new Vector3(LFA.x, LFA.y, LeftForeArmPostion));
            m_UserPosture.SetLeftHandPostion(new Vector3(LH.x, LH.y, LeftForeArmPostion + 50));
            m_UserPosture.SetRightUpLegPostion(new Vector3(RUL.x, RUL.y, 0));
            m_UserPosture.SetRightLegPostion(new Vector3(RL.x, RL.y, 0));
            m_UserPosture.SetRightFootPostion(new Vector3(RF.x, RF.y, 0));
            m_UserPosture.SetLeftUpLegPostion(new Vector3(LUL.x, LUL.y, 0));
            m_UserPosture.SetLeftLegPostion(new Vector3(LL.x, LL.y, 0));
            m_UserPosture.SetLeftFootPostion(new Vector3(LF.x, LF.y, 0));
        }
        else if (check % 2 == 1 && check >= 4 && check != 0)
        {


            //關節移動平滑
            Abdomen = Vector2.Lerp(Abdomen, new Vector2(FloatArray[check - 2][m_DataToBodyPart[0]], FloatArray[check-1][m_DataToBodyPart[0]]), 10f * Time.deltaTime);
            Chest = Vector2.Lerp(Chest, new Vector2(FloatArray[check - 2][m_DataToBodyPart[1]], FloatArray[check-1][m_DataToBodyPart[1]]), 10f * Time.deltaTime);
            RA = Vector2.Lerp(RA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[2]], FloatArray[check-1][m_DataToBodyPart[2]]), 10f * Time.deltaTime);
            RFA = Vector2.Lerp(RFA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[3]], FloatArray[check-1][m_DataToBodyPart[3]]), 10f * Time.deltaTime);
            RH = Vector2.Lerp(RH, new Vector2(FloatArray[check - 2][m_DataToBodyPart[4]], FloatArray[check-1][m_DataToBodyPart[4]]), 10f * Time.deltaTime);
            LA = Vector2.Lerp(LA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[5]], FloatArray[check-1][m_DataToBodyPart[5]]), 10f * Time.deltaTime);
            LFA = Vector2.Lerp(LFA, new Vector2(FloatArray[check - 2][m_DataToBodyPart[6]], FloatArray[check-1][m_DataToBodyPart[6]]), 10f * Time.deltaTime);
            LH = Vector2.Lerp(LH, new Vector2(FloatArray[check - 2][m_DataToBodyPart[7]], FloatArray[check-1][m_DataToBodyPart[7]]), 10f * Time.deltaTime);
            RUL = Vector2.Lerp(RUL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[8]], FloatArray[check-1][m_DataToBodyPart[8]]), 10f * Time.deltaTime);
            RL = Vector2.Lerp(RL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[9]], FloatArray[check-1][m_DataToBodyPart[9]]), 10f * Time.deltaTime);
            RF = Vector2.Lerp(RF, new Vector2(FloatArray[check - 2][m_DataToBodyPart[10]], FloatArray[check-1][m_DataToBodyPart[10]]), 10f * Time.deltaTime);
            LUL = Vector2.Lerp(LUL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[11]], FloatArray[check-1][m_DataToBodyPart[11]]), 10f * Time.deltaTime);
            LL = Vector2.Lerp(LL, new Vector2(FloatArray[check - 2][m_DataToBodyPart[12]], FloatArray[check-1][m_DataToBodyPart[12]]), 10f * Time.deltaTime);
            LF = Vector2.Lerp(LF, new Vector2(FloatArray[check - 2][m_DataToBodyPart[13]], FloatArray[check-1][m_DataToBodyPart[13]]), 10f * Time.deltaTime);

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
            
            m_UserPosture.SetAbdomenPostion(new Vector3(Abdomen.x, Abdomen.y, 0));
            m_UserPosture.SetChestPostion(new Vector3(Chest.x, Chest.y, 0));
            m_UserPosture.SetRightArmPostion(new Vector3(RA.x, RA.y, 0));
            m_UserPosture.SetRightForeArmPostion(new Vector3(RFA.x, RFA.y, RightForeArmPostion));
            m_UserPosture.SetRightHandPostion(new Vector3(RH.x, RH.y, RightForeArmPostion + 50));
            m_UserPosture.SetLeftArmPostion(new Vector3(LA.x, LA.y, 0));
            m_UserPosture.SetLeftForeArmPostion(new Vector3(LFA.x, LFA.y, LeftForeArmPostion));
            m_UserPosture.SetLeftHandPostion(new Vector3(LH.x, LH.y, LeftForeArmPostion + 50));
            m_UserPosture.SetRightUpLegPostion(new Vector3(RUL.x, RUL.y, 0));
            m_UserPosture.SetRightLegPostion(new Vector3(RL.x, RL.y, 0));
            m_UserPosture.SetRightFootPostion(new Vector3(RF.x, RF.y, 0));
            m_UserPosture.SetLeftUpLegPostion(new Vector3(LUL.x, LUL.y, 0));
            m_UserPosture.SetLeftLegPostion(new Vector3(LL.x, LL.y, 0));
            m_UserPosture.SetLeftFootPostion(new Vector3(LF.x, LF.y, 0));
            
        }

        if (clock>=0)
            clock += 1;




    }
    /*
    IEnumerator LoadTextureFromCache()
    {
        string url =  "C:/Users/Ethan_lab/Desktop/unity_projects/FBX_TESTING/Assets/FBX/m_fbx.fbx";

        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {

                prefab=uwr.downloadHandler ;
            }
        }
    }*/
    public void GetModelData()
    {
        StartCoroutine(GetModelDataCoroutine());
    }

    public void PutData()
    {
        //StartCoroutine(LoadTextureFromCache());
    }



    IEnumerator GetModelDataCoroutine()
    {
        //m_TestID.text = "Loading...";
        

        byte[] myData = System.Text.Encoding.UTF8.GetBytes("{}");

        string api = "http://140.121.197.98:8888/project1201/file/download/619caf8a1edd9913cee1cf48";

        using (UnityWebRequest request = UnityWebRequest.Get(api))
        {
            /*
            var dh = new DownloadHandlerFile(savePath);
            dh.removeFileOnAbort = true;
            request.downloadHandler = dh;
            */
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                //m_TestID.text = request.error;
            }
            else
            {


                //m_TestID.text = request.downloadHandler.text;
                Debug.Log("request "+ request.downloadHandler.GetType());


                if (request.isDone)
                {
                    Debug.Log("A = " + Application.persistentDataPath + "/Test.MP4");
                    using (FileStream fs = new FileStream("C:/Users/Ethan_lab/Desktop/unity_projects/FBX_TESTING/Assets/FBX/m_fbx.fbx", FileMode.Create))
                    //using (FileStream fs = new FileStream(Application.persistentDataPath + "/m_fAbx.fbx", FileMode.Create))
                    {

                        byte[] data = request.downloadHandler.data;
                        fs.Write(data, 0, data.Length);
                    }
                }

                
            }


        }
        

    }




    /**********未來資料精準或換成智慧衣後就可不必使用此部分之函式 此部分用於資料校準 將位置相差較大的部分篩選並進行更改或刪除**********/

    /// <summary>
    /// 若資料與前一筆資料相差過大，作對矩陣資料進行更改(因為為CALL BY REF 所以不回傳值)
    /// </summary>
    /// <param name="array"></param>
    private void CompareData(float[][] array)
    {
        //Debug.Log("check= " + check);
        if (check<=2)
        {
           //第一筆資料 不需比較
        }
        else
        {
            //若與前次位置相差過大 進行數據調整 使改變不一下過大
            for (int i=0;i< m_DataToBodyPart.Length; i++)
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

}
