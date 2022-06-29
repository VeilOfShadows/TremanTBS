using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Variables
    Node[,] grid;
    [SerializeField]
    float xzScale = 1.5f;

    [SerializeField]
    float yScale = 2;

    Vector3 minPos;

    int maxX;
    int maxZ;
    int maxY;

    List<Vector3> nodeViz = new List<Vector3>();
    public Vector3 extends = new Vector3(0.8f, 0.8f, 0.8f);

    #endregion

    private void Start()
    {
        ReadLevel();
    }

    void ReadLevel()
    {
        GridPosition[] gp = GameObject.FindObjectsOfType<GridPosition>();

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = minX;
        float maxZ = maxX;

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

            #endregion
        }

        int pos_x = Mathf.FloorToInt((maxX - minX) / xzScale);
        int pos_z = Mathf.FloorToInt((maxZ - minZ) / xzScale);

        minPos = Vector3.zero;
        minPos.x = minX;
        minPos.z = minZ;

        CreateGrid(pos_x, pos_z);
    }

    void CreateGrid(int pos_x, int pos_z)
    {
        grid = new Node[pos_x, pos_z];
        for (int x = 0; x < pos_x; x++)
        {
            for (int z = 0; z < pos_z; z++)
            {
                Node n = new Node();
                n.x = x;
                n.z = z;

                Vector3 tp = minPos;
                tp.x += x * xzScale + 0.5f;
                tp.z += z * xzScale + 0.5f;

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
                    nodeViz.Add(n.worldPosition);
                }

                grid[x, z] = n;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < nodeViz.Count; i++)
        {
            Gizmos.DrawWireCube(nodeViz[i], extends);
        }

    }
}
