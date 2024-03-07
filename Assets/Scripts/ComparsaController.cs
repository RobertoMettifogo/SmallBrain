using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class ComparsaController : MonoBehaviour
{
    private Animator animator;
    private Vector3 startPosition;
    public Transform endPosition;
    private AudioSource comparsaMeow;
    public AudioSource comparsaWalk;
    private float journeyLength;
    private float speed = 3f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        comparsaMeow = GetComponent<AudioSource>();
        animator.SetBool("Run", false);
        startPosition = transform.position;
        journeyLength = Vector3.Distance(startPosition, endPosition.position);
    }
    [ProButton]
    public void MoveTo()
    {
        StartCoroutine(MoveToCoroutine());
    }

    IEnumerator MoveToCoroutine()
    {
        float startTime = Time.time;
        float distanceCovered = 0f;
        animator.SetBool("Run", true);
        comparsaWalk.Play();

        while (distanceCovered < journeyLength)
        {
            float journeyProgress = (Time.time - startTime) * speed;
            float journeyFraction = journeyProgress / journeyLength;
            transform.position = Vector3.Lerp(startPosition, endPosition.position, journeyFraction);

            distanceCovered = Vector3.Distance(startPosition, transform.position);
            yield return null;
        }

        transform.position = endPosition.position;

        animator.SetBool("Run", false);
        comparsaWalk.Stop();
        animator.SetBool("Miao", true);
        comparsaMeow.Play();
        comparsaMeow.mute = false;

        StartCoroutine(Miao());
    }

    IEnumerator Miao()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Miao", false);
        comparsaMeow.mute = false;
        animator.SetBool("Sit", true);
    }

    public void Reset()
    {
        StopAllCoroutines(); // Stop any running coroutine
        transform.position = startPosition; // Set position to initial position
        animator.SetBool("Run", false); // Reset animator parameters
        animator.SetBool("Miao", false);
        animator.SetBool("Sit", false);
    }
}
