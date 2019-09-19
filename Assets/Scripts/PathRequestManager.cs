using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PathRequestManager : MonoBehaviour
{

    // CODE ADAPTED FROM https://youtu.be/-L-WgKMFuhE //

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    ASTAR aStarAlgorithm;

    static PathRequestManager instance;

    bool isProcessingPath;

    void Awake()
    {
        aStarAlgorithm = GetComponent<ASTAR>();

        instance = this;
    }

    // Request path from start point to target point //

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    // Run AStar Algorithm for current path request //

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {

            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;

            aStarAlgorithm.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // Return success of path //

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

}
