using TMPro;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    private ScoreManager scoreManager;
    private GameManager gameManager;
    public TextMeshProUGUI livesUIDisplay;
    private int lives = 3;
    private bool isPlayerOutOfLives = false;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        livesUIDisplay.text = "Lives: " + lives;
    }

    // Update is called once per frame
    public void CheckIfPlayerLosesALife()
    {
        if (scoreManager.hasPlayerScored == false)
        {
            lives--;
            livesUIDisplay.text = "Lives: " + lives;
            if (lives <= 0)
            {
                isPlayerOutOfLives = true;
                gameManager.GameOver();
            }
                

        }
        //reset value for next shot
        scoreManager.hasPlayerScored = false;
    }

    public bool IsGameActive() => !isPlayerOutOfLives;

    private void GameOver()
    {
        isPlayerOutOfLives = true;
    }
}
