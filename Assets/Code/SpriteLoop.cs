using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoop : MonoBehaviour
{

    [Header("运行立即播放")]
    public bool m_StartPlayer;

    [Header("速度")]
    public float m_Speed = 1;

    [Header("资源")]
    public Texture[] m_Sprites;

    [Header("循环")]
    public bool m_Loop;

    [Header("播放完毕隐藏")]
    public bool m_LoopFinishHide;

    private Material m_Material;
    private int m_Index;
    private float m_Time;
    private bool m_Play;
    private Vector3 m_LocalScale;

    private Action m_FinishCallback;
    private void Awake()
    {
        m_Play = m_StartPlayer && m_Sprites.Length != 0;
    }

    private void Start()
    {
        m_Time = 0;
        m_Index = 0;
        if (m_Speed <= 0)
        {
            m_Speed = 1;
        }
        TryGetMaterial();
    }

    public void SetLoopFinishCallback(Action finishCallback)
    {
        m_FinishCallback = finishCallback;
    }

    public void ClearFinishCallback()
    {
        m_FinishCallback = null;
    }

    void Update()
    {
        if (!m_Play)
        {
            return;
        }

        m_Time += Time.deltaTime;
        if (m_Time >= m_Speed)
        {
            m_Time = 0;
            m_Index = m_Index + 1 >= m_Sprites.Length ? 0 : m_Index + 1;
            if (!m_Loop && m_Index == 0)
            {
                Stop();
                return;
            }
            UpdateSprite();
        }
    }

    public void Play()
    {
        m_Play = true;
        m_Time = 0;
        m_Index = 0;
        TryGetMaterial();
        this.transform.localScale = m_LocalScale;
        UpdateSprite();
    }

    public void Stop()
    {
        m_Play = false;
        TryLoopFinishHide();
        TryCallBack();
    }

    private void UpdateSprite()
    {
        m_Material.mainTexture = m_Sprites[m_Index];
    }

    private void TryGetMaterial()
    {
        if (m_Material == null)
        {
            m_Material = this.GetComponent<MeshRenderer>().material;
            m_LocalScale = this.transform.localScale;
            if (!m_StartPlayer)
            {
                this.transform.localScale = Vector3.zero;
            }
        }
    }


    private void TryLoopFinishHide()
    {
        if (m_LoopFinishHide)
        {
            this.transform.localScale = Vector3.zero;
        }
    }

    private void TryCallBack()
    {

        if (m_FinishCallback != null)
        {
            m_FinishCallback();
        }
    }

}
