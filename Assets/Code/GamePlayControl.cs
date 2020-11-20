using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayControl : MonoBehaviour
{
    public float CreateIntervalTime;
    public Vector2 CreateInitSize;
    public bool Playing;

    public MeshRenderer MainPlayer;
    public MeshRenderer Floor;
    public Transform Sun;

    private float createTime;
    private Color MainPlayerDefaultColor;
    private float BeAttack;
    private float LastProcess;

    public UIControl UIControl;

    // Start is called before the first frame update
    void Start()
    {
        createTime = 0;

        MainPlayerDefaultColor = MainPlayer.material.color;
        MainPlayerControl.PlayerBeAttackEvent += PlayerBeAttackEventListener;
    }

    // Update is called once per frame
    void Update()
    {
        if (Playing)
        {
            createTime += Time.deltaTime;
            if (createTime >= CreateIntervalTime)
            {
                createTime = 0;
                CreateMonster();
            }
            SceneArt();
        }

    }

    private void CreateMonster()
    {
        GameObject monster = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/Monster"));
        monster.transform.position = new Vector3(Random.Range(-CreateInitSize.x, CreateInitSize.x), 0, Random.Range(-CreateInitSize.y, CreateInitSize.y));
    }

    private void SceneArt()
    {
        float process = FinalControl.Instance.GetActiveEnergyCount() / FinalControl.Instance.GetEnergyCount();
        if(LastProcess < process)
        {
            BeAttack = 0;
            LastProcess = process;

            float random = Random.Range(0, 1f);
            if (random > 0.4f)
            {
                UIControl.SetText("封印黑暗.", false);
            }

            print("进度：" + process);
        }

        Sun.localScale = Vector3.one * (0.5f + process);

        Color floorColor = Floor.material.color;
        floorColor.r = process;
        floorColor.g = process;
        floorColor.b = process;
        Floor.material.color = floorColor;


        Color playerColor = MainPlayerDefaultColor;
        playerColor.r -= BeAttack - process;
        playerColor.g -= BeAttack - process;
        playerColor.b -= BeAttack - process;
        MainPlayer.material.color = playerColor;

        IsFinish(process);
    }

    private void IsFinish(float process)
    {
        float random = Random.Range(0, 1f);
        if (random > 0.7f)
        {
            UIControl.SetText("需要开启全部阵法！", false);
        }

        if (process >= 0.99f)
        {

            MainPlayerControl.PlayerBeAttackEvent -= PlayerBeAttackEventListener;
            Playing = false;
            UIControl.PlayEndStory();
        }
    }

    public void PlayerBeAttackEventListener()
    {
        BeAttack += 0.001f;

        float random = Random.Range(0,1f);
        if (random > 0.5f)
        {
            UIControl.SetText("小心!", false);
        }
    }
}
