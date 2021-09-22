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
    [Header("左手")]
    private Transform transformLeftArm = null;
    [SerializeField]
    [Header("左手前臂")]
    private Transform transformLeftForeArm = null;
    [SerializeField]
    [Header("右大腿")]
    private Transform transformRightUpLeg = null;
    [SerializeField]
    [Header("右小腿")]
    private Transform transformRightLeg = null;
    [SerializeField]
    [Header("左大腿")]
    private Transform transformLeftUpLeg = null;
    [SerializeField]
    [Header("左小腿")]
    private Transform transformLeftLeg = null;

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
    /// -1:無 0:腹部 1:胸部 2:右手 3:右手前臂 4:左手 5:左手前臂 6:右大腿 7:右小腿 8:左大腿 9:左小腿
    /// </summary>
    [SerializeField]
    private int intBodyPartNum = -1;

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

            if(temp== intBodyPartNum)
            {
                booleanCameraCheck = false;
                floatCameraTimer = 5;
            }
            else booleanCameraCheck = false;

        }
        //booleanCameraCheck為FALSE 代表前面已有判斷過肢體差距或尚未進行判斷 因此以判斷過的數字進行動作
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
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftArmDistance(), transformLeftArm);
            }
            else if (intBodyPartNum == 5)
            {           
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance(), transformLeftForeArm);
            }
            else if (intBodyPartNum == 6)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightUpLegDistance(), transformRightUpLeg);
            }
            else if (intBodyPartNum == 7)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetRightLegDistance(), transformRightLeg);
            }
            else if (intBodyPartNum == 8)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance(), transformLeftUpLeg);
            }
            else if (intBodyPartNum == 9)
            {
                CameraLookAtBodyPart(Posture_Compute_Mgr.Instance.GetLeftLegDistance(), transformLeftLeg);
            }
            else
            {
                this.transform.position = Vector3.Slerp(this.transform.position, v3CameraStartPostion, 0.02f);
                
                booleanCameraCheck = false;
                //當相機回歸到原本位置 重新面向前方
                if (this.transform.position.magnitude - v3CameraStartPostion.magnitude <=0.5 && this.transform.position.magnitude - v3CameraStartPostion.magnitude>=-0.5)
                {
                    this.transform.rotation= Quaternion.Lerp(this.transform.rotation, quaternionCameraStartRotation, 0.02f);
                    //this.transform.forward = v3CameraStartRotation;
                }
            }
        }

        
        floatCameraTimer -= Time.deltaTime;
        //Debug.Log(Posture_Compute_Mgr.Instance.GetAbdomenDistance()); 


    }

    /// <summary>
    /// 此函式為判斷距離的公式
    /// </summary>
    /// <param name="distance">兩個模型的部位位置相差多少</param>
    /// <returns></returns>
    private bool CheckVector3Distance(Vector3 distance)
    {
        //Debug.LogWarning(distance);
        if (distance.x >= 0.05 || distance.y >= 0.05 || distance.z >= 0.05) return true;
        else if(distance.x <= -0.05 || distance.y <= -0.05 || distance.z <= -0.05) return true;
        else return false;
    }

    /// <summary>
    /// 判斷各個部位與教練模型差距 調整相機位置
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="bodypart"></param>
    private void CameraLookAtBodyPart(Vector3 distance,Transform bodypart)
    {
        //Debug.LogWarning(distance.x+" "+distance.y + " "+distance.z + " " + bodypart);
        if (distance.x >= 0.05 || distance.x <= -0.05)
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

        else if (distance.y >= 0.05 || distance.y <= -0.05)
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
        else if (distance.z >= 0.05 || distance.z <= -0.05)
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
 
    }

    /// <summary>
    /// 判斷是哪個關節位置不正確(只取一個)
    /// </summary>
    private void CompareBodyPart()
    {
        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetAbdomenDistance()))
        {
            intBodyPartNum = 0;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetChestDistance()))
        {
            intBodyPartNum = 1;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightArmDistance()))
        {
            intBodyPartNum = 2;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightForeArmDistance()))
        {
            intBodyPartNum = 3;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftArmDistance()))
        {
            intBodyPartNum = 4;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance()))
        {
            intBodyPartNum = 5;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightUpLegDistance()))
        {
            intBodyPartNum = 6;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightLegDistance()))
        {
            intBodyPartNum = 7;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance()))
        {
            intBodyPartNum = 8;
        }
        else if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftLegDistance()))
        {
            intBodyPartNum = 9;
        }
        else
        {
            intBodyPartNum = -1;
        }
    }

    /// <summary>
    /// 判斷關節位置與教練姿勢差距並上色
    /// </summary>
    private void CompareBodyPartsAndDrawColor()
    {
        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetAbdomenDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformAbdomen.gameObject, "BodyPart").ToArray();
            //Debug.LogError(transformAbdomen.gameObject);
            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetChestDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformChest.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightArmDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightForeArmDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightForeArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftArmDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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



        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftForeArmDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftForeArm.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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

        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightUpLegDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightUpLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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



        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetRightLegDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformRightLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftUpLegDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftUpLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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


        if (CheckVector3Distance(Posture_Compute_Mgr.Instance.GetLeftLegDistance()))
        {
            GameObject[] temp = Posture_Compute_Mgr.Instance.FindGameObjectInChildLayerWithTag(transformLeftLeg.gameObject, "BodyPart").ToArray();

            for (int j = 0; j < temp.Length; j++)
            {
                Posture_Compute_Mgr.Instance.ChangeBodyPartToRed(temp[j].transform);
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

    }
}
