/*
   - Gloval.cs -
   汎用関数など、よく使いそうなものはここにまとめる.
*/

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
    /// グローバル定数.
    /// </summary>
    public static class Gl_Const
    {
        //[盤面]マス数.
        public const int   BOARD_GRID_HEI  = 10;   //縦マス数.
        public const int   BOARD_GRID_WID  = 10;   //横マス数.

        //[盤面]位置調整.
        public const float BOARD_LEFT_UP_X = -4f;  //盤面の左上基準点x.
        public const float BOARD_LEFT_UP_Y = 4f;   //盤面の左上基準点y.
        public const float BOARD_GRID_SIZE = 0.8f; //グリッドのサイズ比率.
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
}