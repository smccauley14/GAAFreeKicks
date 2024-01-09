using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int goalsScored = 0;
    private int pointsScored = 0;
    public bool hasPlayerScored;
    public float MaxShotDistanceIncrement = 10f;
    public TextMeshProUGUI scoreUIDisplay;

    private void Start()
    {
        UpdateScoreDisplay();
    }
    public void AddPoint()
    {
        pointsScored++;
        UpdateScoreDisplay();
    }

    public void AddGoal()
    {
        goalsScored++;
        UpdateScoreDisplay();
    }

    public int GetPointsScored() => (goalsScored * 3) + pointsScored;

    public int GetGoalsScored() => goalsScored;

    private void UpdateScoreDisplay()
    {
        scoreUIDisplay.text = "Score: " + goalsScored + "-" + pointsScored;
    }
}
