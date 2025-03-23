using Gloval;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("HP残量")]
    int hp;

    [Tooltip("攻撃力")]
    int atk;

    [Tooltip("現在どのマスにいるのかをセット")]
    public Vector2Int currentPos;
    
    [Tooltip("目標となるマスor駒の位置をセット")]
    Vector2Int targetPos;
    
    [Tooltip("グリッドマネージャーをセット")]
    GridManager gridManager;

    [Tooltip("敵をステートを管理する用")]
    public EntityState currentState = EntityState.STOP;

    [Tooltip("敵のScriptableObjectをセット")]
    public EntityBase enemyData;

    [Tooltip("攻撃の対象")]
    GameObject target;

    [Tooltip("1度しか処理しないところで使うフラグ")]
    bool isFirst;

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (currentState)
        {
            case EntityState.STOP:
                StopState();
                break;

            case EntityState.MOVE:
                MoveState();
                break;
            
            case EntityState.ATTACK:
                AttackState();
                break;
            
            case EntityState.DEATH:
                DeathState();
                break;
            
            case EntityState.NONE:
                break;
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Init()
    {
        isFirst = true;

        // HPと攻撃力をデータで初期化する
        hp  = enemyData.hp;
        atk = enemyData.atk;

        // グリッドマネージャーを取得
        gridManager = FindObjectOfType<GridManager>();

        // 現在の位置をセルサイズで割ってどのマスにいるのかを特定する
        currentPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / Gl_Const.BOARD_CELL_SIZE),
                                    Mathf.RoundToInt(transform.localPosition.y / Gl_Const.BOARD_CELL_SIZE));

        // 敵の位置を保持させる
        gridManager.activeEnemyList.Add(currentPos);

        // 宝の位置を取得
        targetPos = gridManager.FindTreasurePosition();

        // 移動処理
        StartCoroutine(MoveToTarget());
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// </summary>
    /// <param name="atk">攻撃した側の攻撃力</param>
    public void Damage(int atk)
    {
        hp -= atk;

        hp = 0;
        var index = 0;

        for (var i = 0; i < gridManager.activeEnemyObjList.Count; i++)
        {
            if (gridManager.activeEnemyObjList[i] == gameObject)
            {
                // 自分自身の場合
                index = i;
                break;
            }
        }

        // 取得したインデックスの要素を削除する
        gridManager.activeEnemyList.RemoveAt(index);
        gridManager.activeEnemyObjList.RemoveAt(index);

        Destroy(gameObject); // 自信を破棄する
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
            if (!gridManager.IsInsideGrid(currentPos) || !gridManager.IsInsideGrid(targetPos))
            {
                Debug.LogError("無効な位置です！");
                yield break; // コルーチンを早期に終了
            }

            var path = AStarPathfinding(currentPos, targetPos);

            // 状態異常の場合は少し処理をスキップ

            if (path.Count > 1)
            {
                var isEncount = false; // エンカウントしたかどうか
                var nextPos = path[1]; // 次の移動先を取得
                
                //print($"現在の位置：{currentPos}、移動先の位置：{nextPos}");

                // 次の移動先に駒がいるか確認する

                var count = 0;
                foreach (var piece in gridManager.activePieceList)
                {
                    if (piece == nextPos)
                    {
                        // 次の移動先に駒がいた場合処理をスキップする
                        print("エンカウントしました");
                        isEncount = true; // エンカウントしたのでtrueにする
                        target = gridManager.activePieceObjList[count];
                        break;
                    }

                    count++;
                }

                if (isEncount)
                {
                    //print("エンカウントしました");
                    currentState = EntityState.ATTACK; // エンカウントしているので攻撃させる
                    //break; // エンカウントしていたら処理をしない
                }
                else
                {
                    yield return MoveTo(nextPos);                   // 移動処理を待って後続の処理を続行する
                    currentPos = nextPos;                           // 移動するので移動先の位置で上書きする

                    // 現在の位置をセルサイズで割ってどのマスにいるのかを特定する
                    currentPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / Gl_Const.BOARD_CELL_SIZE),
                                                Mathf.RoundToInt(transform.localPosition.y / Gl_Const.BOARD_CELL_SIZE));

                    //print($"currentPos:{currentPos}");
                    //gridManager.activeEnemyList[count] = currentPos;    // 敵の位置を保持させる
                }
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
        var end = new Vector3(nextPos.x * Gl_Const.BOARD_CELL_SIZE, nextPos.y * Gl_Const.BOARD_CELL_SIZE, 0);
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
    List<Vector2Int> AStarPathfinding(Vector2Int start, Vector2Int goal)
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

    float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // マンハッタン距離
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
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

    /// <summary>
    /// ステートがStopの時の処理
    /// </summary>
    void StopState()
    {
    }

    /// <summary>
    /// ステートがMoveの時の処理
    /// </summary>
    void MoveState()
    {
    }

    /// <summary>
    /// ステートがAttackの時の処理
    /// </summary>
    void AttackState()
    {
        // 1度しか通らないようにする
        if (isFirst)
        {
            //print("アタックステート内");
            // 攻撃処理を呼ぶ
            StartCoroutine(Attack());
            
            isFirst = false;
        }
    }

    /// <summary>
    /// ステートがDeathの時の処理
    /// </summary>
    void DeathState()
    {
        // 死亡処理
    }

    /// <summary>
    /// ステートを変更する時の処理
    /// </summary>
    public void ChangeState(EntityState newState)
    {
        // 変更後のステートが変更前と同じではないか確認する
        if (currentState == newState)
        {
            return;
        }

        print($"State changed: {currentState} → {newState}");
        currentState = newState;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // 少しディレイさせたいので少し処理を待つ
        yield return Gl_Func.Delay(1f);

        // 実際に攻撃する
        //print("攻撃");
        if (target != null)
        {
            target.GetComponent<Piece>().Damage(atk);
        }

        isFirst = true; // 攻撃のステートで使うのでフラグを戻しておく
        currentState = EntityState.MOVE; // ステートをMoveに変更
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