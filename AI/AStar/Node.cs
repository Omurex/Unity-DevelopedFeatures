using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int distanceToStart { get; private set; } // How far this node is from the starting node, used to calculate cost
    public int distanceToEnd { get; private set; } // How far this node is from the end node, used to calculate cost

    public bool enabled { get; private set; } // Whether or not this node should be considered in AStar
    public Vector2Int boardPosition { get; private set; }

    public List<Node> connectedNodes { get; private set; } = new List<Node>(); // Nodes directly next to this node
    public Node parentNode { get; private set; } // Node that this node came from


    public Node(Vector2Int boardPos, bool isEnabled = true)
    {
        boardPosition = boardPos;
        enabled = isEnabled;
    }


    // Forms connection from this node to passed in node; Calls FormConnection in 
    // otherNode if this was the first node in the two-way connection process
    public void FormConnection(Node otherNode, bool firstConnection = true)
    {
        if(otherNode == null) return; // Can't connect with node that doesn't exist

        if(connectedNodes.Contains(otherNode) == false)
        {
            connectedNodes.Add(otherNode);
            
            if(firstConnection == true)
            {
                otherNode.FormConnection(this, false);
            }
        }
    }


    public void UpdateDistanceToStart(Vector2Int startPos)
    {
        distanceToStart = parentNode.distanceToStart + (int) ( Mathf.Pow(Mathf.Abs(boardPosition.x - startPos.x), 2) + Mathf.Pow(Mathf.Abs(boardPosition.y - startPos.y), 2) );
    }


    public void UpdateDistanceToStart(int newDistanceToStart)
    {
        distanceToStart = newDistanceToStart;
    }


    public int GetPossibleDistanceToStart(Node testParent, Vector2Int startPos) // Returns what distanceToStart would be with the provided parent
    {
        return testParent.distanceToStart + (int) ( Mathf.Pow(Mathf.Abs(boardPosition.x - startPos.x), 2) + Mathf.Pow(Mathf.Abs(boardPosition.y - startPos.y), 2) );
    }


    public void UpdateDistanceToEnd(Vector2Int endPos)
    {
        distanceToEnd = (int) ( Mathf.Pow(Mathf.Abs(boardPosition.x - endPos.x), 2) + Mathf.Pow(Mathf.Abs(boardPosition.y - endPos.y), 2) );
    }


    public void SetParent(Node node)
    {
        parentNode = node;
    }


    public int GetScore()
    {
        return distanceToStart + distanceToEnd;
    }


    public void SetEnabled(bool isEnabled)
    {
        enabled = isEnabled;
    }
}
