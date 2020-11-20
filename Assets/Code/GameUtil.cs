using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtil
{
    public static bool IsArrive(Vector3 position, Vector3 targetPosition, float range)
    {
        //if (Mathf.Abs(targetPosition.x - position.x) < range && Mathf.Abs(targetPosition.y - position.y) < range && Mathf.Abs(targetPosition.z - position.z) < range)
        if(Vector3.Distance(position, targetPosition) < range)
        {
            return true;
        }
        return false;
    }
}
