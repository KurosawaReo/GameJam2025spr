/*
   - Gloval.cs -
   �ėp�֐��ȂǁA�悭�g�������Ȃ��̂͂����ɂ܂Ƃ߂�.
*/

using System;

namespace Gloval
{
    /// <summary>
    /// �Q�[���̓�Փx.
    /// </summary>
    public enum Difficulty
    { 
        EASY,
        NORMAL,
        HARD
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
        public const int TEST = 2000;
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
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_func
    {
        /// <summary>
        /// �e�X�g�֐�.
        /// </summary>
        public static void Test()
        {

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