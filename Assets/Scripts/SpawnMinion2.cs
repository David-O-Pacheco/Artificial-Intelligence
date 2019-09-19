using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinion2 : MonoBehaviour {

    public GameObject[] bot2Waypoints, top2Waypoints;
    public GameObject Base2BotSpawn, Base2MidSpawn, Base2TopSpawn, T2Bot, T2Mid, T2Top;
    float timer, spawnTimer, spawnIteration;

    void Start ()
    {

        timer = 25;

    }

    void Update ()
    {

        // Spawn 4 minions for base2 in 3 different locations //

        timer += Time.deltaTime;
        if (timer > 30)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > 1f && spawnIteration < 4)
            {
                Instantiate(T2Bot, Base2BotSpawn.transform.position, Quaternion.identity);
                Instantiate(T2Mid, Base2MidSpawn.transform.position, Quaternion.identity);
                Instantiate(T2Top, Base2TopSpawn.transform.position, Quaternion.identity);
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
