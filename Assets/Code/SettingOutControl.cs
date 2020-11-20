using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingOutControl : MonoBehaviour
{
    private int Layermask;
    private List<Vector3> RaycastPointList = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        Layermask = 1 << LayerMask.NameToLayer("Floor");
    }


    // Update is called once per frame
    void Update()
    {
        if (!FinalControl.Instance.IsEnableDraw())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastPointList.Clear();
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Layermask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);

                Vector3 position = hit.point;
                // print("position:" + position);

                RaycastPointList.Add(position);
                FinalControl.Instance.AddDrawPosition(position);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            for (int index = 0; index < RaycastPointList.Count; index++)
            {
                FinalControl.Instance.AddPosition(RaycastPointList[index]);
            }
            RaycastPointList.Clear();
        }
    }
}
