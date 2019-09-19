using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit1 : MonoBehaviour {

    Transform target, collidedObj;
    public GameObject bulletInst, instPrefab;
    float timer;
    Vector3 startPos;
    float speed = 2;
    Vector3[] path;
    int targetIndex;

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
        target = GameObject.FindGameObjectWithTag("Target").transform;
        startPos = transform.position;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        // Check which state is active for Bot Lane Minions //

        if (state == StateMachine.Move)
        {
            // MOVE STATE //

            target = GameObject.FindGameObjectWithTag("Target").transform;
        }
        else if (state == StateMachine.AttackPlayer)
        {
            // ATTACK PLAYER STATE //

            StopCoroutine("FollowPath");
            path = null;
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                instPrefab = Instantiate(bulletInst, transform.position, Quaternion.identity);
                timer = 0;
            }

            if (instPrefab != null)
            {
                instPrefab.transform.position = Vector3.MoveTowards(instPrefab.transform.position, collidedObj.transform.position, 0.2f);
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
                    Destroy(collidedObj.transform.gameObject, 0.8f);
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
                    transform.position = startPos;
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
