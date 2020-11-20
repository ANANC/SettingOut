using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalControl 
{
    private static FinalControl m_Instance;
    public static FinalControl Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new FinalControl();
            }
            return m_Instance;
        }
    }

    public delegate void AddDrawPoint(Vector3 point);
    public static AddDrawPoint TryAddAddDrawPointEvent;

    public delegate void AddSettingOutPoint(Vector3 point);
    public static  AddSettingOutPoint AddSettingOutPointEvent;


    public delegate void WalkPoint(Vector3 point);
    public static WalkPoint WalkPointEvent;

    public delegate void AddActivePoint(Vector3 point);
    public static AddActivePoint AddActivePointEvent;

    public delegate void DestroyPoint(Vector3 position);
    public static DestroyPoint DestroyPointEvent;


    private List<Vector3> AllPoints = new List<Vector3>();
    private const float Interval = 0.1f;
    public void AddDrawPosition(Vector3 position)
    {
        position.y = 0;

        TryAddAddDrawPointEvent?.Invoke(position);
    }

    public void AddPosition(Vector3 position)
    {
        if(AllPoints.Count == 0)
        {
            SetEnableDraw(false);
        }

        position.y = 0;

        if (AllPoints.Count > 0)
        {
            Vector3 lastPosition = AllPoints[AllPoints.Count - 1];
            float distance = Vector3.Distance(lastPosition, position);
            if (distance < Interval)
            {
                return;
            }
            if (distance> 1f)
            {
                AddPosition(Vector3.Lerp(lastPosition, position, Time.deltaTime));
            }
        }

        AllPoints.Add(position);
        AddSettingOutPointEvent?.Invoke(position);
    }

    private List<Vector3> ActivePoints = new List<Vector3>();
    public void AddActivePosition(Vector3 position)
    {
        position.y = 0;
        ActivePoints.Add(position);
        AddActivePointEvent?.Invoke(position);
    }

    public List<Vector3> GetActivePoints()
    {
        return ActivePoints;
    }

    private bool EnableDraw = true;
    public void SetEnableDraw(bool isEnableDraw)
    {
        EnableDraw = isEnableDraw;
    }

    public bool IsEnableDraw()
    {
        return EnableDraw;
    }


    public void DestroyPointListener(Vector3 position)
    {
        AllPoints.Remove(position);
        ActivePoints.Remove(position);
        DestroyPointEvent?.Invoke(position);
    }

    private float EnergyCount = 0;
    private float ActiveEnergyCount = 0;
    public void AddEnergy()
    {
        EnergyCount += 1;
    }

    public void AddActiveEnergy()
    {
        ActiveEnergyCount += 1;
    }

    public float GetEnergyCount()
    {
        return EnergyCount;
    }

    public float GetActiveEnergyCount()
    {
        return ActiveEnergyCount;
    }
}
