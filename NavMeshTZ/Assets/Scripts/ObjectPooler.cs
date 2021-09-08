using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.GetComponent<Players>().cubeID = i;
                obj.GetComponent<Players>().textID.text = i.ToString();
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.GetComponent<Players>().currentHealth = objectToSpawn.GetComponent<Players>().maxHealth;
        objectToSpawn.GetComponent<Players>().healthBar.SetHealth(objectToSpawn.GetComponent<Players>().currentHealth);
        objectToSpawn.GetComponent<Players>().xp = 0;
        objectToSpawn.GetComponent<Players>().textXP.text = "0";
        objectToSpawn.GetComponent<Players>().damage = 5;
        objectToSpawn.GetComponent<Players>().textDamage.text = "5";

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
    
    public void SpawnInPool (string tag, GameObject spawnedObj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
        }
        else
        {
            spawnedObj.GetComponent<Players>().currentHealth = spawnedObj.GetComponent<Players>().maxHealth;
            spawnedObj.GetComponent<Players>().healthBar.SetHealth(spawnedObj.GetComponent<Players>().currentHealth);
            spawnedObj.GetComponent<Players>().xp = 0;
            spawnedObj.GetComponent<Players>().textXP.text = "0";
            spawnedObj.GetComponent<Players>().damage = 5;
            spawnedObj.GetComponent<Players>().textDamage.text = "5";
            poolDictionary[tag].Enqueue(spawnedObj);
        }
    }
}
