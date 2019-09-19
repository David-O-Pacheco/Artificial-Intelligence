using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base1BotLaneUnit : MonoBehaviour {

    Transform collidedObj;
    public GameObject bulletInst, instPrefab;
    GameObject[] waypointsBot;
    GameObject getSpawnpoint, target;
    float timer;
    float speed = 2;
    Vector3[] path;
    int targetIndex, currWP;


    // Creates a statemachine //

    public enum StateMachine
    {
        Move,
        AttackPlayer,
        AttackTower,
        AttackMinion
    }

    public StateMachine state = StateMachine.Move;

    void Start()
    {
        getSpawnpoint = GameObject.Find("Pathfinding");
        target = getSpawnpoint.GetComponent<SpawnMinion1>().bot1Waypoints[0];

        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    void Update()
    {
        // Check which state is active for Bot Lane Minions //

        if (state == StateMachine.Move)
        {
            // MOVE STATE //

            target = getSpawnpoint.GetComponent<SpawnMinion1>().bot1Waypoints[currWP];
        }
        else if (state == StateMachine.AttackPlayer)
        {
            StopCoroutine("FollowPath");
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                instPrefab = Instantiate(bulletInst, transform.position, Quaternion.identity);
                timer = 0;
            }
        }
        else if (state == StateMachine.AttackMinion)
        {
            // ATTACK MINION STATE //

            StopCoroutine("FollowPath");
            path = null;
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                if (collidedObj != null)
                {
                    Destroy(collidedObj.transform.gameObject, 1);
                }
                timer = 0;
            }

            if (gameObject != null)
            {
                if (collidedObj == null)
                {
                    if (gameObject != null)
                    {
                        state = StateMachine.Move;
                        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
                    }
                }
            }
        }
        else if (state == StateMachine.AttackTower)
        {
            // ATTACK TOWER STATE //

            StopCoroutine("FollowPath");
            path = null;

            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                if (collidedObj != null)
                {
                    Destroy(collidedObj.transform.gameObject, 1);
                }
                timer = 0;
            }

            if (gameObject != null)
            {
                if (collidedObj == null)
                {
                    if (gameObject != null)
                    {
                        state = StateMachine.Move;
                        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
                    }
                }
            }
        }
    }

    // Change the state machine when colliding with targets //

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = StateMachine.AttackPlayer;
        }

        if (other.gameObject.tag == "Minion2")
        {
            collidedObj = other.gameObject.transform;
            state = StateMachine.AttackMinion;
        }
        if (other.gameObject.tag == "TowerBase2")
        {
            collidedObj = other.gameObject.transform;
            state = StateMachine.AttackTower;
        }
        if (other.gameObject.tag == "TurretBullet2" || other.gameObject.tag == "MinionBullet2")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    // Reset states when leaving the trigger collisions //

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject != null)
            {
                state = StateMachine.Move;
            }
        }

        if (other.gameObject.tag == "Minion2")
        {
            if (gameObject != null)
            {
                state = StateMachine.Move;
            }
        }

        if (other.gameObject.tag == "TowerBase2")
        {
            if (gameObject != null)
            {
                state = StateMachine.Move;
            }
        }
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

        }
    }

    // Goes from start to target by creating waypoints to the target //

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = null;
                    if (currWP < getSpawnpoint.GetComponent<SpawnMinion1>().bot1Waypoints.Length - 1)
                    {
                        currWP++;
                        targetIndex = 0;
                        target = getSpawnpoint.GetComponent<SpawnMinion1>().bot1Waypoints[currWP];
                        if (gameObject != null)
                        {
                            PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
                        }
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    // Draws gizmos lines and squares for path //

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(path[i], new Vector3(0.4f, 0.4f, 0.4f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
