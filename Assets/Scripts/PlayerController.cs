using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform ball;
    private Vector3 offset = new Vector3(5f, -0.28f, 5f);
    public bool isMoving;
    private float moveSpeed = 5f;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    public void ResetPlayer()
    {
        if (ball != null)
        {
             if (ball.position.x < 0)
                offset.x = -3f;

            if (ball.position.x >= 0)
                offset.x = 3f;

            transform.position = ball.position + offset;
            animator.SetBool("Run", false);
        }
    }

    public void MoveTowardsBall()
    {
        if (ball != null)
        {
            Vector3 direction = ball.position - transform.position;
            direction.z += 1;

            // Use only the x and z components for movement
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);

            float distance = horizontalDirection.magnitude;

            if (distance <= 0.1f)
            {
                animator.SetBool("Run", false);
                isMoving = false;
            }
            else
            {
                animator.SetBool("Run", true);
                horizontalDirection.Normalize();
                transform.Translate(horizontalDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
