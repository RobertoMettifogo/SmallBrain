using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;

public class SpawnController : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public GameController gameController;
    public float CameraOffset = 2;
    public UIController uiController;
    public string String = "some Value";
    private Coroutine _Spawning;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    [SerializeField]
    public int spawnedObjectCount { get; private set; }
    [SerializeField]
    private float difficulty = 1f;
    [SerializeField]
    private float percentageAdd = 0.8f;
    [SerializeField]
    private float percentageSub = 0.5f;
    [SerializeField]
    private float percentageMulti = 0.3f;
    [SerializeField]
    private float percentageDiv = 0.1f;

    Dictionary<MathOperation, IOperation> opDictionary = new Dictionary<MathOperation, IOperation>();

    void Start()
    {
        opDictionary.Add(MathOperation.Addition, new AdditionOperation());
        opDictionary.Add(MathOperation.Subtraction, new SubtractionOperation());
        opDictionary.Add(MathOperation.Multiplication, new MultiplicationOperation());
        opDictionary.Add(MathOperation.Division, new DivisionOperation());
        StartSpawning();
    }

    private IOperation RandomOperationSpawnChance()
    {
        MathOperation chosenOperation = MathOperation.Addition;

        if (Random.value <= percentageAdd) //%80 percent chance
        {
            chosenOperation = MathOperation.Addition;
        }
        else if (Random.value <= percentageSub) //%50 percent chance
        {
            chosenOperation = MathOperation.Subtraction;
        }
        else if (Random.value <= percentageMulti) //%30 percent chance 
        {
            chosenOperation = MathOperation.Multiplication;
        }
        else if (Random.value <= percentageDiv) //%10 percent chance 
        {
            chosenOperation = MathOperation.Division;
        }
            return opDictionary[chosenOperation];
    }

    private IEnumerator Spawning()
    {
        while (true)
        {
            WaitForSeconds wait = new WaitForSeconds(5f - difficulty);
            SpawnObject();
            yield return wait;
        }
    }

    public void StartSpawning()
    {
        if (_Spawning == null)
        {
            _Spawning = StartCoroutine(Spawning());
        }
    }

    public void StopSpawning()
    {
        if (_Spawning != null)
        {
            StopCoroutine(_Spawning);
            _Spawning = null;
        }
    }

    // We have our enum of possible operation, we can easly add or remove
    public enum MathOperation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    void SpawnObject()
    {
        //Where we ll spawn the object?
        Camera mainCamera = Camera.main;
        float CameraWdht = mainCamera.aspect * mainCamera.orthographicSize;
        Vector3 SpawnPosition = new Vector3(Random.Range(-CameraWdht +1, CameraWdht -1), (mainCamera.orthographicSize + CameraOffset), 0);

        //Instantiate the rock in initial position & add the rock to list spawnedObjects
        GameObject spawnedObject = Instantiate(ObjectToSpawn, SpawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawnedObject);

        //Chose and operation and set this operation to the text via UI Monosingleton
        IOperation randomOperation = RandomOperationSpawnChance();
        (string operationText, string operationResult) = randomOperation.GiveMeOperation(difficulty);
        UIController.Instance.AssignOperationsToObject(spawnedObject, operationText);

        // Get the correct answer for the spawned operation
        int correctAnswer = randomOperation.GetCorrectAnswer();

        // Set the correct answer for the rock
        Rock rock = spawnedObject.GetComponent<Rock>();
        if (rock != null)
        {
            rock.SetCorrectAnswer(correctAnswer);
        }
    }

    public List<GameObject> GetSpawnedObjects()
    {
        return spawnedObjects;
    }


    //Special rock will destroy in a radius
    public void DestroyObjectsInRadius(Vector3 center, float radius)
    {
        //Add the object in the radius to a list
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null && Vector3.Distance(obj.transform.position, center) <= radius)
            {
                Destroy(obj);
                gameController.IncrementScore();
                objectsToRemove.Add(obj);
            }
        }
        //Once destroy we remove and clear up the list
        foreach (GameObject objToRemove in objectsToRemove)
        {
            spawnedObjects.Remove(objToRemove);
        }
    }
    //Reset the count to reach special spawn point
    public void ResetSpawnedObjectCount()
    {
        spawnedObjectCount = 0;
    }

    public void DifficultStep1()
    {
        percentageAdd = 0.5f;
        percentageSub = 0.5f;
        percentageMulti = 0.4f;
        percentageDiv = 0.3f;
        DiffUP();
        Debug.Log("StepUP");
    }

    public void DifficultStep2()
    {
        percentageAdd = 0.4f;
        percentageSub = 0.4f;
        percentageMulti = 0.6f;
        percentageDiv = 0.5f;
        DiffUP();
        Debug.Log("StepUPPPER");
    }

    public void DiffUP()
    {
        difficulty++;
        StopSpawning();
        StartSpawning();
    }

    //EX INVOKE METHOD LESS EFFICENT THAN COROUTINES
    void StartSpawningInvoke(float interval)
    {
        float spawnrate = 5f - difficulty;
        interval = spawnrate;
        //We call the SpawnObject methos for the period of interval
        InvokeRepeating("SpawnObject", 0, interval);
    }

    void StopSpawningInvoke()
    {
        CancelInvoke("SpawnObject");
    }
}