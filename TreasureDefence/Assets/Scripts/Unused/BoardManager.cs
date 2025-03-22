#if false
/*
    ���X�g���Ă��萔.

    //�}�X��.
    public const int   BOARD_GRID_HEI  = 10;   //�c�}�X��.
    public const int   BOARD_GRID_WID  = 10;   //���}�X��.

    //�ʒu����.
    public const float BOARD_LEFT_UP_X = -4f;  //�Ֆʂ̍����_x.
    public const float BOARD_LEFT_UP_Y = 4f;   //�Ֆʂ̍����_y.
    public const float BOARD_GRID_SIZE = 0.8f; //�O���b�h�̃T�C�Y�䗦.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gloval;

/// <summary>
/// �v���C���[��G�̋�f�[�^.
/// </summary>
public class Piece
{
    //private�ϐ�.
    private string m_id;

    //get, set.
    public string id
    {
        get => m_id;
        set => m_id = value;
    }
}

/// <summary>
/// �Ֆʂ̃f�[�^.
/// </summary>
public class BoardData
{
    //private�ϐ�.
    private TileType m_terrain;
    private Piece m_plyPiece;
    private Piece m_enmPiece;

    //������(�R���X�g���N�^)
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
/// Board�Ɏg��prefab�܂Ƃ�.
/// </summary>
[Serializable]
public class BoardPrefab
{
    //private�ϐ�.
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
/// �ՖʊǗ��v���O����.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] BoardPrefab prfb;

    [Header("- value -")]
    [SerializeField] int debugX;
    [SerializeField] int debugY;

    BoardData[,] board; //�Ֆʃf�[�^������2�����z��.

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
    /// �Ֆʂ̏�����.
    /// </summary>
    private void InitBoard()
    {
        //�}�X�������߂�.
        board = new BoardData[Gl_Const.BOARD_GRID_WID, Gl_Const.BOARD_GRID_HEI];

        //�S�}�X���[�v.
        for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++) {

                //�}�X�̏�����.
                board[x, y] = new BoardData(TileType.EMPTY);

                //�������͕ǂɂ���.
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
    /// �Ֆʂ̐���.
    /// </summary>
    private void GenerateBoard()
    {
        EraseAllTerrain(); //�Đ������邽�߈�U����.

        //�S�}�X���[�v.
        for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++) {
            for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++) {

                GameObject obj = null;

                //�n�`��.
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

                //�z�u.
                if (obj != null)
                {
                    Gl_Func.PlaceInBPos(obj, x+debugX, y+debugY);
                    //Gl_Func.ObjectSetBoard(obj, x, y);
                }
            }
        }
    }

    /// <summary>
    /// �Ֆʂ�Terrain�̑S����.
    /// </summary>
    private void EraseAllTerrain()
    {
        /*
           childCount�Ŏqobj�̐����擾�\.

           �������ADestroy()�Ŏqobj���������Ă������ɂ͔��f���ꂸ
           ��x�������I���܂ő��݂͎c�葱������ۂ�.
        */

        //�qobj�̐�������������.
        for (int i = 0; i < prfb.inObj.transform.childCount; i++){
            //�擾.
            var obj = prfb.inObj.transform.GetChild(i);
            if (obj != null)
            {
                Destroy(obj.gameObject); //����.
            }
        }
    }
}
#endif