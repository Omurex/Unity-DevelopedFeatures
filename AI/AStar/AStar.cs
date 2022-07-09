using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar // Pathfinding script used for moving character around other characters
{
    private Dictionary<Vector2Int, Node> nodeBoard = new Dictionary<Vector2Int, Node>();

    private HashSet<Node> disabledNodes = new HashSet<Node>(); // Hashset of all nodes that aren't enabled, used for quick resetting of board

    public AStar() 
    {
    }

    
    public void SetUpNode(Vector2Int boardPosInt)
    {
        if(nodeBoard.ContainsKey(boardPosInt)) // Node already in board
        {
            return;
        }

        Node newNode = new Node(boardPosInt);
        nodeBoard.Add(boardPosInt, newNode);

        // Side by side
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x - 1, boardPosInt.y)));
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x + 1, boardPosInt.y)));
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x, boardPosInt.y - 1)));
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x, boardPosInt.y + 1)));

        // Diagonal
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x - 1, boardPosInt.y - 1)));
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x + 1, boardPosInt.y - 1)));
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x + 1, boardPosInt.y + 1)));
        newNode.FormConnection(nodeBoard.GetValueOrDefault(new Vector2Int(boardPosInt.x - 1, boardPosInt.y + 1)));
    }


    public List<Vector2Int> GetPathBetweenPoints(Vector2Int startPoint, Vector2Int endPoint) // Takes in start and end points, returns path in list of Vector2s
    {
        List<Vector2Int> path = new List<Vector2Int>();

        // Make sure both nodes exist
        if(nodeBoard.ContainsKey(startPoint) == false || nodeBoard.ContainsKey(endPoint) == false)
        {
            return path;
        }

        Node startNode = nodeBoard[startPoint];
        Node currentNode = startNode;
        Node endNode = nodeBoard[endPoint];

        HashSet<Node> openNodes = new HashSet<Node>() { startNode }; // List of nodes that are possible candidates for path
        HashSet<Node> closedNodes = new HashSet<Node>(); // List of nodes that are in the path

        currentNode.SetParent(currentNode);
        UpdateNodeScore(currentNode, startPoint, endPoint);

        while(openNodes.Count > 0)
        {
            currentNode = FindLowestScoreNode(openNodes);
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if(currentNode == endNode)
            {
                path = GetPathFromClosedNodesList(startNode, currentNode);
                break;
            }

            foreach(Node connectedNode in currentNode.connectedNodes) // Loop through connections of currentNode in order to add them to the openNodes list if they aren't already on it
            {
                if(connectedNode.enabled == false || closedNodes.Contains(connectedNode)) // If this node is disabled or has already been selected for the path, go to the next node
                {
                    continue;
                }
                else if(openNodes.Contains(connectedNode)) // If this node is already in openNodes, check to see if the path using this parent instead would be less costly
                {
                    int possibleDistanceFromStart = connectedNode.GetPossibleDistanceToStart(currentNode, startPoint);
                    if(possibleDistanceFromStart < connectedNode.distanceToStart)
                    {
                        connectedNode.SetParent(currentNode);
                        connectedNode.UpdateDistanceToStart(possibleDistanceFromStart);
                    }
                }
                else // If this node isn't already in closed or open list, add it to open list with currentNode as parent
                {
                    connectedNode.SetParent(currentNode);
                    UpdateNodeScore(connectedNode, startPoint, endPoint);
                    openNodes.Add(connectedNode);
                }

            }
        }

        return path; // If there is no path to endPoint, return empty path
    }


    private Node FindLowestScoreNode(HashSet<Node> nodeHashSet) // Returns the best node based on distance from start and distance from end
    {
        HashSet<Node>.Enumerator nodeEnumerator = nodeHashSet.GetEnumerator();
        nodeEnumerator.MoveNext();

        Node lowestNode = nodeEnumerator.Current;
        int lowestScore = lowestNode.GetScore();

        while(nodeEnumerator.MoveNext())
        {
            int newScore = nodeEnumerator.Current.GetScore();
            if(newScore < lowestScore)
            {
                lowestNode = nodeEnumerator.Current;
                lowestScore = newScore;
            }
        }

        return lowestNode;
    }


    private void UpdateListOfNodeScores(List<Node> nodeList, Vector2Int startPoint, Vector2Int endPoint)
    {
        foreach(Node node in nodeList)
        {
            UpdateNodeScore(node, startPoint, endPoint);
        }
    }


    private void UpdateNodeScore(Node node, Vector2Int startPoint, Vector2Int endPoint)
    {
        node.UpdateDistanceToStart(startPoint);
        node.UpdateDistanceToEnd(endPoint);
    }


    private List<Vector2Int> GetPathFromClosedNodesList(Node firstNode, Node lastNode) // Uses a "Linked List" type of structure to go back through the nodes and get the path
    {
        List<Vector2Int> pathToEnd = new List<Vector2Int>();
        Node currentNode = lastNode; // We know that this has to be the endNode, so we work backwards from here

        while(currentNode != firstNode)
        {
            pathToEnd.Add(currentNode.boardPosition);
            currentNode = currentNode.parentNode;
        }

        pathToEnd.Add(firstNode.boardPosition);

        pathToEnd.Reverse(); // Since we started at the end, we must reverse the list to have the path from the start to the end

        return pathToEnd;
    }


    public void TogglePointEnabled(Vector2Int point, bool isEnabled)
    {
        Node node = nodeBoard.GetValueOrDefault(point);

        if(node == null) return;

        node.SetEnabled(isEnabled);

        if(isEnabled == false && disabledNodes.Contains(node) == false)
        {
            disabledNodes.Add(node);
        }
        else if(isEnabled == true && disabledNodes.Contains(node) == true)
        {
            disabledNodes.Remove(node);
        }
    }


    public void TogglePointEnabled(List<Vector2Int> pointList, bool isEnabled)
    {
        foreach(Vector2Int point in pointList)
        {
            TogglePointEnabled(point, isEnabled);
        }
    }


    public void ResetBoardEnabled()
    {
        foreach(Node node in disabledNodes)
        {
            node.SetEnabled(true);
        }

        disabledNodes.Clear();
    }


    public Node GetNode(Vector2Int point)
    {
        return nodeBoard.GetValueOrDefault(point);
    }
}
