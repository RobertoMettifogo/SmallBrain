using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class Rock : MonoBehaviour
{
    public float speed = 2.0f;
    private Vector3 startPosition;
    private float startTime;
    private float journeyLength;
    private Vector3 endpos;
    public GameController gameController;
    public SpawnController spawnController;
    private UIController uiController;
    private Animator animator;

    private bool HasReachedEnd = false;

    private SpawnController.MathOperation assignedOperation;
    private int operandA;
    private int operandB;
    private int correctAnswer;

    private Operations operations;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        spawnController = FindObjectOfType<SpawnController>();
        startPosition = transform.position;
        Vector3 end = startPosition + Vector3.down * 19f;
        endpos = end;
        journeyLength = Vector3.Distance(startPosition, end);
        startTime = Time.time;
        uiController = UIController.Instance;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckKill();
    }

    public void Kill()
    {
        gameController.LoseLives();
        Destroy();
    }

    [ProButton]
    public void Destroy()
    {
        animator.SetBool("Explosion", true);
        Destroy(gameObject, 0.5f);
    }
    public void Wipe()
    {
        Destroy(gameObject);
    }

    public void CheckKill()
    {
        if (HasReachedEnd)
            return;
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(startPosition, endpos, fracJourney);

        if (fracJourney >= 1.0f)
        {
            HasReachedEnd = true;
            Kill();
        }
    }

    // Method to set the correct answer for the rock
    public void SetCorrectAnswer(int answer)
    {
        correctAnswer = answer;
    }

    // Method to check the answer when the rock reaches the end
    public void CheckAnswer(int playerAnswer)
    {
        if (playerAnswer == correctAnswer)
        {
            if (spawnController.spawnedObjectCount >= 20 && spawnController.spawnedObjectCount <= 30)
            {
                spawnController.DestroyObjectsInRadius(transform.position, 10);
                gameController.IncrementScore();
                Destroy();
                spawnController.ResetSpawnedObjectCount();
            }
            else
            {
                Destroy();
                gameController.IncrementScore();
            }
        }
        else
        {
            Debug.Log("Incorrect Answer");
        }
    }
    private void OnDestroy()
    {
        if (uiController != null)
        {
            uiController.RemoveOperationsFromObject(gameObject);
        }
    }
}
