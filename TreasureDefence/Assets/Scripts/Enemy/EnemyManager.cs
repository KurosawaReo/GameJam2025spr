using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("生きている敵のリスト")]
    List<Enemy> enemies = new List<Enemy>();

    [Tooltip("敵のプレハブをセット")]
    [SerializeField] GameObject enemyPrefab;

    [Tooltip("敵をまとめる親オブジェクトをセット")]
    [SerializeField] Transform enemyParent;

    [Tooltip("敵のScriptableObjectをセット")]
    [SerializeField] List<EnemyData> enemyDatas = new List<EnemyData>();

    [Tooltip("グリッドマネージャーをセット")]
    public GridManager gridManager;

    [Tooltip("GameManagerをセット")]
    public GameManager gameManager;

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
        gameManager = FindObjectOfType<GameManager>();
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
    /// 敵を描画生成.
    /// </summary>
    IEnumerator SpawnEnemies()
    {
#if false
        //守備フェーズじゃなければ.
        if (gameManager.gameData.phase != Phase.DEFENSE)
        {
            yield return new WaitForSeconds(recastTime);
   
            StartCoroutine(SpawnEnemies());
        }
#endif
        //守備フェーズじゃない間は、処理をここで留める.
        while (gameManager.gameData.phase != Phase.DEFENSE)
        {
            yield return Gl_Func.Delay(recastTime);
        }

        //全マスループ.
        for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++) {
            for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++) {

                //敵の生成ポイントなら.
                if (gridManager.grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    // 確率で処理をスキップして敵を生成しないようにする
                    var rnd = Random.Range(0,100);

                    if (rnd < 30)
                    {
                        // 処理をスキップ
                        continue;
                    }

                    // 敵の生成ポイントを見つけた時
                    // 敵の種類から抽選をしてどの敵を出すのか決める
                    var enemyIndex = Random.Range(0, enemyDatas.Count);
                    //print($"敵の番号：{enemyIndex}");

                    // 敵生成
                    var enemy = Instantiate(enemyPrefab, enemyParent);

                    // EnemyにScriptableObjectをセット
                    enemy.GetComponent<Enemy>().enemyData = enemyDatas[enemyIndex];

                    // 初期座標を調整
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);

                    // 敵の画像を変更
                    enemy.GetComponent<Image>().sprite = enemyDatas[enemyIndex].sprite;

                    // リストに敵を追加
                    AddEnemy(enemy.GetComponent<Enemy>());
                }
            }
        }

        yield return Gl_Func.Delay(recastTime); //遅延.
        recastTime -= 0.01f;                    //遅延(生成時間)をだんだん短くする.
        
        StartCoroutine(SpawnEnemies());         //この関数をループ.
    }
}
