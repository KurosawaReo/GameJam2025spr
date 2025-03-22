using Gloval;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("現在どのマスにいるのかをセット")]
    Vector2Int currentPos;
    
    [Tooltip("目標となるマスor駒の位置をセット")]
    Vector2Int targetPos;
    
    [Tooltip("グリッドマネージャーをセット")]
    GridManager gridManager;

    void Start()
    {
        // グリッドマネージャーを取得
        gridManager = FindObjectOfType<GridManager>();

        // 現在の位置をセルサイズで割ってどのマスにいるのかを特定する
        currentPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / Gl_Const.CELL_SIZE),
                                    Mathf.RoundToInt(transform.localPosition.y / Gl_Const.CELL_SIZE));
        
        // 宝の位置を取得
        targetPos = gridManager.FindTreasurePosition();
        
        // 移動処理
        StartCoroutine(MoveToTarget());
    }

    /// <summary>
    /// 目標に向かって移動
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToTarget()
    {
        // 目標に到達するまで待機
        while (currentPos != targetPos)
        {
            if (!gridManager.IsPositionValid(currentPos) || !gridManager.IsPositionValid(targetPos))
            {
                Debug.LogError("無効な位置です！");
                yield break; // コルーチンを早期に終了
            }

            var path = AStarPathfinding(currentPos, targetPos);

            // 状態異常の場合は少し処理をスキップ

            if (path.Count > 1)
            {
                Vector2Int nextPos = path[1]; // 次の移動先を取得
                yield return MoveTo(nextPos);
                currentPos = nextPos;
            }
            else
            {
                break; // ルートが見つからない場合は停止
            }

            yield return null;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="nextPos">次の移動先</param>
    /// <returns></returns>
    IEnumerator MoveTo(Vector2Int nextPos)
    {
        var start = transform.localPosition;
        var end = new Vector3(nextPos.x * Gl_Const.CELL_SIZE, nextPos.y * Gl_Const.CELL_SIZE, 0);
        var duration = 0.2f;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime / 10;
            yield return null;
        }

        transform.localPosition = end;
    }

    /// <summary>
    /// ルート探索部分
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private List<Vector2Int> AStarPathfinding(Vector2Int start, Vector2Int goal)
    {
        // ここにA*アルゴリズムの実装（グリッドを元に最適ルートを計算）
        var path = new List<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float>();
        var fScore = new Dictionary<Vector2Int, float>();
        var openSet = new PriorityQueue<Vector2Int>();

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector2Int neighbor in gridManager.GetNeighbors(current))
            {
                float tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }
        return path; // ルートが見つからない場合
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // マンハッタン距離
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}

public class PriorityQueue<T>
{
    List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
        elements = elements.OrderBy(e => e.Value).ToList();
    }

    public T Dequeue()
    {
        var bestItem = elements[0];
        elements.RemoveAt(0);
        return bestItem.Key;
    }

    public bool Contains(T item)
    {
        return elements.Any(e => EqualityComparer<T>.Default.Equals(e.Key, item));
    }
}