using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarNavigator : MonoBehaviour
{
    public static AStarNavigator instance { get; private set; }


    [SerializeField] Tilemap worldTileMap;
    [SerializeField] Tilemap hazardTileMap;

    //Vector3Int playerGridPos;
    //List<Vector2Int> currentEnemyPath = new List<Vector2Int>();

    AStar aStar;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        aStar = new AStar();

        LoadAStarNodes(worldTileMap);
        ToggleAStarNodes(hazardTileMap, false);
    }


    void LoadAStarNodes(Tilemap tMap)
    {
        tMap.CompressBounds(); // Compress in case tilemap bounds got messed up

        for(int x = tMap.cellBounds.xMin; x < tMap.cellBounds.xMax; x++)
        {
            for(int y = tMap.cellBounds.yMin; y < tMap.cellBounds.yMax; y++)
            {
                if(tMap.GetTile(new Vector3Int(x, y, 0)))
                {
                    aStar.SetUpNode(new Vector2Int(x, y));
                }
            }
        }
    }


    void ToggleAStarNodes(Tilemap tMap, bool nodesEnabled)
    {
         tMap.CompressBounds(); // Compress in case tilemap bounds got messed up

        for(int x = tMap.cellBounds.xMin; x < tMap.cellBounds.xMax; x++)
        {
            for(int y = tMap.cellBounds.yMin; y < tMap.cellBounds.yMax; y++)
            {
                if(tMap.GetTile(new Vector3Int(x, y, 0)))
                {
                    aStar.TogglePointEnabled(new Vector2Int(x, y), nodesEnabled);

                    // Connected nodes horizontal / vertical
                    aStar.TogglePointEnabled(new Vector2Int(x - 1, y), nodesEnabled);
                    aStar.TogglePointEnabled(new Vector2Int(x + 1, y), nodesEnabled);
                    aStar.TogglePointEnabled(new Vector2Int(x, y - 1), nodesEnabled);
                    aStar.TogglePointEnabled(new Vector2Int(x, y + 1), nodesEnabled);

                    // Connected nodes diagonal
                    aStar.TogglePointEnabled(new Vector2Int(x - 1, y - 1), nodesEnabled);
                    aStar.TogglePointEnabled(new Vector2Int(x + 1, y + 1), nodesEnabled);
                    aStar.TogglePointEnabled(new Vector2Int(x + 1, y - 1), nodesEnabled);
                    aStar.TogglePointEnabled(new Vector2Int(x - 1, y + 1), nodesEnabled);
                }
            }
        }
    }


    public void LoadPath(Vector3 startWorldPos, Vector3 targetWorldPos, ref List<Vector2> path)
    {
        List<Vector2Int> newCellPath;

        Vector3Int newTargetGridPos = WorldToCell(targetWorldPos);

        Node endNode = aStar.GetNode((Vector2Int) newTargetGridPos);
        bool endNodeEnabled = endNode.enabled;
        aStar.TogglePointEnabled(endNode.boardPosition, true);

        if(path.Count == 0)
        {
            Vector3Int startGridPos = WorldToCell(startWorldPos);
            newCellPath = aStar.GetPathBetweenPoints((Vector2Int) startGridPos, (Vector2Int) newTargetGridPos);
        }
        else
        {
            newCellPath = aStar.GetPathBetweenPoints((Vector2Int) WorldToCell(new Vector3(path[0].x, path[0].y, 0)), (Vector2Int) newTargetGridPos);
        }

        path = new List<Vector2>(newCellPath.Count);
        for(int i = 0; i < newCellPath.Count; i++)
        {
            Vector3Int point = new Vector3Int(newCellPath[i].x, newCellPath[i].y, 0);
            Vector3 worldPoint = worldTileMap.CellToWorld(point);
            path.Add(new Vector2(worldPoint.x, worldPoint.y));
        }

        aStar.TogglePointEnabled(endNode.boardPosition, endNodeEnabled);
    }


    public Vector3Int WorldToCell(Vector3 pos)
    {
        Vector3 worldPosFloor = new Vector3(Mathf.Floor(pos.x), Mathf.Floor(pos.y), Mathf.Floor(pos.z));
        return worldTileMap.WorldToCell(worldPosFloor);
    }


    void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
