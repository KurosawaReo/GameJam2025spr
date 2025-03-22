#if false
/*
    元々使ってた定数.

    //マス数.
    public const int   BOARD_GRID_HEI  = 10;   //縦マス数.
    public const int   BOARD_GRID_WID  = 10;   //横マス数.

    //位置調整.
    public const float BOARD_LEFT_UP_X = -4f;  //盤面の左上基準点x.
    public const float BOARD_LEFT_UP_Y = 4f;   //盤面の左上基準点y.
    public const float BOARD_GRID_SIZE = 0.8f; //グリッドのサイズ比率.
*/

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
        get => m_id;
        set => m_id = value;
    }
}

/// <summary>
/// 盤面のデータ.
/// </summary>
public class BoardData
{
    //private変数.
    private TileType m_terrain;
    private Piece m_plyPiece;
    private Piece m_enmPiece;

    //初期化(コンストラクタ)
    public BoardData(TileType _terrain)
    {
        m_terrain = _terrain;
    }

    //get, set.
    public TileType terrain 
    {
        get => m_terrain;
        set => m_terrain = value; 
    }
    public Piece plyPiece
    {
        get => m_plyPiece;
        set => m_plyPiece = value;
    }
    public Piece enmPiece
    {
        get => m_enmPiece;
        set => m_enmPiece = value;
    }
}

/// <summary>
/// Boardに使うprefabまとめ.
/// </summary>
[Serializable]
public class BoardPrefab
{
    //private変数.
    [SerializeField] GameObject m_ground;
    [SerializeField] GameObject m_wall;
    [SerializeField] GameObject m_obstacle;
    [SerializeField] GameObject m_enemySpawn;
    [SerializeField] GameObject m_treasure;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject ground 
    { 
        get => m_ground;
        set => m_ground = value; 
    }
    public GameObject wall
    {
        get => m_wall;
        set => m_wall = value;
    }
    public GameObject obstacle
    {
        get => m_obstacle;
        set => m_obstacle = value;
    }
    public GameObject enemySpawn
    {
        get => m_enemySpawn;
        set => m_enemySpawn = value;
    }
    public GameObject treasure
    {
        get => m_treasure;
        set => m_treasure = value;
    }
    public GameObject inObj
    {
        get => m_inObj;
        set => m_inObj = value;
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
                board[x, y] = new BoardData(TileType.EMPTY);

                //周り一周は壁にする.
                if (x == 0 || x == Gl_Const.BOARD_GRID_WID-1 ||
                    y == 0 || y == Gl_Const.BOARD_GRID_HEI-1 )
                {
                    board[x, y].terrain = TileType.WALL;
                }
            }
        }

        //test.
        board[1, 1].terrain = TileType.OBSTACLE;
        board[2, 1].terrain = TileType.ENEMY_SPAWN;
        board[3, 1].terrain = TileType.TREASURE;
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
                    case TileType.EMPTY:
                        break;
                    case TileType.WALL:
                        obj = Instantiate(prfb.wall, prfb.inObj.transform);
                        break;
                    case TileType.OBSTACLE:
                        obj = Instantiate(prfb.obstacle, prfb.inObj.transform);
                        break;
                    case TileType.ENEMY_SPAWN:
                        obj = Instantiate(prfb.enemySpawn, prfb.inObj.transform);
                        break;
                    case TileType.TREASURE:
                        obj = Instantiate(prfb.treasure, prfb.inObj.transform);
                        break;

                    default:
                        break;
                }

                //配置.
                if (obj != null)
                {
                    Gl_Func.PlaceInBPos(obj, x+debugX, y+debugY);
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
#endif