using com.cyborgAssets.inspectorButtonPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController>
{
    public GameObject operationsObjectPrefab;
    public Transform operationsContainer;

    Dictionary<GameObject, Operations> operationsDictionary = new Dictionary<GameObject, Operations>();

    public void AssignOperationsToObject(GameObject obj, string text)
    {
        if (operationsDictionary.ContainsKey(obj))
        {
            return;
        }

        GameObject newOpObject = Instantiate(operationsObjectPrefab, operationsContainer);

        Vector3 newPosition = obj.transform.position;
        newPosition.z -= 8;
        newOpObject.transform.position = newPosition;

        Operations newOp = new Operations(newOpObject, text);
        operationsDictionary[obj] = newOp;
    }

    public void RemoveOperationsFromObject(GameObject obj)
    {
        if (operationsDictionary.ContainsKey(obj))
        {
            Destroy(operationsDictionary[obj].operationsObject);
            operationsDictionary.Remove(obj);
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        foreach (var pair in operationsDictionary)
        {
            GameObject obj = pair.Key;
            Operations op = pair.Value;
            if (obj != null && op != null && op.operationsObject != null)
            {
                op.operationsObject.transform.position = obj.transform.position;

                Vector3 newPosition = obj.transform.position;
                newPosition.z -= 8;
                op.operationsObject.transform.position = newPosition;
            }
        }
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public class Operations
{
    public GameObject operationsObject;
    public Text text;

    public Operations(GameObject opObject, string displayText)
    {
        operationsObject = opObject;
        text = operationsObject.GetComponentInChildren<Text>();
        text.text = displayText;
    }
}
