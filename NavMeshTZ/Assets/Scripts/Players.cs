using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    private Transform _target; // Указываем переменную, к которой будет двигаться наш агент  
    NavMeshAgent _agent; // Указываем переменную агента
    public GameObject[] players;
    public int currentHealth;
    public int maxHealth;
    public int xp;
    public int damage;
    public int damageAmp;
    public int damageMax;
    public Text textDamage;
    public Text textXP;
    public string[] winners = new string[5];
    public HealthBar healthBar;
    public int cubeID;
    public Text textID;
    private GameObject player;
    private bool is_target;
    ObjectPooler objectPooler;
    private static System.Random _global = new System.Random();
    [ThreadStatic]
    private static System.Random _local;

    // Start is called before the first frame update
    void Start()
    {
        _agent = (NavMeshAgent)this.GetComponent("NavMeshAgent"); // Указываем, что переменная _agent - это наш агент.
        is_target = false;
        maxHealth = 100;
        currentHealth = maxHealth;
        Time.fixedDeltaTime = 1f;
        healthBar.SetMaxHealth(maxHealth);
        objectPooler = ObjectPooler.Instance;
        damage = 5;
        damageMax = 50;
        damageAmp = 5;
        xp = 0;
        textDamage.text = damage.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length <= 1)
        {
            Application.Quit();
        }
        else
        {
            players = SelectionSort(players);
            if (players.Length >= winners.Length)
            {
                for (int i = 0; i<winners.Length; i++)
                {
                    winners[i] = players[i].GetComponent<Players>().cubeID.ToString();
                }
            }
            else
            {
                for (int i = 0; i< players.Length; i++)
                {
                    winners[i] = players[i].GetComponent<Players>().cubeID.ToString();
                }
                for(int i = players.Length; i < winners.Length; i++)
                {
                    winners[i] = "";
                }
            }
            for (int i = 0; i < winners.Length; i++)
            {
                PlayerPrefs.SetString(i.ToString(), winners[i]);
            }

            if (!is_target)
            {
                player = players[Next(0, players.Length - 1)];
                while (_agent.transform.position == player.transform.position)
                {
                    player = players[Next(0, players.Length)];
                }
                _target = player.transform;
                _agent.SetDestination(_target.position); // Заставляем агента двигаться в сторону _target'а 
                is_target = true;
            }
            else
            {
                if (players.Length > 1)
                {
                    _agent.SetDestination(_target.position); // Заставляем агента двигаться в сторону _target'а 
                }
            }
            if (player==null)
            {
                is_target = false;
            }
            if (player.GetComponent<Players>().currentHealth <= 0)
            {
                objectPooler.SpawnInPool("Cube", player);
                player.SetActive(false);
                is_target = false;
                if (damage < damageMax)
                {
                    damage += damageAmp;
                    textDamage.text = damage.ToString();
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (_target != null && is_target)
        {
            if (Vector3.Distance(_agent.transform.position, _target.position) < 2f)
            {
                //Debug.Log("Fixed");
                xp += 1;
                textXP.text = xp.ToString();
                player.GetComponent<Players>().currentHealth -= damage;
                player.GetComponent<Players>().healthBar.SetHealth(player.GetComponent<Players>().currentHealth);
            }
            //Debug.Log(Vector3.Distance(_agent.transform.position, _target.position));
        }
    }

    public static int Next(int x1, int x2)
    {
        System.Random inst = _local;
        if (inst == null)
        {
            int seed;
            lock (_global) seed = _global.Next(x1,x2);
            _local = inst = new System.Random(seed);
        }
        return inst.Next(x1,x2);
    }
    GameObject[] SelectionSort(GameObject[] unsortedList)
    {
        int max;
        GameObject temp;

        for (int i = 0; i<unsortedList.Length;i++)
        {
            max = i;
            for(int j=i+1; j<unsortedList.Length;j++)
            {
                if(unsortedList[j].GetComponent<Players>().damage> 
                    unsortedList[max].GetComponent<Players>().damage)
                {
                    max = j;
                }
            }
            if(max!=i)
            {
                temp = unsortedList[i];
                unsortedList[i] = unsortedList[max];
                unsortedList[max] = temp;
            }
        }
        return unsortedList;
    }
}
