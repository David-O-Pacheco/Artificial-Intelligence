using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{

    bool turretEnter, targetMinion = false;
    GameObject getEntity, getPlayer;
    public GameObject turretBullet, bulletInst;
    Transform turretHead;
    float timer;

    void Start()
    {

        turretHead = gameObject.transform.GetChild(0);
        getPlayer = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {

        // Shoot minions or player every 2 seconds //

        if (turretEnter && !targetMinion)
        {
            timer += Time.deltaTime;
            if (timer > 2.0f)
            {
                bulletInst = Instantiate(turretBullet, turretHead.transform.position, Quaternion.identity);
                timer = 0;
            }

            if (getEntity != null)
            {
                Debug.DrawRay(turretHead.transform.position, getEntity.transform.position - turretHead.transform.position, Color.yellow);
            }
        }

        if (turretEnter && targetMinion)
        {
            timer += Time.deltaTime;
            if (timer > 2.0f)
            {
                bulletInst = Instantiate(turretBullet, turretHead.transform.position, Quaternion.identity);
                timer = 0;
            }

            if (getEntity != null)
            {
                Debug.DrawRay(turretHead.transform.position, getEntity.transform.position - turretHead.transform.position, Color.yellow);
            }
        }

        if (getPlayer != null)
        {
            if (targetMinion)
            {
                if (bulletInst != null && getEntity != null)
                {
                    bulletInst.transform.position = Vector3.MoveTowards(bulletInst.transform.position, getEntity.transform.position, 0.2f);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Minion2")
        {
            getEntity = collision.gameObject;
            targetMinion = true;
            turretEnter = true;
        }

        if (collision.gameObject.tag == "MinionBullet2")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Minion2")
        {
            turretEnter = false;
            targetMinion = false;
            timer = 0;
        }

        if (collision.gameObject.tag == "Player")
        {
            timer = 0;
        }
    }
}
