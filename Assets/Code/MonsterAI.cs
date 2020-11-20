using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Vector2 MoveSize;
    public Transform MainPlayer;
    public float SpeedMin;
    public float SpeedMax;
    public float SeeLength;

    private MoveControl moveControl;
    private Transform m_Transform;
    private float Speed;
    private bool Floow;
    private bool FloowPlayer;
    private bool FloowPoint;
    private const float Interval = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Speed = Random.Range(SpeedMin, SpeedMax);
        if (Speed == 0)
        {
            Speed = 1;
        }
        Floow = true;
        FloowPoint = false;
        FloowPlayer = true;

        MainPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        m_Transform = this.transform;
        moveControl = this.GetComponent<MoveControl>();

        moveControl.Speed = Speed;
    }

    private void OnDestroy()
    {
        FinalControl.AddActivePointEvent -= AddActivePointListener;
        FinalControl.DestroyPointEvent -= DestroyPointListener;
    }

    // Update is called once per frame
    void Update()
    {
        if (Floow == false)
        {
            AddRandomPoint();

            Vector3 playerPosition = MainPlayer.position;
            Vector3 selfPosition = m_Transform.position;

            if (Vector3.Distance(playerPosition, selfPosition) > SeeLength)
            {
                return;
            }

          //  print("See Player!");
            moveControl.TargetPointList.Clear();

            Floow = true;
            FloowPlayer = true;
            FloowPoint = false;

        }

        if (FloowPlayer)
        {
            Vector3 selfPosition = m_Transform.position;

            float distance;
            moveControl.TargetPointList.Clear();
            moveControl.TargetPointList.Add(MainPlayer.position);

            distance = Vector3.Distance(MainPlayer.position, selfPosition);
            List<Vector3> activePositions = FinalControl.Instance.GetActivePoints();

            FloowPoint = false;
            int lastIndex = -1;
            for (int index = activePositions.Count-1; index >= 0; index--)
            {
                float pointDistance = Vector3.Distance(activePositions[index], selfPosition);
                if (pointDistance < distance)
                {
                   // print("FloowPoint!");
                    FloowPoint = true;
                    lastIndex = index;
                    break;
                }
            }

            if (FloowPoint)
            {
                FloowPlayer = false;

                moveControl.TargetPointList.Clear();
                for (int index = lastIndex; index <= activePositions.Count - 1; index++)
                {
                    moveControl.TargetPointList.Add(activePositions[index]);
                }
                   
                FinalControl.AddActivePointEvent += AddActivePointListener;
                FinalControl.DestroyPointEvent += DestroyPointListener;
            }
        }

        if(FloowPoint)
        {
            if(moveControl.TargetPointList.Count == 0)
            {
               // print("巡逻!");

               // Floow = false;
                FloowPlayer = true;
               // FloowPoint = false;

                FinalControl.AddActivePointEvent -= AddActivePointListener;
                FinalControl.DestroyPointEvent -= DestroyPointListener;
            }
        }
    }

    public void AddActivePointListener(Vector3 point)
    {
        moveControl.TargetPointList.Add(point);
    }

    public void DestroyPointListener(Vector3 point)
    {
        moveControl.TargetPointList.Remove(point);
    }


    public void AddRandomPoint()
    {
        if (moveControl.TargetPointList.Count != 0)
        {
            return;
        }

        Vector3 point = new Vector3(Random.Range(-MoveSize.x, MoveSize.x),0, Random.Range(-MoveSize.y, MoveSize.y));
        moveControl.TargetPointList.Add(point);
    }

    public bool IsFloowPoint()
    {
        return FloowPoint;
    }
}
