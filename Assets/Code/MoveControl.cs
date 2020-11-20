using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    public float MaxSpeed; 
    public float Speed;
    public List<Vector3> TargetPointList = new List<Vector3>();

    private Transform m_Transform;

    private const float ArriveRange = 1f;

    public bool IsMain;
    private bool IsRotation;


    // Start is called before the first frame update
    void Start()
    {
        m_Transform = this.transform;

        IsRotation = true;
        if (IsMain)
        {
            IsRotation = false;
            if (Speed == 0)
            {
                Speed = 1;
            }
            FinalControl.AddSettingOutPointEvent += AddSettingOutPointListener;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetPointList.Count > 0)
        {
            Vector3 targetPoint = TargetPointList[0];
            Vector3 selfPoint = m_Transform.position;

            Vector3 newPosition = Vector3.Lerp(selfPoint, targetPoint, Time.deltaTime * Speed);

            Vector3 distance = newPosition - selfPoint;
            if(Mathf.Abs(distance.x) > MaxSpeed)
            {
                newPosition.x = selfPoint.x + MaxSpeed * (distance.x > 0 ? 1 : -1);
            }
            if (Mathf.Abs(distance.z) > MaxSpeed)
            {
                newPosition.z = selfPoint.z + MaxSpeed * (distance.z > 0 ? 1 : -1);
            }
            newPosition.y = 0;

            if (IsRotation)
            {
                m_Transform.LookAt(newPosition);
            }
            m_Transform.position = newPosition;

            if (GameUtil.IsArrive(newPosition, targetPoint, ArriveRange))
            {
                TargetPointList.RemoveAt(0);

                if(IsMain)
                {
                    FinalControl.WalkPointEvent(targetPoint);
                }
            }
        }
    }

    public void AddSettingOutPointListener(Vector3 point)
    {
        TargetPointList.Add(point);
    }
   
}
