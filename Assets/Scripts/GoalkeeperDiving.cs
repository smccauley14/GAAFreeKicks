using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperDiving : MonoBehaviour
{
    private Animator animator;
    private bool hasAnimationPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Test");
        Debug.Log(other.tag);
        if (other.CompareTag("Ball") && !hasAnimationPlayed)
        {
            // Trigger the animation based on the child's tag
            string childTag = gameObject.tag;

            switch (childTag)
            {
                case "BottomLeft":
                    Debug.Log("Dive bottom left");
                    animator.SetBool("BottomLeft", true);
                    break;

                case "TopLeft":
                    animator.SetBool("TopLeft", true);
                    break;

                case "BottomRight":
                    animator.SetBool("BottomRight", true);
                    break;

                case "TopRight":
                    animator.SetBool("TopRight", true);
                    break;
            }

            StartCoroutine(StopAnimationAfterOneLoop(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
        }
    }

    IEnumerator StopAnimationAfterOneLoop(float animationLength)
    {
        hasAnimationPlayed = true;

        // Wait for the animation to complete one loop
        yield return new WaitForSeconds(animationLength);

        // Stop the animation
        StopAnimation();
    }

    void StopAnimation()
    {
        // Reset animation triggers and flags
        animator.SetBool("BottomLeft", false);
        animator.SetBool("TopLeft", false);
        animator.SetBool("BottomRight", false);
        animator.SetBool("TopRight", false);

        hasAnimationPlayed = false;
    }
}