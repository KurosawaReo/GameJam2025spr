using Gloval;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyPathfinding pathfinding;
    private int currentPathIndex = 0;
    private List<Vector2Int> path = new List<Vector2Int>();

    public float moveSpeed = 2.0f;  // 敵の移動速度
    private RectTransform rectTransform;  // UI要素としてのRectTransform

    public Vector2Int treasurePosition;  // 宝の位置
    public GridManager gridManager;  // GridManager参照

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // RectTransformの取得
        pathfinding = GetComponent<EnemyPathfinding>();
        gridManager = GameObject.FindObjectOfType<GridManager>();

        // 敵の生成位置をスタート位置として設定
        var startPosition = new Vector2Int((int)transform.localPosition.x / Gl_Const.CELL_SIZE, (int)transform.localPosition.y / Gl_Const.CELL_SIZE);

        // ゴール位置を盤面から取得
        var goalPosition = gridManager.FindTreasurePosition();

        if (goalPosition != Vector2Int.zero)
        {
            // ルート探索
            pathfinding.FindPath(startPosition, goalPosition);
            path = pathfinding.GetPath();

            print($"path.Count:{path.Count}");

            // 経路が見つかったか確認
            if (path.Count == 0)
            {
                Debug.LogError("経路が見つかりませんでした！スタート地点: " + startPosition + " ゴール地点: " + goalPosition);
            }
            else
            {
                print("経路が見つかりました！経路数: " + path.Count);
                // 最初の位置に移動
                rectTransform.anchoredPosition = new Vector2(path[0].x, path[0].y);  // RectTransformの位置を設定
                currentPathIndex = 1;
                StartCoroutine(MoveAlongPath()); // コルーチンを開始
            }
        }
        else
        {
            Debug.LogError("ゴール位置が見つかりませんでした！");
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (currentPathIndex < path.Count)
        {
            var targetPosition = new Vector2(path[currentPathIndex].x, path[currentPathIndex].y);

            print($"targetPosition:{targetPosition}");

            // ターゲットに向かって移動
            while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0.1f)
            {
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;  // 1フレーム待機
            }

            currentPathIndex++;  // 次のポイントへ進む
        }
    }
}
