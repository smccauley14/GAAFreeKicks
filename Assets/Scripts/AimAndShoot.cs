using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AimAndShoot : MonoBehaviour
{
    public Transform ball;
    public Slider powerSlider;
    public PlayerController player;
    public Image windArrow;
    public TextMeshProUGUI windText;
    private GameManager gameManager;
    private LivesManager livesManager;
    private BallController ballController;

    private float maxDistanceX = 50f;
    private float maxDistanceY = 10f;
    private float maxDistanceZ = 70f;

    private LineRenderer lineRenderer;
    private Rigidbody ballRigidbody;

    private Vector3 initialPosition;
    private Vector3 endPosition;
    private float lastDistanceX = 0f;
    private float lastDistanceY = 0f;
    private float shotPower = 0f;
    private float yRange = 0;
    public bool isPowerAdjustable = true;
    public bool hasShot = false;

    private WindForce windForceScript;

    private void Start()
    {
        InitializeComponents();
        FindGameObjects();
        windForceScript = GameObject.Find("WindZone").GetComponent<WindForce>();
        UpdateWindDirection();
    }

    private void Update()
    {
        if (livesManager.IsGameActive())
        {
            if (isPowerAdjustable)
            {
                HandleInput();
                UpdateAim();
                gameManager.SetDistanceFromGoals();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !hasShot)
            {
                isPowerAdjustable = false;
                shotPower = powerSlider.value;
                player.isMoving = true;
                StartCoroutine(MovePlayerTowardsBall());
            }

            if(hasShot)
            {
                ballRigidbody.AddForce(windForceScript.GetCurrentWindDirection() * Time.deltaTime);
            }
        }
    }

    private IEnumerator MovePlayerTowardsBall()
    {
        while (player.isMoving)
        {
            player.MoveTowardsBall();
            yield return null;
        }

        //ApplyWindForceAfterShot();
        player.animator.SetBool("Kick", true);
        Shoot();
    }

    private void ApplyWindForceAfterShot()
    {
        StartCoroutine(ApplyWindForceWithDelay(0.1f));
    }

    private IEnumerator ApplyWindForceWithDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

        float windForceMultiplier = windForceScript.GetCurrentWindForce();
        ballRigidbody.AddForce(windForceScript.GetCurrentWindDirection() * windForceMultiplier, ForceMode.Impulse);
    }

    private void InitializeComponents()
    {
        initialPosition = new Vector3(0, 1f, 0);
        lastDistanceX = 0f;
        lastDistanceY = 0;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(0, 1, 0));
        powerSlider.onValueChanged.AddListener(HandlePowerAdjustment);
        ballRigidbody = ball.GetComponent<Rigidbody>();
    }

    private void FindGameObjects()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        livesManager = GameObject.Find("Lives").GetComponent<LivesManager>();
        ballController = GameObject.Find("Ball").GetComponent<BallController>();
    }

    private void HandleInput()
    {
        float horizontalInput = -Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        lastDistanceX = Mathf.Clamp(lastDistanceX + horizontalInput, -maxDistanceX, maxDistanceX);
        lastDistanceY = Mathf.Clamp(lastDistanceY + verticalInput, -maxDistanceY, maxDistanceY);

        if (lastDistanceY < yRange)
            lastDistanceY = 0;

        powerSlider.value = Mathf.PingPong(Time.time, 1);
    }

    private void UpdateAim()
    {
        endPosition = new Vector3(initialPosition.x + lastDistanceX, initialPosition.y + lastDistanceY, initialPosition.z);
        lineRenderer.SetPosition(0, endPosition);
        //gameObject.SetActive(true);
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    private void Shoot()
    {
        //gameObject.SetActive(false);
        gameObject.GetComponent<Renderer>().enabled = false;
        Vector3 worldStartPosition = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(0));
        Vector3 worldEndPosition = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(1));

        float initialForceMagnitude = shotPower * 100;
        float distance = Vector3.Distance(worldStartPosition, worldEndPosition);

        // Adjust the multiplier based on how quickly you want the force to decrease over distance
        float forceMultiplier = Mathf.Clamp01(10 - distance / maxDistanceZ);

        // Calculate the final force magnitude
        float forceMagnitude = initialForceMagnitude * forceMultiplier;

        Vector3 forceDirection = (worldStartPosition - worldEndPosition).normalized;

        ballRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        hasShot = true;
        ballController.rollingStartTime = Time.time;
        player.animator.SetBool("Kick", false);
    }

    public void UpdateWindDirection()
    {
        windForceScript.UpdateWindDirection();
    }

    private void HandlePowerAdjustment(float value)
    {
        if (!isPowerAdjustable)
        {
            powerSlider.value = shotPower;
        }
    }

    public void ResetAim()
    {
        lastDistanceX = 0;
        lastDistanceY = 0;
        UpdateAim();
        hasShot = false;
    }
}
