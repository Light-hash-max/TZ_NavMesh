using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spawner : MonoBehaviour
{

   // public GameObject MobToSpawn;
    public Camera mainCamera;
    public int numberOfMobsAtOnce;
    private readonly int spawnRange = 9;
    //public List<GameObject> mobsSpawned;
    ObjectPooler objectPooler;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numberOfMobsAtOnce; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(transform.position.x - spawnRange, transform.position.x + spawnRange),
                transform.position.y,
                Random.Range(transform.position.z - spawnRange, transform.position.z + spawnRange));

            // GameObject mobSpawned = Instantiate(MobToSpawn, randomPosition, Quaternion.identity) as GameObject;
            //mobSpawned.transform.parent = this.transform;
            //mobsSpawned.Add(mobSpawned);
            objectPooler = ObjectPooler.Instance;
            objectPooler.SpawnFromPool("Cube", randomPosition, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                objectPooler.SpawnFromPool("Cube", hit.point, Quaternion.identity);
            }
            /*if (MobToSpawn == null)
            {
                MobToSpawn = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    MobToSpawn.GetComponent<Players>().currentHealth = MobToSpawn.GetComponent<Players>().maxHealth;
                    MobToSpawn.GetComponent<Players>().healthBar.SetHealth(MobToSpawn.GetComponent<Players>().currentHealth);
                    MobToSpawn.GetComponent<Players>().xp = 0;
                    MobToSpawn.GetComponent<Players>().textXP.text = "0";
                    GameObject mobSpawned = Instantiate(MobToSpawn, hit.point, Quaternion.identity) as GameObject;
                    mobSpawned.transform.parent = this.transform;
                    mobsSpawned.Add(mobSpawned);
                }
            }*/
        }
    }


}
