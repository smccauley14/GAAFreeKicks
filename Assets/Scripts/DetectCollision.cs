using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private ScoreManager scoreManager;
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Point") && other.CompareTag("Ball"))
        {
            scoreManager.hasPlayerScored = true;
            scoreManager.AddPoint();
        }

        if (gameObject.CompareTag("Goal") && other.CompareTag("Ball"))
        {
            scoreManager.hasPlayerScored = true;
            scoreManager.AddGoal();
        }
    }
}
