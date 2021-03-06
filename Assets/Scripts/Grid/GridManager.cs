using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Variables
    Node[,,] grid;
    [SerializeField]
    float xzScale = 1.5f;

    [SerializeField]
    float yScale = 2;

    Vector3 minPos;

    int maxX;
    int maxZ;
    int maxY;

    public bool visualiseCollisions;
    public bool visualiseAllCollisions;

    List<Vector3> nodeViz = new List<Vector3>();
    List<Node> walkableViz = new List<Node>();
    public Vector3 extends = new Vector3(0.8f, 0.8f, 0.8f);

    int pos_x;
    int pos_y;
    int pos_z;
    #endregion

    public GameObject unit;
    public GameObject tileViz;

    GameObject tileContainer;

    public void Init()
    {
        tileContainer = new GameObject("tileContainer");

        ReadLevel();        
    }

    void ReadLevel()
    {
        GridPosition[] gp = GameObject.FindObjectsOfType<GridPosition>();

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = minX;
        float maxZ = maxX;
        float minY = minX;
        float maxY = maxX;

        for (int i = 0; i < gp.Length; i++)
        {
            Transform t = gp[i].transform;

            #region Read Positions
            if (t.position.x < minX)
            {
                minX = t.position.x;
            }

            if (t.position.x > maxX)
            {
                maxX = t.position.x;
            }

            if (t.position.z < minZ)
            {
                minZ = t.position.z;
            }

            if (t.position.z > maxZ)
            {
                maxZ = t.position.z;
            }

            if (t.position.y < minY)
            {
                minY = t.position.y;
            }

            if (t.position.y > maxY)
            {
                maxY = t.position.y;
            }

            #endregion

        }

        pos_x = Mathf.FloorToInt((maxX - minX) / xzScale);
        pos_z = Mathf.FloorToInt((maxZ - minZ) / xzScale);
        pos_y = Mathf.FloorToInt((maxY - minY) / yScale);

        if (pos_y == 0)
        {
            pos_y = 1;
        }

        minPos = Vector3.zero;
        minPos.x = minX;
        minPos.z = minZ;
        minPos.y = minY;

        CreateGrid(pos_x, pos_z, pos_y);
    }

    void CreateGrid(int pos_x, int pos_z, int pos_y)
    {
        grid = new Node[pos_x, pos_y, pos_z];

        for (int y = 0; y < pos_y; y++)
        {
            for (int x = 0; x < pos_x; x++)
            {
                for (int z = 0; z < pos_z; z++)
                {
                    Node n = new Node();
                    n.x = x;
                    n.z = z;
                    n.y = y;

                    Vector3 tp = minPos;
                    tp.x += x * xzScale + 0.5f;
                    tp.z += z * xzScale + 0.5f;
                    tp.y += y * yScale;

                    n.worldPosition = tp;

                    Collider[] overlapNode = Physics.OverlapBox(tp, extends/2, Quaternion.identity);

                    if (overlapNode.Length > 0)
                    {
                        bool isWalkable = false;
                        for (int i = 0; i < overlapNode.Length; i++)
                        {
                            GridObject obj = overlapNode[i].transform.GetComponentInChildren<GridObject>();
                            if (obj != null)
                            {
                                if (obj.isWalkable && n.obstacle == null)
                                {
                                    isWalkable = true;                                    
                                }
                                else
                                {
                                    isWalkable = false;
                                    n.obstacle = obj;
                                }

                            }
                        }
                        n.isWalkable = isWalkable;
                    }

                    if (n.isWalkable)
                    {
                        RaycastHit hit;
                        Vector3 origin = n.worldPosition;
                        origin.y += yScale - .1f;
                        if(Physics.Raycast(origin, Vector3.down, out hit, yScale - .1f))
                        {
                            n.worldPosition = hit.point;
                        }

                        GameObject go = Instantiate(tileViz, new Vector3(n.worldPosition.x, n.worldPosition.y + .1f, n.worldPosition.z), Quaternion.identity) as GameObject;
                        n.tileViz = go;
                        go.transform.parent = tileContainer.transform;
                        go.SetActive(true);
                    }
                    else
                    {
                        if(n.obstacle == null)
                        {
                            n.isAir = true;
                        }
                    }

                    if(n.obstacle != null)
                    {
                        nodeViz.Add(n.worldPosition);
                    }
                    else
                    {
                        walkableViz.Add(n);
                    }

                    grid[x, y, z] = n;
                }
            }
        }
    }

    public Node GetNode(Vector3 wp)
    {
        //removes any offsets
        Vector3 p = wp - minPos;
        int x = Mathf.RoundToInt(p.x / xzScale);
        int y = Mathf.RoundToInt(p.y / yScale);
        int z = Mathf.RoundToInt(p.z / xzScale);

        return GetNode(x, y, z);
    }

    public Node GetNode(int x, int y, int z)
    {
        if (x < 0 || x > pos_x - 1 || y < 0 || y > pos_y - 1 || z < 0 || z > pos_z -1)
        {
            return null;
        }
        return grid[x, y, z];
    }

    private void OnDrawGizmos()
    {
        if (visualiseCollisions)
        {
            if(visualiseAllCollisions)
            {
                foreach(Node n in grid)
                {
                    if(n.isWalkable)
                    {
                        Gizmos.color = Color.green;
                    }
                    else if(n.obstacle)
                    {
                        Gizmos.color = Color.red;
                    }
                    else if(n.isAir)
                    {
                        Gizmos.color = new Color(255f, 255f, 255f, 5f);
                    }
                    Gizmos.DrawWireCube(n.worldPosition, extends);
                }
                return;
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < nodeViz.Count; i++)
            {
                Gizmos.DrawWireCube(nodeViz[i], extends);
            }
            Gizmos.color = Color.green;
            foreach (Node n in walkableViz)
            {
                if (n.isWalkable)
                {
                    Gizmos.DrawWireCube(n.worldPosition, extends);
                }
            }
        }


        
    }
}
