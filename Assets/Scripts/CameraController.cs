using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform ball;
    public Transform nets;
    private Vector3 offset = new Vector3(3f, 4.5f, 12f);


    public void ResetCamera()
    {
        if (ball != null)
        {
            if (ball.position.x < 0)
                offset.x = -3f;

            if (ball.position.x >= 0)
                offset.x = 3f;

            transform.position = ball.position + offset;
            transform.LookAt(nets.position);
        }
    }
}
