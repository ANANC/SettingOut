using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Distance;
    public float Speed;
    public float Inclined;

    public float ArriveRange = 0.01f;

    private Transform m_Transform;


    // Start is called before the first frame update
    void Start()
    {
        m_Transform = this.transform;

    }

    // Update is called once per frame
    void Update()
    {
        //位置
        Vector3 targetPosition = Target.position + new Vector3(Distance.x, Target.up.y * Distance.y, Distance.z);
        Vector3 newPosition = Vector3.Lerp(m_Transform.position, targetPosition, Speed);
        m_Transform.position = newPosition;

        if (GameUtil.IsArrive(m_Transform.position, targetPosition, ArriveRange))
        {
            m_Transform.position = targetPosition;
        }

        //角度
        Quaternion quaternion = m_Transform.rotation;
        Vector3 eulerAngles = quaternion.eulerAngles;
        eulerAngles.x = Inclined;
        m_Transform.rotation = quaternion;
    }
}
