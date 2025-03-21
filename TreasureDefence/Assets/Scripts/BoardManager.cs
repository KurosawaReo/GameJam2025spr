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
        get { return m_id; }
        set { m_id = value; }
    }
}

/// <summary>
/// �Ֆʂ̃f�[�^.
/// </summary>
public class BoardData
{
    //private�ϐ�.
    private TerrainType m_terrain;
    private Piece m_plyPiece;
    private Piece m_enmPiece;

    //������(�R���X�g���N�^)
    public BoardData(TerrainType _terrain)
    {
        m_terrain = _terrain;
    }

    //get, set.
    public TerrainType terrain 
    {
        get { return m_terrain; }
        set { m_terrain = value; } 
    }
    public Piece plyPiece
    {
        get { return m_plyPiece; }
        set { m_plyPiece = value; }
    }
    public Piece enmPiece
    {
        get { return m_enmPiece; }
        set { m_enmPiece = value; }
    }
}

/// <summary>
/// Board�Ɏg��prefab�܂Ƃ�.
/// </summary>
[Serializable]
public class BoardPrefab
{
    //private�ϐ�.
    [SerializeField] private GameObject m_ground;
    [SerializeField] private GameObject m_wall;
    [SerializeField] private GameObject m_obstacles;
    [SerializeField] private GameObject m_enemyGate;
    [SerializeField] private GameObject m_treasure;
    [Space]
    [SerializeField] private GameObject m_inObj;

    //get, set.
    public GameObject ground 
    { 
        get { return m_ground; }
        set { m_ground = value; } 
    }
    public GameObject wall
    {
        get { return m_wall; }
        set { m_wall = value; }
    }
    public GameObject obstacles
    {
        get { return m_obstacles; }
        set { m_obstacles = value; }
    }
    public GameObject enemyGate
    {
        get { return m_enemyGate; }
        set { m_enemyGate = value; }
    }
    public GameObject treasure
    {
        get { return m_treasure; }
        set { m_treasure = value; }
    }
    public GameObject inObj
    {
        get { return m_inObj; }
        set { m_inObj = value; }
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
                board[x, y] = new BoardData(TerrainType.NONE);

                //�������͕ǂɂ���.
                if (x == 0 || x == Gl_Const.BOARD_GRID_WID-1 ||
                    y == 0 || y == Gl_Const.BOARD_GRID_HEI-1 )
                {
                    board[x, y].terrain = TerrainType.WALL;
                }
            }
        }

        //test.
        board[1, 1].terrain = TerrainType.OBSTACLES;
        board[2, 1].terrain = TerrainType.ENEMY_GATE;
        board[3, 1].terrain = TerrainType.TREASURE;
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
                    case TerrainType.NONE:
                        break;
                    case TerrainType.WALL:
                        obj = Instantiate(prfb.wall, prfb.inObj.transform);
                        break;
                    case TerrainType.OBSTACLES:
                        obj = Instantiate(prfb.obstacles, prfb.inObj.transform);
                        break;
                    case TerrainType.ENEMY_GATE:
                        obj = Instantiate(prfb.enemyGate, prfb.inObj.transform);
                        break;
                    case TerrainType.TREASURE:
                        obj = Instantiate(prfb.treasure, prfb.inObj.transform);
                        break;

                    default:
                        break;
                }

                //�z�u.
                if (obj != null)
                {
                    Gl_Func.ObjectSetBoard(obj, x+debugX, y+debugY);
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
