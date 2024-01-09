using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private Transform nets;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goalkeeper;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject powerSlider;
    [SerializeField] private GameObject windZone;
    [SerializeField] private GameObject difficultyManager;
    [SerializeField] private TextMeshProUGUI powerUIText;
    [SerializeField] private TextMeshProUGUI scoreUIText;
    [SerializeField] private TextMeshProUGUI livesUIText;
    [SerializeField] private TextMeshProUGUI distanceUIText;
    [SerializeField] private TextMeshProUGUI highScoreUIText;
    [SerializeField] private TextMeshProUGUI instructionsUIText;
    [SerializeField] private Button restartButton;
    [SerializeField] private AudioSource introMusic;
    [SerializeField] private AudioSource crowdMusic;


    public TextMeshProUGUI distanceUIDisplay;
    public TextMeshProUGUI gameOverUIDisplay;

    public void StartGame()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Set initial UI states
        gameOverUIDisplay.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        titleScreen.SetActive(false);
        highScoreUIText.gameObject.SetActive(false);
        instructionsUIText.gameObject.SetActive(false);

        // Set initial game object states
        SetObjectsActiveState(true);
    }

    private void SetObjectsActiveState(bool isActive)
    {
        ball.gameObject.SetActive(isActive);
        nets.gameObject.SetActive(isActive);
        player.SetActive(isActive);
        goalkeeper.SetActive(isActive);
        scoreUIText.gameObject.SetActive(isActive);
        livesUIText.gameObject.SetActive(isActive);
        distanceUIText.gameObject.SetActive(isActive);
        powerSlider.SetActive(isActive);
        windZone.SetActive(isActive);
        powerUIText.gameObject.SetActive(isActive);
        difficultyManager.gameObject.SetActive(isActive);
        if (introMusic.isPlaying)
        {
            introMusic.Stop();
        }
        if (!crowdMusic.isPlaying)
        {
            crowdMusic.Play();
        }
    }

    public void SetDistanceFromGoals()
    {
        float distance = Vector3.Distance(ball.position, nets.position);
        string distanceString = distance.ToString("F2");
        distanceUIDisplay.text = "Distance: " + distanceString + " m";
    }

    public void GameOver()
    {
        SetObjectsActiveState(false);
        gameOverUIDisplay.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowHighScores()
    {
        SetObjectsActiveState(false);
        var isHighScoreDisplayed = highScoreUIText.gameObject.activeSelf;
        highScoreUIText.gameObject.SetActive(!isHighScoreDisplayed);
        instructionsUIText.gameObject.SetActive(false);
    }

    public void ShowInstructions()
    {
        SetObjectsActiveState(false);
        var isInstructionsDisplayed = instructionsUIText.gameObject.activeSelf;
        instructionsUIText.gameObject.SetActive(!isInstructionsDisplayed);
        highScoreUIText.gameObject.SetActive(false);
    }
}
