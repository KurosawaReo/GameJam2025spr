using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyPathfinding : MonoBehaviour
{
    public GridManager gridManager;
    private List<Vector2Int> path = new List<Vector2Int>();

    public void FindPath(Vector2Int start, Vector2Int goal)
    {
        gridManager = FindObjectOfType<GridManager>();
        path = AStarSearch(start, goal);
    }

    private List<Vector2Int> AStarSearch(Vector2Int start, Vector2Int goal)
    {
        var openSet = new SortedList<float, Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float>();
        var fScore = new Dictionary<Vector2Int, float>();

        openSet.Add(0, start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Values[0];
            openSet.RemoveAt(0);

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector2Int neighbor in gridManager.GetNeighbors(current))
            {
                if (gridManager.IsObstacle(neighbor)) continue;

                float tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                    if (!openSet.ContainsValue(neighbor))
                        openSet.Add(fScore[neighbor], neighbor);
                }
            }
        }
        return new List<Vector2Int>();
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    public List<Vector2Int> GetPath()
    {
        return path;
    }
}
