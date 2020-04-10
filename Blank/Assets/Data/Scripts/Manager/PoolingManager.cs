// Création du script : Benjamin Chouinard
// Date : 02-04-2019 16:15
// Modification : de base
// Date de modification : 4 avril par Maxime

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager : MonoBehaviour
{
    /*[SerializeField]*/ public PoolObject[] poolArray;

    /*private*/public Dictionary<string, PoolObject> pool = new Dictionary<string, PoolObject>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < poolArray.Length; i++)
        {
            PoolObject obj = poolArray[i];
            obj.Initialise();
            pool.Add(obj.name, obj);
        }
    }


    public GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        PoolObject poolObject;
        //Place requested Object in obj, if its null, pop error
        if (!pool.TryGetValue(name, out poolObject))
        {
            //Debug.LogError("<color=red>WRONG KEY : " + name + "</color>");
            return null;
        }
        GameObject go = poolObject.GetNext();
        go.transform.position = position;
        go.transform.rotation = rotation;
        //Debug.Log(go.name);
        return go;
    }


    public GameObject GetObjectAutoReturn(string name, Vector3 position, Quaternion rotation, float timeBeforeReturn, System.Action callback)
    {
        GameObject gameObject = GetObject(name, position, rotation);
        StartCoroutine(AutoReturn(name, gameObject, timeBeforeReturn, callback));
        return gameObject;
    }

    #region GetObject Overload
    public GameObject GetObject(string name, Vector3 position, Vector3 eulerAngles)
    {
        return GetObject(name, position, Quaternion.Euler(eulerAngles));
    }

    public GameObject GetObject(string name, Vector3 position)
    {
        return GetObject(name, position, Quaternion.identity);
    }

    public GameObject GetObject(string name)
    {
        return GetObject(name, Vector3.zero, Quaternion.identity);
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, Vector3 eulerAngles, float timeBeforeReturn)
    {
        return GetObjectAutoReturn(name, position, Quaternion.Euler(eulerAngles), timeBeforeReturn, null);
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, float timeBeforeReturn)
    {
        return GetObjectAutoReturn(name, position, Quaternion.identity, timeBeforeReturn, null);
    }

    public GameObject GetObjectAutoReturn(string name, float timeBeforeReturn)
    {
        return GetObjectAutoReturn(name, Vector3.zero, Quaternion.identity, timeBeforeReturn, null);
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, Vector3 eulerAngles, float timeBeforeReturn, System.Action callback)
    {
        return GetObjectAutoReturn(name, position, Quaternion.Euler(eulerAngles), timeBeforeReturn, callback);
    }

    public GameObject GetObjectAutoReturn(string name, Vector3 position, float timeBeforeReturn, System.Action callback)
    {
        return GetObjectAutoReturn(name, position, Quaternion.identity, timeBeforeReturn, callback);
    }

    public GameObject GetObjectAutoReturn(string name, float timeBeforeReturn, System.Action callback)
    {
        return GetObjectAutoReturn(name, Vector3.zero, Quaternion.identity, timeBeforeReturn, callback);
    }
    #endregion

    public void ReturnObject(string name, GameObject gameObject)
    {
        TryReturnObject(name, gameObject);
    }

    IEnumerator AutoReturn(string name, GameObject gameObject, float timeBeforeReturn, System.Action callback)
    {
        yield return new WaitForSeconds(timeBeforeReturn);

        bool tryReturnObjectSucess = TryReturnObject(name, gameObject);

        if (tryReturnObjectSucess)
        {
            if (callback != null)
                callback.Invoke();
        }
    }

    bool TryReturnObject(string name, GameObject gameObject)
    {
        if (pool.ContainsKey(name))
        {
            //If the object is already in pool, dont return it in pol
            if (pool[name].IsObjectInPool(gameObject))
                return false;

            pool[name].ReturnObject(gameObject);
            return true;
        }
        else
        {
            //Debug.LogError("<color=red>WRONG KEY : " + name + "</color>");
            return false;
        }
    }

    [System.Serializable]
    public class PoolObject
    {
        public string name;
        public GameObject prefab;
        public int quantity;

        private Queue<GameObject> queue;
        private Dictionary<GameObject, bool> inPoolDictionary = new Dictionary<GameObject, bool>();

        GameObject objParent;

        public void Initialise()
        {
            objParent = new GameObject(name);
            queue = new Queue<GameObject>();

            for (int i = 0; i < quantity; i++)
            {
                AddNewObject();
            }
        }

        void AddNewObject()
        {
            GameObject go = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            inPoolDictionary.Add(go, true);

            go.transform.SetParent(objParent.transform);
            go.SetActive(false);
            queue.Enqueue(go);
        }

        public void ReturnObject(GameObject gameObject)
        {
            inPoolDictionary[gameObject] = true;
            queue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        public bool IsObjectInPool(GameObject gameObject)
        {
            return inPoolDictionary[gameObject];
        }

        public GameObject GetNext()
        {
            if (queue.Count == 0)
                AddNewObject();

            GameObject currentObj = queue.Dequeue();
            inPoolDictionary[currentObj] = false;
            currentObj.SetActive(true);
            return currentObj;
        }
    }
}
