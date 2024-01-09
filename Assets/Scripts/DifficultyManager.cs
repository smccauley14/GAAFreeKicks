using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Reference to the ScoreManager
    private ScoreManager scoreManager;
    private BallController ballController;

    private void Start()
    {
        ballController = GameObject.Find("Ball").GetComponent<BallController>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    // Increment for maxShotDistance
    public float shotIncrement = 10f;


    public void UpdateMaxShotDistance()
    {
        if (scoreManager.GetPointsScored() % 5 == 0 && ballController.maxShotDistance < 60f)
        {
            ballController.maxShotDistance += shotIncrement;
            ballController.minShotDistance += shotIncrement;
        }
    }
}