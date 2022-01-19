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
    public User_posture m_CoachPosture = null;




    private float[][] FloatArray;



    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("A = " + Application.persistentDataPath + "/Test.MP4");
    }

    private void Update()
    {

        //從資料庫中抓取資料並存至persistentDataPath(暫不需要)
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(Application.persistentDataPath);
            GetModelData();
        }
        //重新讀取PREFAB (暫不需要)
        if (Input.GetKeyDown(KeyCode.L))
        {
            
            if (File.Exists(Application.persistentDataPath + "/m_fbx.fbx"))
            {
                using (FileStream fs = new FileStream(Application.persistentDataPath + "/m_fbx.fbx", FileMode.Open))
                {

                    BinaryFormatter b = new BinaryFormatter();
                    b.Serialize(fs, prefab);

                    //byte[] data = request.downloadHandler.data;
                    //fs.Read(data, 0, data.Length);
                    Instantiate(prefab);
                }
                
            }
            else
            {
                Debug.LogWarning(Application.persistentDataPath + "/m_fbx.fbx can't be read");
            }
            GetModelData();
        }
        //讀取身體位置資訊
        if (Input.GetKeyDown(KeyCode.U))
        {
            string FileContent = null;
            TextAsset binAsset = new TextAsset();

            //用FileStream讀取csv二進位制檔案
            if (File.Exists(Application.persistentDataPath + "/data.csv"))
            {   
                /*
                using (FileStream fs = new FileStream(Application.persistentDataPath + "/data.csv", FileMode.Open))
                {
                    
                }
                */
                FileContent = File.ReadAllText(Application.persistentDataPath + "/data.csv");
                binAsset = new TextAsset(FileContent);
            }
            else
            {
                Debug.LogWarning(Application.persistentDataPath + "/data.txt can't be read");
            }

            //resources
            //TextAsset binAsset = Resources.Load("data", typeof(TextAsset)) as TextAsset;
            //讀取每一行的內容
            string[] lineArray = binAsset.text.Split('\r');
            //建立二維陣列
            string[][] Array = new string[lineArray.Length][];
            //建立浮點數二維陣列
            FloatArray = new float[lineArray.Length][];

            //Debug.LogWarning(lineArray.Length);
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
            //資料讀取完後 傳遞資料給模型
            m_UserPosture.SetFloatArray(FloatArray);
            m_CoachPosture.SetFloatArray(FloatArray);
        }
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
                m_TestID.text = request.error;
            }
            else
            {
                //m_TestID.text = request.downloadHandler.text;
                Debug.Log("request "+ request.downloadHandler.GetType());


                if (request.isDone)
                {
                    using (FileStream fs = new FileStream(Application.persistentDataPath+"/m_fbx.fbx", FileMode.Create))
                    //using (FileStream fs = new FileStream(Application.persistentDataPath + "/m_fAbx.fbx", FileMode.Create))
                    {

                        byte[] data = request.downloadHandler.data;
                        fs.Write(data, 0, data.Length);
                    }
                }
            }
        }
    }
}
