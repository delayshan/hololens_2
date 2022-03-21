using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posture_Compute_Mgr : MonoBehaviour
{
    public static Posture_Compute_Mgr Instance = null;

    /*這些部位之後看要不要改成自動抓部位 而不用手動拉*/

    [SerializeField][Header("使用者姿勢")]
    private User_posture UPUserPosture = null;

    [SerializeField][Header("教練姿勢")]
    private User_posture UPCoachPosture = null;

    [SerializeField][Header("半透明綠色")]
    private Material materialTransParentGreen = null;

    [SerializeField][Header("半透明黃色")]
    private Material materialTransParentYellow = null;

    [SerializeField][Header("半透明紅色")]
    private Material materialTransParentRed = null;

    [SerializeField][Header("半透明藍色")]
    private Material materialTransParentBlue = null;

    private GameObject[] gameobjectUserBodyParts =null;

    private GameObject[] gameobjectCoachBodyParts = null;

    private GameObject[] gameobjectUserBodyMeshModel = null;

    private GameObject[] gameobjectCoachBodyMeshModel = null;

    private void Awake()
    {
        if (Posture_Compute_Mgr.Instance != null)
        {
            return;
        }

        Posture_Compute_Mgr.Instance = this.GetComponent<Posture_Compute_Mgr>();
    }

    private void OnDestroy()
    {
        if (Posture_Compute_Mgr.Instance != null)
        {
            Posture_Compute_Mgr.Instance = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameobjectUserBodyParts= FindGameObjectInChildrenWithTag(UPUserPosture.gameObject,"BodyPart").ToArray();
        gameobjectCoachBodyParts= FindGameObjectInChildrenWithTag(UPCoachPosture.gameObject, "BodyPart").ToArray();
        gameobjectUserBodyMeshModel = FindGameObjectInChildrenWithTag(UPUserPosture.gameObject, "BodyMeshModel").ToArray();
        gameobjectCoachBodyMeshModel = FindGameObjectInChildrenWithTag(UPCoachPosture.gameObject, "BodyMeshModel").ToArray();
        for(int i=0;i< gameobjectUserBodyParts.Length; i++)
        {
            gameobjectUserBodyParts[i].GetComponent<Renderer>().material = materialTransParentGreen;
        }
        for (int i = 0; i < gameobjectCoachBodyParts.Length; i++)
        {
            gameobjectCoachBodyParts[i].GetComponent<Renderer>().material = materialTransParentBlue;
        }

        //HOLOLENS暫時設定
        for (int i = 0; i < gameobjectCoachBodyParts.Length; i++)
        {
            gameobjectCoachBodyParts[i].SetActive(false);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(gameobjectUserBodyParts==null) Debug.LogError("沒有TAG為BodyPart的使用者骨架");
            else if (gameobjectUserBodyParts[0].active)
            {
                Debug.Log("取消顯示使用者骨架");
                for (int i = 0; i < gameobjectUserBodyParts.Length; i++)
                {
                    gameobjectUserBodyParts[i].SetActive(false);
                }
            }
            else
            {
                Debug.Log("顯示使用者骨架");
                for (int i = 0; i < gameobjectUserBodyParts.Length; i++)
                {
                    gameobjectUserBodyParts[i].SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gameobjectCoachBodyParts == null) Debug.LogError("沒有TAG為BodyPart的教練骨架");
            else if (gameobjectCoachBodyParts[0].active)
            {
                Debug.Log("取消顯示教練骨架");
                for (int i = 0; i < gameobjectCoachBodyParts.Length; i++)
                {
                    gameobjectCoachBodyParts[i].SetActive(false);
                }
            }
            else
            {
                Debug.Log("顯示教練骨架");
                for (int i = 0; i < gameobjectCoachBodyParts.Length; i++)
                {
                    gameobjectCoachBodyParts[i].SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (gameobjectUserBodyMeshModel == null) Debug.LogError("沒有TAG為BodyMeshModel的使用者人體模型");
            else if (gameobjectUserBodyMeshModel[0].active)
            {
                Debug.Log("取消顯示使用者人體模型");
                for (int i = 0; i < gameobjectUserBodyMeshModel.Length; i++)
                {
                    gameobjectUserBodyMeshModel[i].SetActive(false);
                }
            }
            else
            {
                Debug.Log("顯示使用者人體模型");
                for (int i = 0; i < gameobjectUserBodyMeshModel.Length; i++)
                {
                    gameobjectUserBodyMeshModel[i].SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (gameobjectCoachBodyMeshModel == null) Debug.LogError("沒有TAG為BodyMeshModel的教練人體模型");
            else if (gameobjectCoachBodyMeshModel[0].active)
            {
                Debug.Log("取消顯示教練人體模型");
                for (int i = 0; i < gameobjectCoachBodyMeshModel.Length; i++)
                {
                    gameobjectCoachBodyMeshModel[i].SetActive(false);
                }
            }
            else
            {
                Debug.Log("顯示教練人體模型");
                for (int i = 0; i < gameobjectCoachBodyMeshModel.Length; i++)
                {
                    gameobjectCoachBodyMeshModel[i].SetActive(true);
                }
            }
        }
    }

    /// <summary>
    ///尋找指定物件之所有子物件是否有該TAG
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="tagname"></param>
    /// <returns></returns>
    private List<GameObject> FindGameObjectInChildrenWithTag(GameObject gameObject,string tagname)
    {
        List<GameObject> childWithTag = new List<GameObject>();
        for(int children=0;children< gameObject.transform.childCount; children++)
        {
            //Debug.LogWarning(gameObject.name);
            //Debug.LogWarning(gameObject.transform.GetChild(children).gameObject.name);
            if (gameObject.transform.GetChild(children).tag == tagname)
            {
                childWithTag.Add(gameObject.transform.GetChild(children).gameObject);
            }
            if (gameObject.transform.GetChild(children).childCount != 0)
            {
                childWithTag.AddRange(FindGameObjectInChildrenWithTag(gameObject.transform.GetChild(children).gameObject, tagname));
                //List<GameObject> temp = FindGameObjectInChildrenWithTag(gameObject.transform.GetChild(children).gameObject, tagname);
                //Debug.LogWarning(gameObject.transform.GetChild(children).gameObject.name);
            }
            

        }
        return childWithTag;
    }

    /// <summary>
    /// 尋找指定物件之一層子物件是否有該TAG
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="tagname"></param>
    /// <returns></returns>
    public List<GameObject> FindGameObjectInChildLayerWithTag(GameObject gameObject, string tagname)
    {
        List<GameObject> childWithTag = new List<GameObject>();
        for (int children = 0; children < gameObject.transform.childCount; children++)
        {
            //Debug.LogWarning(gameObject.name);
            //Debug.LogWarning(gameObject.transform.childCount);
            //Debug.LogWarning(gameObject.transform.GetChild(children).gameObject.name);
            if (gameObject.transform.GetChild(children).tag == tagname)
            {
                //Debug.LogWarning("B"+ gameObject.transform.GetChild(children).gameObject.name);
                childWithTag.Add(gameObject.transform.GetChild(children).gameObject);
            }
        }
        return childWithTag;
    }

    /////////////////////////計算使用者各個關節與教練關節差距/////////////////////////////////////
    public Vector3 GetAbdomenDistance()
    {
        return UPUserPosture.GetAbdomenPostion().position - UPCoachPosture.GetAbdomenPostion().position;
    }
    public Vector3 GetChestDistance()
    {
        return UPUserPosture.GetChestPostion().position - UPCoachPosture.GetChestPostion().position;
    }
    public Vector3 GetRightArmDistance()
    {
        return UPUserPosture.GetRightArmPostion().position - UPCoachPosture.GetRightArmPostion().position;
    }
    public Vector3 GetRightForeArmDistance()
    {
        return UPUserPosture.GetRightForeArmPostion().position - UPCoachPosture.GetRightForeArmPostion().position;
    }
    public Vector3 GetRightHandDistance()
    {
        return UPUserPosture.GetRightHandPostion().position - UPCoachPosture.GetRightHandPostion().position;
    }
    public Vector3 GetLeftArmDistance()
    {
        return UPUserPosture.GetLeftArmPostion().position - UPCoachPosture.GetLeftArmPostion().position;
    }
    public Vector3 GetLeftForeArmDistance()
    {
        return UPUserPosture.GetLeftForeArmPostion().position - UPCoachPosture.GetLeftForeArmPostion().position;
    }
    public Vector3 GetLeftHandDistance()
    {
        return UPUserPosture.GetLeftHandPostion().position - UPCoachPosture.GetLeftHandPostion().position;
    }
    public Vector3 GetRightUpLegDistance()
    {
        return UPUserPosture.GetRightUpLegPostion().position - UPCoachPosture.GetRightUpLegPostion().position;
    }
    public Vector3 GetRightLegDistance()
    {
        return UPUserPosture.GetRightLegPostion().position - UPCoachPosture.GetRightLegPostion().position;
    }
    public Vector3 GetRightFootDistance()
    {
        return UPUserPosture.GetRightFootPostion().position - UPCoachPosture.GetRightFootPostion().position;
    }
    public Vector3 GetLeftUpLegDistance()
    {
        return UPUserPosture.GetLeftUpLegPostion().position - UPCoachPosture.GetLeftUpLegPostion().position;
    }
    public Vector3 GetLeftLegDistance()
    {
        return UPUserPosture.GetLeftLegPostion().position - UPCoachPosture.GetLeftLegPostion().position;
    }
    public Vector3 GetLeftFootDistance()
    {
        return UPUserPosture.GetLeftFootPostion().position - UPCoachPosture.GetLeftFootPostion().position;
    }
    /////////////////////////計算使用者各個關節與教練關節差距/////////////////////////////////////
    ///////////////////////////////////改變模型顏色///////////////////////////////////////////////

    public void ChangeBodyPartToGreen(Transform BodyPart)
    {
        BodyPart.GetComponent<Renderer>().material = materialTransParentGreen;
    }
    public void ChangeBodyPartToYellow(Transform BodyPart)
    {
        BodyPart.GetComponent<Renderer>().material = materialTransParentYellow;
    }

    public void ChangeBodyPartToRed(Transform BodyPart)
    {
        BodyPart.GetComponent<Renderer>().material = materialTransParentRed;
    }


    ///////////////////////////////////改變模型顏色///////////////////////////////////////////////
}
