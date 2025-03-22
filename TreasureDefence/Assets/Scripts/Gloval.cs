/*
   - Gloval.cs -
   �ėp�֐��ȂǁA�悭�g�������Ȃ��̂͂����ɂ܂Ƃ߂�.
*/

using System;
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
    /// �^�C���̃^�C�v.
    /// </summary>
    public enum TileType
    {
        EMPTY,          // ��
        OBSTACLE,       // ��Q��
        RIDE_OBSTACLE,  // �����Q��
        WALL,           // ��
        TREASURE,       // ��
        ENEMY_SPAWN     // �G�̐����|�C���g
    }

    /// <summary>
    /// �O���[�o���萔.
    /// </summary>
    public static class Gl_Const
    {
        public const int ENTITY_ADDRES_NUM = 2;     // �G���e�B�e�B�̌��ݒn�Ǘ��p�z��̃T�C�Y
        public const int CELL_SIZE = 100;           // 1�}�X�̑傫��

        public const string IMAGES_PATH = "Images/";    // Images�t�H���_�̃p�X
        public const string RESOURCES_PATH = "Resources/";
        public const string GRID_JSON_PATH = "Assets/" + RESOURCES_PATH + "gridData.json";    // gridData�̃p�X
        public static string[] TILE_TYPE_IMAGES_PATH =  // �^�C���^�C�v�ʂ̉摜�̃p�X
        {
            IMAGES_PATH + "Empty",          // ��
            IMAGES_PATH + "Obstacle",       // ��Q��
            IMAGES_PATH + "RideObstacle",   // ��Q��
            IMAGES_PATH + "Wall",           // ��
            IMAGES_PATH + "Treasure",       // ��
            IMAGES_PATH + "EnemySpawn",     // �G�̐����|�C���g
        };

        //[�Ֆ�]�}�X��.
        public const int   BOARD_GRID_HEI  = 10;   //�c�}�X��.
        public const int   BOARD_GRID_WID  = 10;   //���}�X��.

        //[�Ֆ�]�ʒu����.
        public const float BOARD_LEFT_UP_X = -4f;  //�Ֆʂ̍����_x.
        public const float BOARD_LEFT_UP_Y = 4f;   //�Ֆʂ̍����_y.
        public const float BOARD_GRID_SIZE = 0.8f; //�O���b�h�̃T�C�Y�䗦.

        public const float ENEMY_DEFAULT_RECAST_TIME = 10;   // �f�t�H���g�̓G�����̃��L���X�g�^�C��
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

    /// <summary>
    /// 1�}�X�̏��.
    /// </summary>
    [Serializable]
    public class GridCell
    {
        public TileType     tileType;     // �^�C���^�C�v
        public bool         isOccupied;   // ��u���Ă��邩�ǂ���
        public EntityBase   entity;       // �G���e�B�e�B�̎��̂��Z�b�g

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="_type">�^�C���^�C�v</param>
        public GridCell(TileType _type)
        {
            tileType    = _type;
            isOccupied  = false;
            entity      = null;
        }
    }
}