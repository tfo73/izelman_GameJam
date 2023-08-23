using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(6f, 3f, -10f);
    public Transform target;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + offset.y, offset.z);
        // transform.position = Vector3.Slerp(transform.position,newPos,FollowSpeed*Time.deltaTime);
        transform.position = newPos;

    }
}
