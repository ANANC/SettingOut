using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerControl : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    // Start is called before the first frame update

    public delegate void PlayerBeAttack();
    public static PlayerBeAttack PlayerBeAttackEvent;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            PlayerBeAttackEvent?.Invoke();
            //Color color = MeshRenderer.material.color;
            //color.r -= 0.01f;
            //color.g -= 0.01f;
            //color.b -= 0.01f;
            //MeshRenderer.material.color = color;
        }
    }
}
