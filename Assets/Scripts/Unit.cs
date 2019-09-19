using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    Transform target, player;
    public GameObject bulletInst, instPrefab;
    public Transform[] waypointsTop;
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            // ATTACK MINION STATE //

            StopCoroutine("FollowPath");
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                instPrefab = Instantiate(bulletInst, transform.position, Quaternion.identity);
                timer = 0;
            }
        }

        if (instPrefab != null)
        {
            instPrefab.transform.position = Vector3.MoveTowards(instPrefab.transform.position, player.transform.position, 0.2f);
        }
    }

    // Change the state machine when colliding with targets //

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = StateMachine.AttackPlayer;
        }
    }

    // Reset states when leaving the trigger collisions //

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = StateMachine.Move;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
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
                Gizmos.DrawCube(path[i], new Vector3(0.4f,0.4f,0.4f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
