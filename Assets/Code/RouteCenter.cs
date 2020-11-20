using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteCenter : MonoBehaviour
{
    public float ActiveTime = 5;
    public float Interval = 3;

    public class Point
    {
        public Vector3 Position;
        public GameObject PointGameObject;
        public MeshRenderer MeshRenderer;
        public bool HasArt;
        public bool IsActive;
        public float ActiveTime;
    }

    private List<Point> m_AllPointList = new List<Point>();
    private List<Point> m_DestroyPointList = new List<Point>();
    private List<Point> m_AddPointList = new List<Point>();

    private Point LastArtPoint;

    // Start is called before the first frame update
    void Start()
    {
        FinalControl.TryAddAddDrawPointEvent += AddAddDrawPointListener;
        FinalControl.WalkPointEvent += WalkPointEventListener;
    }

    // Update is called once per frame
    void Update()
    {
        for(int index = 0;index< m_AddPointList.Count;index++)
        {
            m_AllPointList.Add(m_AddPointList[index]);
        }
        m_AddPointList.Clear();

        for (int index = 0; index < m_DestroyPointList.Count; index++)
        {
            m_AllPointList.Remove(m_DestroyPointList[index]);
        }
        m_DestroyPointList.Clear();

        for ( int index = 0;index<m_AllPointList.Count;index++)
        {
            Point point = m_AllPointList[index];
            if(point.IsActive)
            {
                if(point.ActiveTime >= ActiveTime)
                {
                    if (point.HasArt)
                    {
                        GameObject.DestroyImmediate(point.PointGameObject);
                    }

                    m_DestroyPointList.Add(point);
                    FinalControl.Instance.DestroyPointListener(point.Position);
                    continue;
                }

                point.ActiveTime += Time.deltaTime;

                if (point.HasArt)
                {
                    //Color color = point.MeshRenderer.material.color;
                    //color.r += 0.001f;
                    //color.g += 0.001f;
                    //color.b += 0.001f;
                    //point.MeshRenderer.material.color = color;

                    point.PointGameObject.transform.localScale *= 0.999f;
                }
            }
        }

        if (LastArtPoint != null && LastArtPoint.IsActive)
        {
            FinalControl.Instance.SetEnableDraw(true);
        }
    }

    public void AddAddDrawPointListener(Vector3 point)
    {
        if (m_AllPointList.Count == 0 && m_AddPointList.Count == 0)
        {
            LastArtPoint = AddPointData(point,true);
        }
        else
        {
            Point lastPointData = LastArtPoint;

            if (Vector3.Distance(point, lastPointData.Position) >= Interval)
            {
                LastArtPoint = AddPointData(point, true);
            }
            else
            {
                AddPointData(point, false);
            }
        }
    }

    private Point AddPointData(Vector3 position,bool hasArt)
    {
        Point pointdata = new Point();
        pointdata.Position = position;
        pointdata.HasArt = hasArt;
        if (hasArt)
        {
            pointdata.PointGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/RoutePoint"));
            pointdata.PointGameObject.transform.position = position;
            pointdata.MeshRenderer = pointdata.PointGameObject.GetComponent<MeshRenderer>();
        }
        pointdata.IsActive = false;
        pointdata.ActiveTime = 0;

        m_AddPointList.Add(pointdata);

        return pointdata;
    }

    public void WalkPointEventListener(Vector3 position)
    {
        int Index = -1;
        for (int index = m_AllPointList.Count - 1; index >= 0; index--)
        {
            if (!m_AllPointList[index].HasArt)
            {
                continue;
            }

            if(GameUtil.IsArrive(position, m_AllPointList[index].Position,0.1f))
            {
                Index = index;
                break;
            }
        }

        if(Index == -1)
        {
            return;
        }

        Point pointData = m_AllPointList[Index];
        pointData.IsActive = true;

        if (pointData.HasArt)
        {
            pointData.MeshRenderer.material.color = new Color(0.98f, 0.83f, 0.28f);
            FinalControl.Instance.AddActivePosition(pointData.Position);
        }
    }
}
