using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKcontrl : MonoBehaviour
{
    Animator anim;

    public float ikWeight=1;

    public Transform leftIKtarget;
    public Transform rightIKtarget;
    public GameObject m_bone;
    public Vector3 m_transform;
    public Quaternion rotation;




    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       // m_bone.transform.localPosition = m_transform;
        
    }

    private void OnAnimatorIK()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeight);


        anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftIKtarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightFoot, rightIKtarget.position);
        //anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperLeg, rotation);
        
    }

    private void LateUpdate()
    {
        m_bone.transform.localPosition = m_transform;
    }

}
