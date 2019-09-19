using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IHeapItem<Node> {

    // CODE ADAPTED FROM https://youtu.be/-L-WgKMFuhE //

    public bool isWalkable;
    public Vector3 worldPos;
    public int gridX, gridY, gridZ, movementPenalty;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool _isWalkable, Vector3 _worldPos, int _gridX, int _gridY, int _gridZ, int _penalty)
    {
        isWalkable = _isWalkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        gridZ = _gridZ;
        movementPenalty = _penalty;
    }

    // Calculate fCost //

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {

        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }

    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}
