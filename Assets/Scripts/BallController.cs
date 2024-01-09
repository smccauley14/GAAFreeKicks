using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float maxPower;
    public Transform nets;
    public CameraController cameraFollow;
    public LivesManager livesManager;
    public PlayerController player;
    public float windForce = 10f;
    public float maxShotDistance = 21f;
    public float minShotDistance = 13f;
    private Rigidbody ball;
    private AimAndShoot aimAndShoot;
    private WindForce windController;
    [SerializeField]
    private DifficultyManager difficultyManager;

    public float rollingStartTime;
    private const float rollingCheckDuration = 10f;
    private const float rollingVelocityThreshold = 5f;

    private void Start()
    {
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        aimAndShoot = GameObject.Find("Line").GetComponent<AimAndShoot>();
        cameraFollow = Camera.main.GetComponent<CameraController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        livesManager = GameObject.Find("Lives").GetComponent<LivesManager>();
        windController = GameObject.Find("WindZone").GetComponent<WindForce>();
    }

    void FixedUpdate()
    {
        if (aimAndShoot.hasShot && Mathf.Abs(ball.velocity.z) < rollingVelocityThreshold)
        {
            // If the ball's z-axis velocity is below the threshold, start or update the rolling timer
            if (rollingStartTime == 0f)
            {
                rollingStartTime = Time.time;
            }
            else if (Time.time - rollingStartTime > rollingCheckDuration)
            {
                HandleWallCollision();
            }

            // TODO: Unable to get this working, to be fixed in future release
            // windController.ApplyWindForce();
        }
        else
        {
            rollingStartTime = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BallCatcher") || collision.gameObject.CompareTag("Sideline") || collision.gameObject.CompareTag("Goal"))
        {
            ResetBallState();
            HandleWallCollision();            
        }
    }

    private void HandleSlowShot()
    {
        livesManager.CheckIfPlayerLosesALife();
        ResetBallState();
        SetBallToRandomPosition();
    }
    private void HandleWallCollision()
    {
        livesManager.CheckIfPlayerLosesALife();
        Invoke(nameof(DelayedBallPositionReset), 5);
    }

    private void DelayedBallPositionReset()
    {
        ResetBallState();
        SetBallToRandomPosition();
    }

    private void SetBallToRandomPosition()
    {
        aimAndShoot.ResetAim();
        aimAndShoot.UpdateWindDirection();
        
        Vector3 randomPosition = GenerateRandomPosition();
        difficultyManager.UpdateMaxShotDistance();
        while (Vector3.Distance(randomPosition, nets.position) > maxShotDistance || Vector3.Distance(randomPosition, nets.position) < minShotDistance)
        {
            randomPosition = GenerateRandomPosition();
        }        
        transform.position = randomPosition;
        ball.MovePosition(randomPosition);

        aimAndShoot.isPowerAdjustable = true;
        ResetCameraAndPlayer();
    }

    private Vector3 GenerateRandomPosition()
    {
        float randomX = Random.Range(-25, 25);
        float randomY = Random.Range(-0.1f, -0.1f);
        float randomZ = Random.Range(-63, 60);

        return new Vector3(randomX, randomY, randomZ);
    }

    private void ResetBallState()
    {
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void ResetCameraAndPlayer()
    {
        if (cameraFollow != null)
        {
            cameraFollow.ResetCamera();
        }

        if (player != null)
        {
            player.ResetPlayer();
        }
    }
}
