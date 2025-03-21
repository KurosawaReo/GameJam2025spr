/*
   - Gloval.cs -
   �ėp�֐��ȂǁA�悭�g�������Ȃ��̂͂����ɂ܂Ƃ߂�.
*/

using UnityEngine;

namespace Gloval
{
    /// <summary>
    /// �Q�[���̓�Փx.
    /// </summary>
    public enum Difficulty
    { 
        EASY,
        NORMAL,
        HARD,
    }
    /// <summary>
    /// �Q�[���̃t�F�[�Y.
    /// </summary>
    public enum Phase
    {
        PREPARA, //preparation = ���K.
        DEFENSE, //defense = �h��.
    }
    /// <summary>
    /// �n�`���.
    /// </summary>
    public enum TerrainType
    {
        NONE,       //�Ȃ�.
        WALL,       //��.
        OBSTACLES,  //��Q��.
        ENEMY_GATE, //�G�����.
        TREASURE,   //��.
    }


    /// <summary>
    /// �O���[�o���萔.
    /// </summary>
    public static class Gl_Const
    {
        //[�Ֆ�]�}�X��.
        public const int   BOARD_GRID_HEI  = 10;   //�c�}�X��.
        public const int   BOARD_GRID_WID  = 10;   //���}�X��.

        //[�Ֆ�]�ʒu����.
        public const float BOARD_LEFT_UP_X = -4f;  //�Ֆʂ̍����_x.
        public const float BOARD_LEFT_UP_Y = 4f;   //�Ֆʂ̍����_y.
        public const float BOARD_GRID_SIZE = 0.8f; //�O���b�h�̃T�C�Y�䗦.
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// �{�[�h���W������unity��ɔz�u����.
        /// </summary>
        /// <param name="_obj">�z�u����I�u�W�F�N�g</param>
        /// <param name="_x">�Ֆʍ��W��x</param>
        /// <param name="_y">�Ֆʍ��W��y</param>
        public static void ObjectSetBoard(GameObject _obj, int _x, int _y)
        {
            //�O���b�h�̃T�C�Y.
            float size = Gl_Const.BOARD_GRID_SIZE;
            //��_+���炷��.
            float setX = Gl_Const.BOARD_LEFT_UP_X + _x * size;
            float setY = Gl_Const.BOARD_LEFT_UP_Y - _y * size;
            //�z�u.
            _obj.transform.localPosition = new Vector2(setX, setY);
            _obj.transform.localScale    = new Vector2(size, size);
        }
    }
}