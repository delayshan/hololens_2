using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posture_Cam : MonoBehaviour
{
    [SerializeField]
    [Header("腹部")]
    private Transform transformAbdomen = null;
    [SerializeField]
    [Header("胸口")]
    private Transform transformChest = null;
    [SerializeField]
    [Header("右手")]
    private Transform transformRightArm = null;
    [SerializeField]
    [Header("右手前臂")]
    private Transform transformRightForeArm = null;
    [SerializeField]
    [Header("右手掌")]
    private Transform transformRightHand = null;
    [SerializeField]
    [Header("左手")]
    private Transform transformLeftArm = null;
    [SerializeField]
    [Header("左手前臂")]
    private Transform transformLeftForeArm = null;
    [SerializeField]
    [Header("左手掌")]
    private Transform transformLeftHand = null;
    [SerializeField]
    [Header("右大腿")]
    private Transform transformRightUpLeg = null;
    [SerializeField]
    [Header("右小腿")]
    private Transform transformRightLeg = null;
    [SerializeField]
    [Header("右腳踝")]
    private Transform transformRightFoot = null;
    [SerializeField]
    [Header("左大腿")]
    private Transform transformLeftUpLeg = null;
    [SerializeField]
    [Header("左小腿")]
    private Transform transformLeftLeg = null;
    [SerializeField]
    [Header("左腳踝")]
    private Transform transformLeftFoot = null;

    /// <summary>
    /// 使用者關節與教練機的位置差距以多少為基準為差距大(標為紅色)
    /// </summary>
    [SerializeField]
    [Header("距離差距大之數字")]
    private float floatBigDifferent = 0.05f;

    /// <summary>
    /// 使用者關節與教練機的位置差距以多少為基準為差距小(標為黃色)
    /// </summary>
    [SerializeField]
    [Header("距離差距小之數字")]
    private float floatSmallDifferent = 0.03f;

    private Vector3 v3CameraStartPostion = Vector3.zero;
    private Quaternion quaternionCameraStartRotation = Quaternion.identity;
    [SerializeField]
    private float floatCameraTimer = 5;
    /// <summary>
    /// 相機是否可以移動
    /// </summary>
    [SerializeField]
    private bool booleanCameraCheck=false;
    /// <summary>
    /// 目前相機在以誰為主移動
    /// -1:無 0:腹部 1:胸部 2:右手臂 3:右手前臂 4.右手掌 5:左手臂 6:左手前臂 7:左手掌 8:右大腿 9:右小腿 10:右腳踝 11:左大腿 12:左小腿 13:左腳踝
    /// </summary>
    [SerializeField]
    private int intBodyPartNum = -1;

    /// <summary>
    /// 差距極小的身體部位 控制相機看著但不移動
    /// -1:無 0:腹部 1:胸部 2:右手臂 3:右手前臂 4.右手掌 5:左手臂 6:左手前臂 7:左手掌 8:右大腿 9:右小腿 10:右腳踝 11:左大腿 12:左小腿 13:左腳踝
    /// </summary>
    [SerializeField]
    private int intSmallDifBodyPartNum = -1;

    // Start is called before the first frame update
    void Start()
    {
        intBodyPartNum = -1;
        floatCameraTimer = 5;
        booleanCameraCheck = false;
        v3CameraStartPostion = this.transform.position;
        quaternionCameraStartRotation = this.transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        CompareBodyPartsAndDrawColor();


        /**因骨架資料為SCALE乘200才正常顯示 因此先暫時調整相機位置**/
        if (Input.GetKeyDown(KeyCode.U))
        {
            this.SetCam();
        }
    }

    void LateUpdate()
    {
        if(booleanCameraCheck == false && floatCameraTimer <= 0)
        {
            booleanCameraCheck = true;
            floatCameraTimer = 5;
        }


        //看之後要不要改成以CompareBodyPart為主判斷
        if (booleanCameraCheck == true)
        {
            int temp = intBodyPartNum;
            CompareBodyPart();
            //若為同個部位 繼續保持拍攝 並重製TIMER
            if(temp== intBodyPartNum)
            {
                booleanCameraCheck = false;
                floatCameraTimer = 5;
            }
            else booleanCameraCheck = false;

        }
        //booleanCameraCheck為FALSE 代表前面已有判斷過肢體差距或從未進行判斷 因此以判斷過的數字進行動作
        //當有過大差距時intBodyPartNum會有數字 若有則相機會移動且看著該部位
        //intBodyPartNum沒數字時會接著判斷intSmallDifBodyPartNum是否有數字 代表有些微差距的部分 
        else if (booleanCameraCheck == false)
        {
            if (intBodyPartNum == 0)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetAbdomenDistance(), transformAbdomen);
            }
            else if (intBodyPartNum == 1)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetChestDistance(), transformChest);
            }
            else if (intBodyPartNum == 2)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightArmDistance(), transformRightArm);
            }
            else if (intBodyPartNum == 3)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightForeArmDistance(), transformRightForeArm);
            }
            else if (intBodyPartNum == 4)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightHandDistance(), transformRightHand);
            }
            else if (intBodyPartNum == 5)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftArmDistance(), transformLeftArm);
            }
            else if (intBodyPartNum == 6)
            {           
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance(), transformLeftForeArm);
            }
            else if (intBodyPartNum == 7)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftHandDistance(), transformLeftHand);
            }
            else if (intBodyPartNum == 8)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightUpLegDistance(), transformRightUpLeg);
            }
            else if (intBodyPartNum == 9)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightLegDistance(), transformRightLeg);
            }
            else if (intBodyPartNum == 10)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightFootDistance(), transformRightFoot);
            }
            else if (intBodyPartNum == 11)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance(), transformLeftUpLeg);
            }
            else if (intBodyPartNum == 12)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftLegDistance(), transformLeftLeg);
            }
            else if (intBodyPartNum == 13)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftFootDistance(), transformLeftFoot);
            }
            else
            {
                this.transform.position = Vector3.Slerp(this.transform.position, v3CameraStartPostion, 0.02f);
                booleanCameraCheck = false;

                //當相機回歸到原本位置 重新面向前方
                //if (this.transform.position.magnitude - v3CameraStartPostion.magnitude <=0.5 && this.transform.position.magnitude - v3CameraStartPostion.magnitude>=-0.5)               
                // Debug.LogWarning(this.transform.position.magnitude - v3CameraStartPostion.magnitude);

                //相機回歸後 將檢視是否有細微偏差的部位 有的話則看著該部位
                if (intSmallDifBodyPartNum == 0)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetAbdomenDistance(), transformAbdomen);
                }
                else if (intSmallDifBodyPartNum == 1)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetChestDistance(), transformChest);
                }
                else if (intSmallDifBodyPartNum == 2)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightArmDistance(), transformRightArm);
                }
                else if (intSmallDifBodyPartNum == 3)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightForeArmDistance(), transformRightForeArm);
                }
                else if (intSmallDifBodyPartNum == 4)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightHandDistance(), transformRightHand);
                }
                else if (intSmallDifBodyPartNum == 5)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftArmDistance(), transformLeftArm);
                }
                else if (intSmallDifBodyPartNum == 6)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance(), transformLeftForeArm);
                }
                else if (intSmallDifBodyPartNum == 7)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftHandDistance(), transformLeftHand);
                }
                else if (intSmallDifBodyPartNum == 8)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightUpLegDistance(), transformRightUpLeg);
                }
                else if (intSmallDifBodyPartNum == 9)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightLegDistance(), transformRightLeg);
                }
                else if (intSmallDifBodyPartNum == 10)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightFootDistance(), transformRightFoot);
                }
                else if (intSmallDifBodyPartNum == 11)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance(), transformLeftUpLeg);
                }
                else if (intSmallDifBodyPartNum == 12)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftLegDistance(), transformLeftLeg);
                }
                else if (intSmallDifBodyPartNum == 13)
                {
                    CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftFootDistance(), transformLeftFoot);
                }
                else
                {
                    //若沒有重大差距或細微差距的部位 則轉回預設視角
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, quaternionCameraStartRotation, 0.05f);
                }
                    
                //this.transform.forward = v3CameraStartRotation;
                
            }
        }

        
        floatCameraTimer -= Time.deltaTime;
        //Debug.Log(Posture_Compute_Mgr.Instance.GetAbdomenDistance()); 


    }

    /// <summary>
    /// 此函式為判斷距離的公式 -1代表無差距 0代表差距細微 1代表差距大
    /// </summary>
    /// <param name="distance">兩個模型的部位位置相差多少</param>
    /// <returns></returns>
    private int CheckVector3Distance(Vector3 distance)
    {
        //Debug.LogWarning(distance);
        if (distance.x >= floatBigDifferent || distance.y >= floatBigDifferent || distance.z >= floatBigDifferent) return 1;
        else if(distance.x <= -floatBigDifferent || distance.y <= -floatBigDifferent || distance.z <= -floatBigDifferent) return 1;
        else if ((distance.x <= -floatSmallDifferent && distance.x >= -floatBigDifferent) || (distance.y <= -floatSmallDifferent && distance.y >= -floatBigDifferent) || (distance.z <= -floatSmallDifferent && distance.z >= -floatBigDifferent)) return 0;
        else if((distance.x >= floatSmallDifferent && distance.x <= floatBigDifferent) || (distance.y >= floatSmallDifferent && distance.y <= floatBigDifferent) || (distance.z >= floatSmallDifferent && distance.z <= floatBigDifferent)) return 0;
        else return -1;
    }

    /// <summary>
    /// 判斷各個部位與教練模型差距(包含細微差距) 調整相機位置
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="bodypart"></param>
    private void CameraLookAtBodyPart(Vector3 distance,Transform bodypart)
    {
        //Debug.LogWarning(distance.x+" "+distance.y + " "+distance.z + " " + bodypart);
        if (distance.x >= floatBigDifferent || distance.x <= -floatBigDifferent)
        {
            this.transform.LookAt(bodypart);
            if (distance.x > 0)
            {
                
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(bodypart.transform.position.x + 1f, bodypart.transform.position.y+0.1f, bodypart.transform.position.z + 1f), 0.02f);
            }
            else
            {
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(bodypart.transform.position.x - 1f, bodypart.transform.position.y+0.1f, bodypart.transform.position.z + 1f), 0.02f);
            }
        }

        else if (distance.y >= floatBigDifferent || distance.y <= -floatBigDifferent)
        {
            this.transform.LookAt(bodypart);
            if (distance.y > 0)
            {
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(bodypart.transform.position.x + 0.5f, bodypart.transform.position.y + 0.5f, bodypart.transform.position.z + 1f), 0.02f);
            }
            else
            {
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(bodypart.transform.position.x - 0.5f, bodypart.transform.position.y - 0.5f, bodypart.transform.position.z + 1f), 0.02f);
            }
        }
        else if (distance.z >= floatBigDifferent || distance.z <= -floatBigDifferent)
        {
            this.transform.LookAt(bodypart);
            if (distance.z > 0)
            {
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(bodypart.transform.position.x + 0.5f, bodypart.transform.position.y+0.1f, bodypart.transform.position.z + 1.5f), 0.02f);
            }
            else
            {
                this.transform.position = Vector3.Slerp(this.transform.position, new Vector3(bodypart.transform.position.x - 0.5f, bodypart.transform.position.y+0.1f, bodypart.transform.position.z - 1.5f), 0.02f);
            }
        }
        //差距細微 CAMERA看著該位置 但不更改位置
        else if (CheckVector3Distance(distance) == 0)
        {
            Quaternion newLookTarget= Quaternion.LookRotation(bodypart.transform.position - this.transform.position);
            this.transform.rotation =Quaternion.Lerp(this.transform.rotation, newLookTarget, 0.05f);
        }
 
    }

    /// <summary>
    /// 判斷是哪個關節位置不正確(只取一個) 找到的關節會更改intBodyPartNum及intSmallDifBodyPartNum的值
    /// </summary>
    private void CompareBodyPart()
    {
        //判斷是否有嚴重偏差(紅色)與輕微偏差(黃色)
        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetAbdomenDistance())==1)
        {
            intBodyPartNum = 0;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetChestDistance()) == 1)
        {
            intBodyPartNum = 1;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightArmDistance()) == 1)
        {
            intBodyPartNum = 2;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightForeArmDistance()) == 1)
        {
            intBodyPartNum = 3;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightHandDistance()) == 1)
        {
            intBodyPartNum = 4;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftArmDistance()) == 1)
        {
            intBodyPartNum = 5;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance()) == 1)
        {
            intBodyPartNum = 6;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftHandDistance()) == 1)
        {
            intBodyPartNum = 7;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightUpLegDistance()) == 1)
        {
            intBodyPartNum = 8;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightLegDistance()) == 1)
        {
            intBodyPartNum = 9;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightFootDistance()) == 1)
        {
            intBodyPartNum = 10;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance()) == 1)
        {
            intBodyPartNum = 11;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftLegDistance()) == 1)
        {
            intBodyPartNum = 12;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftFootDistance()) == 1)
        {
            intBodyPartNum = 13;
        }
        else
        {
            intBodyPartNum = -1;

        }

        //些微偏差(黃色)
        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetAbdomenDistance()) == 0)
        {
            intSmallDifBodyPartNum = 0;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetChestDistance()) == 0)
        {
            intSmallDifBodyPartNum = 1;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightArmDistance()) == 0)
        {
            intSmallDifBodyPartNum = 2;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightForeArmDistance()) == 0)
        {
            intSmallDifBodyPartNum = 3;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightHandDistance()) == 0)
        {
            intSmallDifBodyPartNum = 4;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftArmDistance()) == 0)
        {
            intSmallDifBodyPartNum = 5;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance()) == 0)
        {
            intSmallDifBodyPartNum = 6;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftHandDistance()) == 0)
        {
            intSmallDifBodyPartNum = 7;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightUpLegDistance()) == 0)
        {
            intSmallDifBodyPartNum = 8;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightLegDistance()) == 0)
        {
            intSmallDifBodyPartNum = 9;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightFootDistance()) == 0)
        {
            intSmallDifBodyPartNum = 10;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance()) == 0)
        {
            intSmallDifBodyPartNum = 11;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftLegDistance()) == 0)
        {
            intSmallDifBodyPartNum = 12;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftFootDistance()) == 0)
        {
            intSmallDifBodyPartNum = 13;
        }
        else
        {
            intSmallDifBodyPartNum = -1;
        }
    }

    /// <summary>
    /// 判斷關節位置與教練姿勢差距並上色
    /// </summary>
    private void CompareBodyPartsAndDrawColor()
    {
        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetAbdomenDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformAbdomen.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }

        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetAbdomenDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformAbdomen.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformAbdomen.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetChestDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformChest.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }

        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetChestDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformChest.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformChest.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightArmDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightArmDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightArm.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightArm.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightForeArmDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightForeArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightForeArmDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightForeArm.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightForeArm.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightHandDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightHand.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightHandDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightHand.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightHand.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftArmDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftArmDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftArm.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftArm.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }



        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftForeArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            } 
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftForeArm.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftForeArm.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftHandDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftHand.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftHandDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftHand.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftHand.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightUpLegDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightUpLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightUpLegDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightUpLeg.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightUpLeg.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }



        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightLegDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightLegDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightLeg.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightLeg.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightFootDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightFoot.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightFootDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightFoot.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightFoot.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftUpLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftUpLeg.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftUpLeg.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftLegDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftLegDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftLeg.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftLeg.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftFootDistance()) == 1)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftFoot.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
            }
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftFootDistance()) == 0)
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftFoot.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToYellow(temp[j].transform);
            }
        }
        else
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftFoot.gameObject, "BodyPart").ToArray();
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToGreen(temp[j].transform);
            }
        }
    }



    /**因骨架資料為SCALE乘200才正常顯示 因此先暫時調整相機位置**/
    public void SetCam()
    {
        this.transform.position =new Vector3(transformAbdomen.position.x, transformAbdomen.position.y, transformAbdomen.position.z-100) ;
    }

}
