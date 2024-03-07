using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float lives = 5;
    public float score = 0;
    public Text livesText;
    public Text scoreText;
    public Text lastScore;
    public AudioSource Purr;
    public SpawnController spawnController;
    public InputField inputField;
    public Rock rock;
    public GameObject StartMenu;
    public GameObject answer;
    private Animator animator;
    public ComparsaController comparsa;
    private bool hasReached1000 = false;
    private bool hasReached5000 = false;
    private bool isPaused = true;
    Skybox sky;
    public string externalLinkURL = "https://discord.gg/z6gMV253";

    public enum Animations
    {
        Stretch,
        Lick
    }

    private void Start()
    {
        hasReached1000 = false;
        hasReached5000 = false;
        PauseGame();
        UpdateLives();
        UpdateScore();
        UpdateLastScore();
        //The player can submit a number (int) and will get into the method
        inputField.onEndEdit.AddListener(SubmitAnswer);
        animator = GetComponent<Animator>();
        comparsa = FindObjectOfType<ComparsaController>();
        StartCoroutine(PlayRandomAnimation());
    }

    IEnumerator PlayRandomAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 20f)); // Wait for random time

            Animations randomAnimation = (Animations)Random.Range(0, 2);

            PlayAnimation(randomAnimation);
        }
    }
    void PlayAnimation(Animations animation)
    {
        animator.SetBool("Stretch", false);
        animator.SetBool("Lick", false);

        switch (animation)
        {
            case Animations.Stretch:
                animator.SetBool("Stretch", true);
                Purr.Play();
                break;
            case Animations.Lick:
                animator.SetBool("Lick", true);
                break;
        }

        StartCoroutine(StopAnimationAfterDelay(animation));
    }

    IEnumerator StopAnimationAfterDelay(Animations animation)
    {
        yield return new WaitForSeconds(2f);

        switch (animation)
        {
            case Animations.Stretch:
                animator.SetBool("Stretch", false);
                Purr.Stop();
                break;
            case Animations.Lick:
                animator.SetBool("Lick", false);
                break;
        }
    }

    // Function to open the external link
    public void OpenExternalLink()
    {
        // Open the URL
        Application.OpenURL(externalLinkURL);
    }
    //Simple button interaction
    public void OnButtonClick()
    {
        UnpauseGame();
    }

    //Simple button interaction
    public void OnButtonClickQuit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (score >= 1000 && !hasReached1000 && !hasReached5000)
        {
            comparsa.MoveTo();
            spawnController.DifficultStep1();
            hasReached1000 = true;
        }
        else if (score >= 5000 && hasReached1000 && !hasReached5000)
        {
            spawnController.DifficultStep2();
            Camera mainCamera = Camera.main;
            sky = mainCamera.GetComponent<Skybox>();
            sky.enabled = true;
            hasReached5000 = true;
        }

        if (lives == 0)
        {
            PauseGame();
        }

        int count = spawnController.spawnedObjectCount;
    }
    [ProButton]
    public void diffUP1()
    {
        spawnController.DifficultStep1();
    }
    [ProButton]
    public void diffUP2()
    {
        spawnController.DifficultStep2();
    }

    // update lives text
    private void UpdateLives()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    // update score text
    private void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // update last score text
    private void UpdateLastScore()
    {
        lastScore.text = "LastScore: " + score.ToString();
    }

    // -1 life
    [ProButton]
    public void LoseLives() 
    {
        lives--;
        UpdateLives();
    }
    [ProButton]
    public void IncreaseLives()
    {
        lives += 50;
        UpdateLives();
    }

    //+100 score
    [ProButton]
    public void IncrementScore()
    {
        score += 100;
        UpdateScore();
    }

    // check if player submit answer
    void SubmitAnswer(string answer)
    {
        // Check if the answer string is null or empty
        if (string.IsNullOrEmpty(answer))
        {
            return;
        }

        // Attempt to parse the answer string into an integer
        if (!int.TryParse(answer, out int playerAnswer))
        {
            return;
        }

        List<GameObject> spawnedObjects = FindObjectOfType<SpawnController>().GetSpawnedObjects();

        foreach (GameObject spawnedObject in spawnedObjects)
        {
            // Check if the spawned object is null (has been destroyed)
            if (spawnedObject != null)
            {
                // Get the Rock component attached to the spawned object
                Rock rock = spawnedObject.GetComponent<Rock>();
                if (rock != null)
                {
                    // Check the player's answer against the correct answer for this rock
                    rock.CheckAnswer(playerAnswer);
                }
            }
            else
            {
                // Handle the case where the spawned object is null (optional)
                Debug.LogWarning("Spawned object is null.");
            }
        }

        inputField.text = "";

        inputField.Select();
        inputField.ActivateInputField();
    }

    //Set game to pause status
    void PauseGame()
    {
        comparsa.Reset();
        UpdateLastScore();
        Time.timeScale = 0f;
        isPaused = true;
        StartMenu.SetActive(true);
        answer.SetActive(false);
        DestroyAllRocks();
    }

    //Set the game to unpause status
    void UnpauseGame()
    {
        Time.timeScale = 1f;
        lives = 5;      
        score = 0;
        UpdateScore();
        UpdateLives();
        isPaused = false;
        answer.SetActive(true);
        StartMenu.SetActive(false);
        inputField.Select();
        inputField.ActivateInputField();

        Debug.ClearDeveloperConsole();
    }

    //Clear the board!
    void DestroyAllRocks()
    {
        Rock[] rocks = FindObjectsOfType<Rock>();

        foreach (Rock rock in rocks)
        {
            if (rock != null)
            {
                rock.Wipe();
            }
        }
    }
}
