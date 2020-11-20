using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Image Mask;
    public float MaskTime = 0.5f;
    private float CurMaskTime = 0;
    private Color MaskColor;
    private Color MaskBgColor;

    private bool IsHideMask = false;
    private bool IsShowMask = false;
    private int IsPlayMask = -1;



    public Text WhiteText;
    public Text BlackText;
    public float ShowTextTime = 1;
    private float CurShowTextTime = 0;


    public Transform Loves;
    public float LoveRotationTime = 1;
    private float CurLoveRotationTime = 0;
    public Transform StartSun;
    public Transform EndSun;

    public float ShowStartRentTime = 1;
    private float CurShowStartRentTime = 0;
    public Image StartRent;

    public GameObject StartStroyUI;
    public GameObject EndStroyUI;
    public GamePlayControl GamePlayControl;

    public Image EndRent;
    public Image Power;
    public float ShowPowerTime = 1;
    private float CurShowPowerTime = 0;

    public AudioSource Normal;
    public AudioSource Battle;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartStroy());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayMask != -1)
        {
            PlayMask();
        }

        if (IsHideMask)
        {
            HideMask();
        }
        if (IsShowMask)
        {
            ShowMask();
        }

        ShowText();

        Rotations();
    }

    public void PlayMask()
    {
        if (IsPlayMask == 1)
        {
            CurMaskTime = 0;
            IsShowMask = true;
            IsHideMask = false;
            IsPlayMask = 2;
        }
        else if (IsPlayMask == 2)
        {
            if (IsShowMask == false)
            {
                IsHideMask = true;
                IsPlayMask = 3;
            }
        }
        else if (IsPlayMask == 3)
        {
            if (IsHideMask == false)
            {
                IsPlayMask = -1;
            }
        }

    }

    public void HideMask()
    {
        MaskColor = new Color(MaskBgColor.r, MaskBgColor.g, MaskBgColor.b);

        CurMaskTime += Time.deltaTime;
        if (CurMaskTime >= MaskTime)
        {
            IsHideMask = false;
            MaskColor.a = 0;
            CurMaskTime = 0;
        }
        else
        {
            MaskColor.a = 1 - CurMaskTime;
        }

        Mask.color = MaskColor;
    }

    public void ShowMask()
    {
        MaskColor = new Color(MaskBgColor.r, MaskBgColor.g, MaskBgColor.b);

        CurMaskTime += Time.deltaTime;
        if (CurMaskTime >= MaskTime)
        {
            IsShowMask = false;
            MaskColor.a = 1;
            CurMaskTime = 0;
        }
        else
        {
            MaskColor.a = CurMaskTime;
        }

        Mask.color = MaskColor;
    }

    public void ShowText()
    {
        if(CurShowTextTime <= 0 )
        {
            return;
        }
        CurShowTextTime -= Time.deltaTime;
        if(CurShowTextTime <=0)
        {
            WhiteText.text = "";
            BlackText.text = "";
        }
    }

    public void SetText(string text,bool IsWhite)
    {
        if (IsWhite)
        {
            WhiteText.text = text;
        }
        else
        {
            BlackText.text = text;
        }
        CurShowTextTime = ShowTextTime;
    }


    private IEnumerator StartStroy()
    {
        Normal.Play();

        MaskBgColor = new Color(0, 0, 0);

        IsHideMask = true;

        while (IsHideMask == true)
        {
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        while (Loves.localPosition.y > 50)
        {
            Loves.localPosition += Vector3.down;
            yield return new WaitForSeconds(0);
        }
        SetText("好美..", false);

        yield return new WaitForSeconds(2);


        SetText("!!!", false);
        Color startRentolor;
        while (CurShowStartRentTime < ShowStartRentTime)
        {
            CurShowStartRentTime += Time.deltaTime;
            startRentolor = StartRent.color;
            startRentolor.a = Random.Range(0.2f, 1);
            StartRent.color = startRentolor;
            yield return new WaitForSeconds(0);
        }
        startRentolor = StartRent.color;
        startRentolor.a = 1;
        StartRent.color = startRentolor;

        while (StartSun.localPosition.y > 50)
        {
            StartSun.localPosition += Vector3.down;
            yield return new WaitForSeconds(0);
        }

        SetText("别怕.", false);
        yield return new WaitForSeconds(1);


        Normal.Stop();

        IsShowMask = true;
        while (IsShowMask == true)
        {
            yield return new WaitForSeconds(1);
        }

        Battle.Play();
        StartStroyUI.SetActive(false);

        IsHideMask = true;
        while (IsHideMask == true)
        {
            yield return new WaitForSeconds(1);
        }
       

        print("开始游戏");
        GamePlayControl.Playing = true;

        SetText("快！吸引怪物去阵法！", false);
        yield return new WaitForSeconds(1);
        SetText("（长按左键拖动行走）", false);
    }

    public void PlayEndStory()
    {
        StartCoroutine(EndStroy());
    }

    public IEnumerator EndStroy()
    {

        Battle.Stop();
        Normal.Play();

        SetText("没事了.", true);

        MaskBgColor = new Color(1, 1, 1);

        IsShowMask = true;
        while(IsShowMask == true)
        {
            yield return new WaitForSeconds(1);
        }

        EndStroyUI.SetActive(true);

        IsHideMask = true;
        while(IsHideMask == true)
        {
            yield return new WaitForSeconds(1);
        }

        SetText("好温暖..", false);

        yield return new WaitForSeconds(2);

        SetText("我与你同在.", false);

        while(CurShowPowerTime< ShowPowerTime)
        {
            CurShowPowerTime += Time.deltaTime;
            Color powerColor = Power.color;
            powerColor.a = Random.Range(0.4f, 1f);
            Power.color = powerColor;
            yield return new WaitForSeconds(0);
        }

        yield return new WaitForSeconds(2);

        Color endRentolor;
        CurShowStartRentTime = 0; 
        while (CurShowStartRentTime < ShowStartRentTime)
        {
            CurShowStartRentTime += Time.deltaTime;
            endRentolor = EndRent.color;
            endRentolor.a = Random.Range(0f, 0.4f);
            EndRent.color = endRentolor;
            yield return new WaitForSeconds(0);
        }

        IsShowMask = true;

        while (IsShowMask == true)
        {
            yield return new WaitForSeconds(1);
        }

        SetText("结束..", false);
    }

    private void Rotations()
    {
        CurLoveRotationTime += Time.deltaTime;
        if (CurLoveRotationTime >= LoveRotationTime)
        {
            CurLoveRotationTime = 0;

            for (int index = 0; index < Loves.childCount; index++)
            {
                Transform love = Loves.GetChild(index);

                Quaternion loveQua = love.rotation;
                Vector3 loveAngles = loveQua.eulerAngles;
                loveAngles.z = Random.Range(-2, 2);
                loveQua.eulerAngles = loveAngles;
                love.rotation = loveQua;
            }
        }

        Quaternion startSunQua = StartSun.rotation;
        Vector3 startSunEulerAngles = startSunQua.eulerAngles;
        startSunEulerAngles.z -= 2;
        startSunQua.eulerAngles = startSunEulerAngles;
        StartSun.rotation = startSunQua;

        Quaternion endSunQua = EndSun.rotation;
        Vector3 endSunEulerAngles = endSunQua.eulerAngles;
        endSunEulerAngles.z -= 4;
        endSunQua.eulerAngles = endSunEulerAngles;
        EndSun.rotation = endSunQua;
    }









}
