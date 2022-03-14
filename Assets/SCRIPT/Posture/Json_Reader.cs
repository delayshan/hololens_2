using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class BodyAxis
{
    public float x;
    public float y;
    public float z;
    public float c;
}
[System.Serializable]
public class bodies
{
    public BodyAxis Nose;
    public BodyAxis Neck;
    public BodyAxis RShoulder;
    public BodyAxis RElbow;
    public BodyAxis RWrist;
    public BodyAxis LShoulder;
    public BodyAxis LElbow;
    public BodyAxis LWrist;
    public BodyAxis MidHip;
    public BodyAxis RHip;
    public BodyAxis RKnee;
    public BodyAxis RAnkle;
    public BodyAxis LHip;
    public BodyAxis LKnee;
    public BodyAxis LAnkle;
}
[System.Serializable]
public class bodiesList
{
    public bodies[] bodies;
}




public class Json_Reader : MonoBehaviour
{
    [SerializeField]
    public bodiesList m_BodyData = new bodiesList();

    public User_posture m_UserPosture = null;
    public User_posture m_CoachPosture = null;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            string FileContent = null;
            TextAsset binAsset = new TextAsset();

            //用FileStream讀取csv二進位制檔案
            if (File.Exists(Application.persistentDataPath + "/mjson.json"))
            {
                /*
                using (FileStream fs = new FileStream(Application.persistentDataPath + "/data.csv", FileMode.Open))
                {
                    
                }
                */
                FileContent = File.ReadAllText(Application.persistentDataPath + "/mjson.json");
                binAsset = new TextAsset(FileContent);

                m_BodyData = JsonUtility.FromJson<bodiesList>(binAsset.text);
                Debug.LogWarning(binAsset);
                Debug.LogWarning(m_BodyData.bodies[0].Nose.x);
            }
            else
            {
                Debug.LogWarning(Application.persistentDataPath + "/mjson.json can't be read");
            }


            m_UserPosture.SetJsonBodyData(m_BodyData);
            m_CoachPosture.SetJsonBodyData(m_BodyData);


        }
    }
}
