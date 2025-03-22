/*
   - Gloval.cs -
   汎用関数など、よく使いそうなものはここにまとめる.
*/

using System;

namespace Gloval
{
    /// <summary>
    /// ゲームの難易度.
    /// </summary>
    public enum Difficulty
    { 
        EASY,
        NORMAL,
        HARD
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
        public const int TEST = 2000;
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
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public static class Gl_func
    {
        /// <summary>
        /// テスト関数.
        /// </summary>
        public static void Test()
        {

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