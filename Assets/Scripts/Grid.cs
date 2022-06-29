using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public bool onlyDisplayPathGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;
	public Pathfinding pathfindingScript;

    public List<Node> movement;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}

    public void GetMovementDistance()
	{		
		List<Node> movementNodes = new List<Node>();
		Node startNode = NodeFromWorldPoint(pathfindingScript.seeker.position);
		int speed = 5;
        int stepcount = 0;

        var tileForPreviousStep = new List<Node>();
        tileForPreviousStep.Add(startNode);

        while (stepcount < speed)
        {
            List<Node> neighbours = new List<Node>();

            foreach (Node n in tileForPreviousStep)
            {
                neighbours.AddRange(GetNeighboursWalkable(n));
            }

            movementNodes.AddRange(neighbours);
            tileForPreviousStep = neighbours;
            stepcount++;           
        }
        movement = movementNodes;
    }

    public List<Node> GetNeighboursWalkable(Node node)
    {
        List<Node> neighbours = new List<Node>();

        #region Non diagonal pathfinding
        //loop for adjacent movement, ignoring diagonals
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 ||//centre
                    x == -1 && y == -1 ||//bottom left
                    x == 1 && y == -1 ||//bottom right
                    x == -1 && y == 1 ||//top left
                    x == 1 && y == 1)  //top right
                    continue;               

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (grid[checkX, checkY].walkable)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }
        #endregion

        #region Diagonal inclusive pathfinding
        //######Un-comment this section and then comment out the for loop above this region to change pathfinding from square based to diagonal inclusive.
        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        if (x == 0 && y == 0)
        //            continue;

        //        int checkX = node.gridX + x;
        //        int checkY = node.gridY + y;

        //        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        //        {
        //            neighbours.Add(grid[checkX, checkY]);
        //        }
        //    }
        //}
        #endregion

        return neighbours;
    }


    public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

        #region Non diagonal pathfinding
        //loop for adjacent movement, ignoring diagonals
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 ||//centre
                    x == -1 && y == -1 ||//bottom left
                    x == 1 && y == -1 ||//bottom right
                    x == -1 && y == 1 ||//top left
                    x == 1 && y == 1)  //top right
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        #endregion

        #region Diagonal inclusive pathfinding
        //######Un-comment this section and then comment out the for loop above this region to change pathfinding from square based to diagonal inclusive.
        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        if (x == 0 && y == 0)
        //            continue;

        //        int checkX = node.gridX + x;
        //        int checkY = node.gridY + y;

        //        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        //        {
        //            neighbours.Add(grid[checkX, checkY]);
        //        }
        //    }
        //}
        #endregion

        return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	public List<Node> path;
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
        if (onlyDisplayPathGizmos)
        {
            if (path != null)
            {
                foreach (Node n in path)
                {
					Gizmos.color = Color.black;
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));

				}
			}
            foreach (Node n in movement)
            {                 
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
        else
        {        
			if (grid != null) 
			{
				foreach (Node n in grid) 
				{
					Gizmos.color = (n.walkable)?Color.white:Color.red;
					if (path != null)
                    {
						if (path.Contains(n))
						{
							Gizmos.color = Color.black;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                    }
      //              if (movement.Contains(n))
      //              {
						//Gizmos.color = Color.green;
      //              }
      //              else
      //              {
						//continue;
      //              }
					Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
				}	
			}
		}
	}
}