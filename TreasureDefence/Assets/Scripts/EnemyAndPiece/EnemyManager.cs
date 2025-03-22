using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("生きている敵のリスト")]
    List<Enemy> enemies = new List<Enemy>();

    [Tooltip("敵のプレハブをセット")]
    [SerializeField] GameObject enemyPrefab;

    [Tooltip("敵をまとめる親オブジェクトをセット")]
    [SerializeField] Transform enemyParent;

    [Tooltip("グリッドマネージャーをセット")]
    public GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Init();

        // 敵を生成
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Init()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    /// <summary>
    /// 敵の追加
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    /// <summary>
    /// 敵を描画
    /// </summary>
    void SpawnEnemies()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (gridManager.grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    var enemy = Instantiate(enemyPrefab, enemyParent);
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.CELL_SIZE, y * Gl_Const.CELL_SIZE);
                }
            }
        }
    }
}
