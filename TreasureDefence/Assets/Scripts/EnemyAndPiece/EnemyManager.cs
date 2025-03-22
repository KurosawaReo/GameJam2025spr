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

    [Tooltip("リキャストタイム")]
    float recastTime;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        Init();

        StartCoroutine(SpawnEnemies());
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
        recastTime = Gl_Const.ENEMY_DEFAULT_RECAST_TIME;
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
    IEnumerator SpawnEnemies()
    {
        // todo Enemyを抽選する
        // todo 抽選結果の敵のScrptableObjectを取得or使用して敵の画像を変更する
        // todo できれば各敵スポーンのところからランダムな時間で敵が出てくるようにする

        for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++)
        {
            for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++)
            {
                if (gridManager.grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    var enemy = Instantiate(enemyPrefab, enemyParent);
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.CELL_SIZE, y * Gl_Const.CELL_SIZE);
                    AddEnemy(enemy.GetComponent<Enemy>());
                }
            }
        }

        yield return new WaitForSeconds(recastTime);
        // 生成時間をだんだん短くする
        recastTime -= 0.01f;
        StartCoroutine(SpawnEnemies());
    }
}
