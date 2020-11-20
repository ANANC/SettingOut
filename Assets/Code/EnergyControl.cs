using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyControl : MonoBehaviour
{
    private bool IsLight;
    public MeshRenderer MeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        IsLight = false;
        FinalControl.Instance.AddEnergy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsLight)
        {
            return;
        }

        if(other.gameObject.tag == "Monster")
        {
            MonsterAI monsterAI = other.gameObject.GetComponent<MonsterAI>();
            if(monsterAI.IsFloowPoint())
            {
                GameObject.Destroy(monsterAI.gameObject);
                MeshRenderer.material.color = new Color(0.96f, 0.83f, 0.27f);
                IsLight = true;
                FinalControl.Instance.AddActiveEnergy();
            }
        }
    }
}
