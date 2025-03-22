/*
   - Gloval.cs -
   汎用関数など、よく使いそうなものはここにまとめる.
*/

using System;
using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// ゲームの難易度.
    /// </summary>
    public enum Difficulty
    { 
        EASY,
        NORMAL,
        HARD,
    }
    /// <summary>
    /// ゲームのフェーズ.
    /// </summary>
    public enum Phase
    {
        PREPARA, //preparation = 練習.
        DEFENSE, //defense = 防御.
    }
    /// <summary>
    /// 地形や宝.
    /// </summary>
    public enum TerrainType
    {
        NONE,       //なし.
        WALL,       //壁.
        OBSTACLES,  //障害物.
        ENEMY_GATE, //敵入り口.
        TREASURE,   //宝.
    }


    /// <summary>
    /// タイルのタイプ.
    /// </summary>
    public enum TileType
    {
        EMPTY,          // 空
        OBSTACLE,       // 障害物
        RIDE_OBSTACLE,  // 乗れる障害物
        WALL,           // 壁
        TREASURE,       // 宝
        ENEMY_SPAWN     // 敵の生成ポイント
    }

    /// <summary>
    /// グローバル定数.
    /// </summary>
    public static class Gl_Const
    {
        public const int ENTITY_ADDRES_NUM = 2;     // エンティティの現在地管理用配列のサイズ
        public const int CELL_SIZE = 100;           // 1マスの大きさ

        public const string IMAGES_PATH = "Images/";    // Imagesフォルダのパス
        public const string RESOURCES_PATH = "Resources/";
        public const string GRID_JSON_PATH = "Assets/" + RESOURCES_PATH + "gridData.json";    // gridDataのパス
        public static string[] TILE_TYPE_IMAGES_PATH =  // タイルタイプ別の画像のパス
        {
            IMAGES_PATH + "Empty",          // 空
            IMAGES_PATH + "Obstacle",       // 障害物
            IMAGES_PATH + "RideObstacle",   // 障害物
            IMAGES_PATH + "Wall",           // 壁
            IMAGES_PATH + "Treasure",       // 宝
            IMAGES_PATH + "EnemySpawn",     // 敵の生成ポイント
        };

        //[盤面]マス数.
        public const int   BOARD_GRID_HEI  = 10;   //縦マス数.
        public const int   BOARD_GRID_WID  = 10;   //横マス数.

        //[盤面]位置調整.
        public const float BOARD_LEFT_UP_X = -4f;  //盤面の左上基準点x.
        public const float BOARD_LEFT_UP_Y = 4f;   //盤面の左上基準点y.
        public const float BOARD_GRID_SIZE = 0.8f; //グリッドのサイズ比率.

        public const float ENEMY_DEFAULT_RECAST_TIME = 10;   // デフォルトの敵生成のリキャストタイム
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// ボード座標を元にunity上に配置する.
        /// </summary>
        /// <param name="_obj">配置するオブジェクト</param>
        /// <param name="_x">盤面座標のx</param>
        /// <param name="_y">盤面座標のy</param>
        public static void ObjectSetBoard(GameObject _obj, int _x, int _y)
        {
            //グリッドのサイズ.
            float size = Gl_Const.BOARD_GRID_SIZE;
            //基準点+ずらす量.
            float setX = Gl_Const.BOARD_LEFT_UP_X + _x * size;
            float setY = Gl_Const.BOARD_LEFT_UP_Y - _y * size;
            //配置.
            _obj.transform.localPosition = new Vector2(setX, setY);
            _obj.transform.localScale    = new Vector2(size, size);
        }
    }

    /// <summary>
    /// 1マスの情報.
    /// </summary>
    [Serializable]
    public class GridCell
    {
        public TileType     tileType;     // タイルタイプ
        public bool         isOccupied;   // 駒が置いてあるかどうか
        public EntityBase   entity;       // エンティティの実体をセット

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_type">タイルタイプ</param>
        public GridCell(TileType _type)
        {
            tileType    = _type;
            isOccupied  = false;
            entity      = null;
        }
    }
}