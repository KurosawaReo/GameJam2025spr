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

    public enum PlyAction
    { 
        TEST01,
        TEST02,
        TEST03,

        NONE,   //操作なし.
    }

    /// <summary>
    /// グローバル定数.
    /// </summary>
    public static class Gl_Const
    {
        public const  int    BOARD_CELL_SIZE = 100;     // 盤面の1マスのサイズ.
        public const  float  BOARD_DIS_ADD_X = 0.55f;   // 盤面にobjを配置した時のずらす量x.
        public const  float  BOARD_DIS_ADD_Y = 0.72f;   // 盤面にobjを配置した時のずらす量y.

        public const  int    ENTITY_ADDRES_NUM = 2;     // エンティティの現在地管理用配列のサイズ.

        public const string IMAGES_PATH    = "Images/";     // Imagesフォルダのパス
        public const string RESOURCES_PATH = "Resources/";
        public const string GRID_JSON_PATH = "Assets/" + RESOURCES_PATH + "gridData.json"; // gridDataのパス
        public static string[] TILE_TYPE_IMAGES_PATH =      // タイルタイプ別の画像のパス
        {
            IMAGES_PATH + "Empty",          // 空.
            IMAGES_PATH + "Obstacle",       // 障害物.
            IMAGES_PATH + "RideObstacle",   // 障害物.
            IMAGES_PATH + "Wall",           // 壁.
            IMAGES_PATH + "Treasure",       // 宝.
            IMAGES_PATH + "EnemySpawn",     // 敵の生成ポイント.
        };

        public const float ENEMY_DEFAULT_RECAST_TIME = 5;   // デフォルトのリキャストタイム
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// ボード座標を元に配置.
        /// </summary>
        /// <param name="_obj">配置するオブジェクト</param>
        /// <param name="_bPos">ボード座標</param>
        public static void ObjPlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            Vector2 bPos;

            //位置調整.
            bPos.x = (_x-4) * Gl_Const.BOARD_CELL_SIZE;
            bPos.y = (_y-4) * Gl_Const.BOARD_CELL_SIZE;
            //"Canvas"基準からワールド座標に戻す.
            Vector2 wPos = LPosToWPos(GameObject.Find("Canvas"), bPos);
            
            //配置.
            _obj.transform.position = wPos - new Vector2(Gl_Const.BOARD_DIS_ADD_X, Gl_Const.BOARD_DIS_ADD_Y);
        }

        /// <summary>
        /// ワールド座標をボード座標に変換.
        /// </summary>
        /// <param name="_wPos">ワールド座標</param>
        /// <returns>ボード座標</returns>
        public static (int x, int y) WPosToBPos(Vector2 _wPos)
        {
            _wPos += new Vector2(Gl_Const.BOARD_DIS_ADD_X, Gl_Const.BOARD_DIS_ADD_Y);
            
            //"Canvas"基準のローカル座標に変換.
            Vector2 lPos = WPosToLPos(GameObject.Find("Canvas"), _wPos);

            //グリッド上でどこに位置するかを計算.
            int bPosX = (int)Mathf.Round(lPos.x / Gl_Const.BOARD_CELL_SIZE);
            int bPosY = (int)Mathf.Round(lPos.y / Gl_Const.BOARD_CELL_SIZE);

            //左上のマスが(0, 0)となるようにして返す.
            return (bPosX+4, bPosY+4);
        }

        /// <summary>
        /// ローカル座標をワールド座標に変換.
        /// </summary>
        /// <param name="_obj">親オブジェクト</param>
        /// <param name="_lPos">ローカル座標</param>
        /// <returns>ワールド座標</returns>
        public static Vector2 LPosToWPos(GameObject _obj, Vector2 _lPos)
        {
            var wPos = _obj.transform.TransformPoint(_lPos);
            return wPos;
        }

        /// <summary>
        /// ワールド座標をローカル座標に変換.
        /// </summary>
        /// <param name="_obj">親オブジェクト</param>
        /// <param name="_wPos">ワールド座標</param>
        /// <returns>ローカル座標</returns>
        public static Vector2 WPosToLPos(GameObject _obj, Vector2 _wPos)
        {
            var lPos = _obj.transform.InverseTransformPoint(_wPos);
            return lPos;
        }

        /// <summary>
        /// マウス座標取得.
        /// </summary>
        public static Vector2 GetMousePos()
        {
            Vector2 mPos = Input.mousePosition;
            Vector2 wPos = Camera.main.ScreenToWorldPoint(mPos);

            return wPos;
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