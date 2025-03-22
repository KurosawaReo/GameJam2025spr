using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gloval;

/// <summary>
/// プレイヤーや敵の駒データ.
/// </summary>
public class Piece
{
    //private変数.
    private string m_id;

    //get, set.
    public string id
    {
        get { return m_id; }
        set { m_id = value; }
    }
}

/// <summary>
/// 盤面のデータ.
/// </summary>
public class BoardData
{
    //private変数.
    private TerrainType m_terrain;
    private Piece m_plyPiece;
    private Piece m_enmPiece;

    //初期化(コンストラクタ)
    public BoardData(TerrainType _terrain)
    {
        m_terrain = _terrain;
    }

    //get, set.
    public TerrainType terrain 
    {
        get { return m_terrain; }
        set { m_terrain = value; } 
    }
    public Piece plyPiece
    {
        get { return m_plyPiece; }
        set { m_plyPiece = value; }
    }
    public Piece enmPiece
    {
        get { return m_enmPiece; }
        set { m_enmPiece = value; }
    }
}

/// <summary>
/// Boardに使うprefabまとめ.
/// </summary>
[Serializable]
public class BoardPrefab
{
    //private変数.
    [SerializeField] private GameObject m_ground;
    [SerializeField] private GameObject m_wall;
    [SerializeField] private GameObject m_obstacles;
    [SerializeField] private GameObject m_enemyGate;
    [SerializeField] private GameObject m_treasure;
    [Space]
    [SerializeField] private GameObject m_inObj;

    //get, set.
    public GameObject ground 
    { 
        get { return m_ground; }
        set { m_ground = value; } 
    }
    public GameObject wall
    {
        get { return m_wall; }
        set { m_wall = value; }
    }
    public GameObject obstacles
    {
        get { return m_obstacles; }
        set { m_obstacles = value; }
    }
    public GameObject enemyGate
    {
        get { return m_enemyGate; }
        set { m_enemyGate = value; }
    }
    public GameObject treasure
    {
        get { return m_treasure; }
        set { m_treasure = value; }
    }
    public GameObject inObj
    {
        get { return m_inObj; }
        set { m_inObj = value; }
    }
}

/// <summary>
/// 盤面管理プログラム.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] BoardPrefab prfb;

    [Header("- value -")]
    [SerializeField] int debugX;
    [SerializeField] int debugY;

    BoardData[,] board; //盤面データを入れる2次元配列.

    void Start()
    {
        InitBoard();
        //GenerateBoard();
    }
    void Update()
    {
        GenerateBoard();
    }

    /// <summary>
    /// 盤面の初期化.
    /// </summary>
    private void InitBoard()
    {
        //マス数を決める.
        board = new BoardData[Gl_Const.BOARD_GRID_WID, Gl_Const.BOARD_GRID_HEI];

        //全マスループ.
        for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++) {

                //マスの初期化.
                board[x, y] = new BoardData(TerrainType.NONE);

                //周り一周は壁にする.
                if (x == 0 || x == Gl_Const.BOARD_GRID_WID-1 ||
                    y == 0 || y == Gl_Const.BOARD_GRID_HEI-1 )
                {
                    board[x, y].terrain = TerrainType.WALL;
                }
            }
        }

        //test.
        board[1, 1].terrain = TerrainType.OBSTACLES;
        board[2, 1].terrain = TerrainType.ENEMY_GATE;
        board[3, 1].terrain = TerrainType.TREASURE;
    }

    /// <summary>
    /// 盤面の生成.
    /// </summary>
    private void GenerateBoard()
    {
        EraseAllTerrain(); //再生成するため一旦消去.

        //全マスループ.
        for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++) {

                GameObject obj = null;

                //地形別.
                switch (board[x, y].terrain)
                {
                    case TerrainType.NONE:
                        break;
                    case TerrainType.WALL:
                        obj = Instantiate(prfb.wall, prfb.inObj.transform);
                        break;
                    case TerrainType.OBSTACLES:
                        obj = Instantiate(prfb.obstacles, prfb.inObj.transform);
                        break;
                    case TerrainType.ENEMY_GATE:
                        obj = Instantiate(prfb.enemyGate, prfb.inObj.transform);
                        break;
                    case TerrainType.TREASURE:
                        obj = Instantiate(prfb.treasure, prfb.inObj.transform);
                        break;

                    default:
                        break;
                }

                //配置.
                if (obj != null)
                {
                    Gl_Func.ObjectSetBoard(obj, x+debugX, y+debugY);
                    //Gl_Func.ObjectSetBoard(obj, x, y);
                }
            }
        }
    }

    /// <summary>
    /// 盤面のTerrainの全消去.
    /// </summary>
    private void EraseAllTerrain()
    {
        /*
           childCountで子objの数を取得可能.

           ただし、Destroy()で子objを消去してもすぐには反映されず
           一度処理が終わるまで存在は残り続けるっぽい.
        */

        //子objの数だけ消去する.
        for (int i = 0; i < prfb.inObj.transform.childCount; i++){
            //取得.
            var obj = prfb.inObj.transform.GetChild(i);
            if (obj != null)
            {
                Destroy(obj.gameObject); //消去.
            }
        }
    }
}
