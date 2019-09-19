using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion1 : MonoBehaviour {

    public GameObject[] bot1Waypoints, top1Waypoints;
    public GameObject Base1BotSpawn, Base1MidSpawn, Base1TopSpawn, T1Bot, T1Mid, T1Top;
    float timer, spawnTimer, spawnIteration;

    void Start ()
    {
        timer = 25;

	}
	
	void Update ()
    {

        // Spawn 4 minions for base1 in 3 different locations //

        timer += Time.deltaTime;
        if (timer > 30)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > 1f && spawnIteration < 4)
            {
                Instantiate(T1Bot, Base1BotSpawn.transform.position, Quaternion.identity);
                Instantiate(T1Mid, Base1MidSpawn.transform.position, Quaternion.identity);
                Instantiate(T1Top, Base1TopSpawn.transform.position, Quaternion.identity);
                spawnTimer = 0;
                spawnIteration++;
            }
            else if (spawnIteration >= 4)
            {
                timer = 0;
                spawnTimer = 0;
                spawnIteration = 0;
            }
        }

	}
}
