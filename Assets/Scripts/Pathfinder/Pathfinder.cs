using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    #region Variables
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
    #endregion

    #region Initialisation
    //Constructor to assign the variables of the Pathfinder instance. Used to connect scripts
    public Pathfinder(GridCharacter c, Node start, Node target, PathfindingComplete callback, GridManager gridManager)
    {
        this.gridManager = gridManager;
        character = c;
        startNode = start;
        endNode = target;
        completeCallback = callback;
    }

    //Function is used to call the FindPathActual() which will give our targetPath variable a value. 
    //jobDone is for the multi threading so the pathfinder won't try to calculate a million paths at once.
    //Once a path is found, it will mark the job as done and pick up another one
    public void FindPath()
    {
        targetPath = FindPathActual();
        jobDone = true;
    }

    //Communicates the completion of a pathfinding job to other scripts
    public void NotifyComplete()
    {
        if (completeCallback != null)
        {
            completeCallback(targetPath, character);
        }
    }
    #endregion

    #region Pathfinding
    //Main pathfinding fuunction. Loops through the nodes to find which neighbouring node is closer to the target, moving one by one towards the goal
    List<Node> FindPathActual()
    {
        List<Node> foundPath = new List<Node>();

        //openSet is the list of nodes which are yet to be checked
        List<Node> openSet = new List<Node>();
        //closedSet is a list of nodes which have been checked already
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        //while the openSet has nodes in it, find their neighbours and add them to the openSet
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            //calculates the cost of each node. Lower costs mean closer to the target node
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

            //prevents overlap and double checking of the same node
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //the node being checked is the target node. Path is found, exit the loop
            if (currentNode.Equals(endNode))
            {
                foundPath = RetracePath(startNode, currentNode);
                break;
            }

            //check the elevation of the target node. Determines how neighbours are found
            if(endNode.y == currentNode.y)
            {
                //target node is on the same elevation, ignore vertical neighbours when finding a path
                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    if (!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        //find the cost of  each neighbour. If the cost is lower, that neighbour is added to the openSet
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
                //target node is on a different elevation, include vertical neighbours when finding a path
                foreach(Node neighbour in GetNeighboursY(currentNode))
                {
                    if(!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                        //find the cost of  each neighbour. If the cost is lower, that neighbour is added to the openSet
                        if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            //if the neighbour y value is not walkable, ignore it
                            if(neighbour.y != currentNode.y)
                            {
                                //Check if the node above is walkable, if it isn't then you can't move vertically from here
                                Node newNode = CheckAbove(currentNode);
                                if(newNode != null)
                                {
                                    neighbour.gCost = newMovementCostToNeighbour;
                                    neighbour.hCost = GetDistance(neighbour, endNode);
                                    neighbour.parentNode = currentNode;
                                    if(!openSet.Contains(neighbour))
                                    {
                                        openSet.Add(neighbour);
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
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
        }

        return foundPath;
    }

    //finds the distance between two nodes. Irellevant for NESW movement
    int GetDistance(Node posA, Node posB)
    {
        int distX = Mathf.Abs(posA.x - posB.x);
        int distZ = Mathf.Abs(posA.z - posB.z);

        //commented code seems to promote zigzags when finding a path.
        //returning 0 favours more square and smooth paths.

        if (distX > distZ)
        {
            return 0;
            //return 14* distZ + 10 * (distX - distZ);
        }

        return 0;
        //return 14* distZ + 10 * (distZ - distX);

    }

    //takes a node and finds it's 4 neighbours.
    List<Node> GetNeighbours(Node node)
    {
        List<Node> retList = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                //if the node being checked is on the corners, ignore them and don't check. Ensures no diagonal pathfinding
                if(x == 0 && z == 0 ||
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

                //finds the node in the grid array from the currentNode + the current neighbour it is searching
                Node n = GetNode(_x, _y, _z);

                //checks if the neighbours are walkable or not
                Node newNode = GetNeighbour(n);

                //if the node returns null, it is not walkable and not added to the neighbours list
                if(newNode != null)
                {
                    retList.Add(newNode);
                }
            }
        }
        return retList;
    }

    //takes a node and finds all neighbours, ignoring corners
    List<Node> GetNeighboursY(Node node)
    {
        List<Node> retList = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                for(int y = -1; y <= 1; y++)
                {        
                    //if the node being checked is on the corners, ignore them and don't check. Ensures no diagonal pathfinding
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

                    //finds the node in the grid array from the currentNode + the current neighbour it is searching
                    Node n = GetNode(_x, _y, _z);

                    //checks if the neighbours are walkable or not
                    Node newNode = GetNeighbour(n);

                    //if the node returns null, it is not walkable and not added to the neighbours list
                    if (newNode != null)
                    {
                        retList.Add(newNode);
                    }
                }
            }
        }
        return retList;
    }

    //returns if neighbours are walkable or not, removing non-walkable neighbours from consideration
    Node GetNeighbour(Node currentNode)
    {
        Node retVal = null;

        if(currentNode != null)
        {
            if (currentNode.isWalkable)
            {
                //Node aboveNode = GetNode(currentNode.x, currentNode.y + 1, currentNode.z);
                //if(aboveNode == null || aboveNode.isAir || character.isCrouched)
                //{
                //}
                //else
                //{
                    
                //}
                retVal = currentNode;
            }
        }

        return retVal;
    }

    //finds the node in the grid array, using the given position
    Node GetNode(int x, int y, int z)
    {
        return gridManager.GetNode(x, y, z);
    }

    //When pathfinding on the Y axis, will check for empty nodes above
    Node CheckAbove(Node currentNode)
    {
        Node retVal = null;

        if(currentNode != null)
        {
            if(currentNode.isWalkable)
            {
                //check if the node above the current node is an obstacle. return null if it is an obstacle
                Node aboveNode = GetNode(currentNode.x, currentNode.y + 1, currentNode.z);
                if(!aboveNode.obstacle)
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

    //retraces the path backwards from the target node to the sart node and populates the path variable with the found nodes
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
    #endregion
}
