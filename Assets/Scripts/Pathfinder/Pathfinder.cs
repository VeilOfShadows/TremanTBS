using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    GridManager gridManager;
    GridCharacter character;

    Node startNode;
    Node endNode;

    //volatile variables retrieve latest value - good with multi threading
    public volatile bool jobDone = false;
    public volatile float timer;

    public delegate void PathfindingComplete(List<Node> n, GridCharacter character);
    public PathfindingComplete completeCallback;

    List<Node> targetPath;

    public Pathfinder(GridCharacter c, Node start, Node target, PathfindingComplete callback, GridManager gridManager)
    {
        this.gridManager = gridManager;
        character = c;
        startNode = start;
        endNode = target;
        completeCallback = callback;
    }

    public void FindPath()
    {
        targetPath = FindPathActual();
        jobDone = true;
    }

    public void NotifyComplete()
    {
        if (completeCallback != null)
        {
            completeCallback(targetPath, character);
        }
    }

    List<Node> FindPathActual()
    {
        List<Node> foundPath = new List<Node>();
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    if (!currentNode.Equals(openSet[i]))
                    {
                        currentNode = openSet[i];
                    }
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.Equals(endNode))
            {
                foundPath = RetracePath(startNode, currentNode);
                break;
            }
            if(endNode.y == currentNode.y)
            {
                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, endNode);
                            neighbour.parentNode = currentNode;
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach(Node neighbour in GetNeighboursY(currentNode))
                {
                    if(!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, endNode);
                            neighbour.parentNode = currentNode;
                            if(!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }

        return foundPath;
    }

    int GetDistance(Node posA, Node posB)
    {
        int distX = Mathf.Abs(posA.x - posB.x);
        int distZ = Mathf.Abs(posA.z - posB.z);

        //commented code seems to procoke zigzags when finding a path.
        //returning 0 favours more square and smooth paths.

        if (distX > distZ)
        {
            return 0;
            //return 14* distZ + 10 * (distX - distZ);
        }

        return 0;
        //return 14* distZ + 10 * (distZ - distX);

    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> retList = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0 ||
                    x == -1 && z == -1 ||
                    x == 1 && z == -1 ||
                    x == -1 && z == 1 ||
                    x == 1 && z == 1 )
                {
                    continue;
                }

                int _x = x + node.x;
                int _y = node.y;
                int _z = z + node.z;

                Node n = GetNode(_x, _y, _z);

                Node newNode = GetNeighbour(n);

                if (newNode != null)
                {
                    retList.Add(newNode);
                }
            }
        }
        return retList;
    }
    List<Node> GetNeighboursY(Node node)
    {
        List<Node> retList = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                for(int y = -1; y <= 1; y++)
                {                    
                    if (x == 0 && z == 0 ||
                    x == -1 && z == -1 ||
                    x == 1 && z == -1 ||
                    x == -1 && z == 1 ||
                    x == 1 && z == 1)
                    {
                        continue;
                    }

                    int _x = x + node.x;
                    int _y = y + node.y;
                    int _z = z + node.z;

                    Node n = GetNode(_x, _y, _z);

                    Node newNode = GetNeighbourY(n);

                    if (newNode != null)
                    {
                        retList.Add(newNode);
                    }
                }
            }
        }
        return retList;
    }


    Node GetNode(int x, int y, int z)
    {
        return gridManager.GetNode(x, y, z);
    }

    Node GetNeighbour(Node currentNode)
    {
        Node retVal = null;

        if(currentNode != null)
        {
            if (currentNode.isWalkable)
            {
                Node aboveNode = GetNode(currentNode.x, currentNode.y + 1, currentNode.z);
                if(aboveNode == null || aboveNode.isAir || character.isCrouched)
                {
                    retVal = currentNode;
                }
                else
                {
                    
                }
            }
        }

        return retVal;
    }
    Node GetNeighbourY(Node currentNode)
    {
        Node retVal = null;

        if(currentNode != null)
        {
            if (currentNode.isWalkable)
            {
                Node aboveNode = GetNode(currentNode.x, currentNode.y + 1, currentNode.z);
                if(aboveNode == null || aboveNode.isAir || character.isCrouched)
                {
                    retVal = currentNode;
                }
                else
                {
                    
                }
            }
        }

        return retVal;
    }


    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        return path;
    }
}
