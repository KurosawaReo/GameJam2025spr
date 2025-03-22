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

    public enum PlyAction
    { 
        TEST01,
        TEST02,
        TEST03,

        NONE,   //����Ȃ�.
    }

    /// <summary>
    /// �O���[�o���萔.
    /// </summary>
    public static class Gl_Const
    {
        public const  int    BOARD_CELL_SIZE = 100;     // �Ֆʂ�1�}�X�̃T�C�Y.
        public const  float  BOARD_DIS_ADD_X = 0.55f;   // �Ֆʂ�obj��z�u�������̂��炷��x.
        public const  float  BOARD_DIS_ADD_Y = 0.72f;   // �Ֆʂ�obj��z�u�������̂��炷��y.

        public const  int    ENTITY_ADDRES_NUM = 2;     // �G���e�B�e�B�̌��ݒn�Ǘ��p�z��̃T�C�Y.

        public const string IMAGES_PATH    = "Images/";     // Images�t�H���_�̃p�X
        public const string RESOURCES_PATH = "Resources/";
        public const string GRID_JSON_PATH = "Assets/" + RESOURCES_PATH + "gridData.json"; // gridData�̃p�X
        public static string[] TILE_TYPE_IMAGES_PATH =      // �^�C���^�C�v�ʂ̉摜�̃p�X
        {
            IMAGES_PATH + "Empty",          // ��.
            IMAGES_PATH + "Obstacle",       // ��Q��.
            IMAGES_PATH + "RideObstacle",   // ��Q��.
            IMAGES_PATH + "Wall",           // ��.
            IMAGES_PATH + "Treasure",       // ��.
            IMAGES_PATH + "EnemySpawn",     // �G�̐����|�C���g.
        };

        public const float ENEMY_DEFAULT_RECAST_TIME = 5;   // �f�t�H���g�̃��L���X�g�^�C��
    }

    /// <summary>
    /// �O���[�o���֐�.
    /// </summary>
    public static class Gl_Func
    {
        /// <summary>
        /// �{�[�h���W�����ɔz�u.
        /// </summary>
        /// <param name="_obj">�z�u����I�u�W�F�N�g</param>
        /// <param name="_bPos">�{�[�h���W</param>
        public static void ObjPlaceOnBoard(GameObject _obj, int _x, int _y)
        {
            Vector2 bPos;

            //�ʒu����.
            bPos.x = (_x-4) * Gl_Const.BOARD_CELL_SIZE;
            bPos.y = (_y-4) * Gl_Const.BOARD_CELL_SIZE;
            //"Canvas"����烏�[���h���W�ɖ߂�.
            Vector2 wPos = LPosToWPos(GameObject.Find("Canvas"), bPos);
            
            //�z�u.
            _obj.transform.position = wPos - new Vector2(Gl_Const.BOARD_DIS_ADD_X, Gl_Const.BOARD_DIS_ADD_Y);
        }

        /// <summary>
        /// ���[���h���W���{�[�h���W�ɕϊ�.
        /// </summary>
        /// <param name="_wPos">���[���h���W</param>
        /// <returns>�{�[�h���W</returns>
        public static (int x, int y) WPosToBPos(Vector2 _wPos)
        {
            _wPos += new Vector2(Gl_Const.BOARD_DIS_ADD_X, Gl_Const.BOARD_DIS_ADD_Y);
            
            //"Canvas"��̃��[�J�����W�ɕϊ�.
            Vector2 lPos = WPosToLPos(GameObject.Find("Canvas"), _wPos);

            //�O���b�h��łǂ��Ɉʒu���邩���v�Z.
            int bPosX = (int)Mathf.Round(lPos.x / Gl_Const.BOARD_CELL_SIZE);
            int bPosY = (int)Mathf.Round(lPos.y / Gl_Const.BOARD_CELL_SIZE);

            //����̃}�X��(0, 0)�ƂȂ�悤�ɂ��ĕԂ�.
            return (bPosX+4, bPosY+4);
        }

        /// <summary>
        /// ���[�J�����W�����[���h���W�ɕϊ�.
        /// </summary>
        /// <param name="_obj">�e�I�u�W�F�N�g</param>
        /// <param name="_lPos">���[�J�����W</param>
        /// <returns>���[���h���W</returns>
        public static Vector2 LPosToWPos(GameObject _obj, Vector2 _lPos)
        {
            var wPos = _obj.transform.TransformPoint(_lPos);
            return wPos;
        }

        /// <summary>
        /// ���[���h���W�����[�J�����W�ɕϊ�.
        /// </summary>
        /// <param name="_obj">�e�I�u�W�F�N�g</param>
        /// <param name="_wPos">���[���h���W</param>
        /// <returns>���[�J�����W</returns>
        public static Vector2 WPosToLPos(GameObject _obj, Vector2 _wPos)
        {
            var lPos = _obj.transform.InverseTransformPoint(_wPos);
            return lPos;
        }

        /// <summary>
        /// �}�E�X���W�擾.
        /// </summary>
        public static Vector2 GetMousePos()
        {
            Vector2 mPos = Input.mousePosition;
            Vector2 wPos = Camera.main.ScreenToWorldPoint(mPos);

            return wPos;
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