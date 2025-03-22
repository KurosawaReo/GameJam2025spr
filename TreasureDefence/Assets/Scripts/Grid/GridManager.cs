/*
   - GridManager.cs -

   �ɖ쌴��y��.
   �Ֆʂ̏c���O���b�h���͉ϒl���ۂ����߁A��������擾����.
*/
using Gloval;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Tooltip("�Ֆʂ̉��̌�")]
    public int width = 10;

    [Tooltip("�Ֆʂ̏c�̌�")]
    public int height = 10;

    [Tooltip("�Ֆʂ̃v���n�u���Z�b�g")]
    [SerializeField] GameObject gridPrefab;

    [Tooltip("�Ֆʂ��܂Ƃ߂�e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] Transform gridParent;

    [Tooltip("��̃v���n�u���Z�b�g")]
    [SerializeField] GameObject piecePrefab;

    [Tooltip("����܂Ƃ߂�e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] Transform pieceParent;

    [Tooltip("�Ֆʂ̏����Z�b�g")]
    public GridCell[,] grid;

    [Tooltip("�Ֆʂ̏����Z�b�g")]
    string savePath;

    [Tooltip("�m�[�h���X�g")]
    List<Node> nodes = new List<Node>();

    private void Awake()
    {
        // �Ֆʏ���ǂݍ���
        LoadGrid();

        // �f�o�b�O�p
        //InitGrid();
        //PrintGrid();

        // �Ֆʂ𐶐�
        GenerateGridObjects();
    }

    void Start()
    {
        
    }

    /// <summary>
    /// �f�o�b�O�p.
    /// �Ֆʂ̏�����.
    /// </summary>
    void InitGrid()
    {
        grid = new GridCell[width, height];

        // ���ׂĂ���̏�Ԃɂ���
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = new GridCell(Gloval.TileType.EMPTY);
            }
        }
    }

    /// <summary>
    /// �f�o�b�O�p.
    /// �Ֆʂ̃v�����g.
    /// </summary>
    void PrintGrid()
    {
        for (var y = height - 1; y >= 0; y--)
        {
            var row = "";
            for (var x = 0; x < width; x++)
            {
                row += (int)grid[x, y].tileType + " ";
            }
            print(row);
        }
    }

    /// <summary>
    /// �ڕW�ʒu��T��
    /// </summary>
    /// <returns></returns>
    public Vector2Int FindTreasurePosition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].tileType == TileType.TREASURE)
                {
                    print($"�󂪌�����܂���:{x},{y}");
                    return new Vector2Int(x, y);
                }
            }
        }
        print("�󂪌�����܂���ł���");
        return Vector2Int.zero;  // ������Ȃ��ꍇ�i0, 0�j
    }

    /// <summary>
    /// �ՖʃI�u�W�F�N�g�𐶐�.
    /// </summary>
    void GenerateGridObjects()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                // todo �摜��ύX�ł���悤�ɂ���

                //var sprite = GetPrefab(grid[x, y].tileType);
                //if (sprite != null)
                //{
                //    var obj = Instantiate(gridPrefab, new Vector3(x, 0, y), Quaternion.identity);
                //    obj.GetComponent<Image>().sprite = sprite;
                //}
       
                var sprite = GetPrefab(grid[x, y].tileType);
                if (sprite != null)
                {
                    // ����
                    var obj = Instantiate(gridPrefab, gridParent);

                    // �ʒu���ړ�
                    obj.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);

                    // �摜��ύX
                    obj.GetComponent<Image>().sprite = sprite;
                }

                // ����
                //var obj = Instantiate(gridPrefab, gridParent);

                // �ʒu���ړ�
                //obj.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE,y * Gl_Const.BOARD_CELL_SIZE);
            }
        }
    }

    /// <summary>
    /// Image���擾.
    /// </summary>
    /// <param name="_type">�^�C���^�C�v</param>
    /// <returns>�Ֆʂ̃v���n�u</returns>
    Sprite GetPrefab(TileType _type)
    {
        Sprite sprite;
        try
        {
            // �����Ă���type���g���ĉ摜�����[�h
            sprite = Resources.Load<Sprite>(Gl_Const.TILE_TYPE_IMAGES_PATH[(int)_type]);
            return sprite;
        }
        catch
        {
            print("�^�C���摜�����[�h�ł��܂���ł����B");
            return null;
        }
    }

    /// <summary>
    /// ���u���邩����.
    /// </summary>
    /// <param name="x">���̃C���f�b�N�X</param>
    /// <param name="y">�c�̃C���f�b�N�X</param>
    /// <returns>�u���邩�ǂ���</returns>
    bool CanPlacePiece(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            // �͈͊O
            return false;
        }

        if (grid[x, y].tileType == TileType.OBSTACLE || grid[x, y].tileType == TileType.WALL ||
            grid[x, y].tileType == TileType.TREASURE || grid[x, y].tileType == TileType.ENEMY_SPAWN)
        {
            // ��Q����ǁA��A�G�����|�C���g�̏�ɂ͒u���Ȃ�
            return false;
        }

        if (grid[x, y].isOccupied)
        {
            // ���łɃR�}������ꍇ�͒u���Ȃ�
            return false;
        }

        // ����ȊO��OK
        return true;
    }

    /// <summary>
    /// ��𐶐�.
    /// </summary>
    /// <param name="x">���̃C���f�b�N�X</param>
    /// <param name="y">�c�̃C���f�b�N�X</param>
    void PlacePiece(int x, int y)
    {
        if (!CanPlacePiece(x, y))
        {
            print("�����ɂ̓R�}��u���܂���I");
            return;
        }

        var piece = Instantiate(piecePrefab, pieceParent);
        piece.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);
        grid[x, y].isOccupied = true;
    }

#if false
    /// <summary>
    /// �G��`��
    /// </summary>
    void SpawnEnemies()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    var enemy = Instantiate(enemyPrefab, enemyParent);
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);
                }
            }
        }
    }
#endif

    /// <summary>
    /// �g���G�f�B�^�[�p.
    /// �^�C�������擾.
    /// </summary>
    /// <param name="x">���̃C���f�b�N�X</param>
    /// <param name="y">�c�̃C���f�b�N�X</param>
    /// <returns>�^�C�����</returns>
    public TileType GetTileType(int x, int y)
    {
        return grid[x, y].tileType;
    }

    /// <summary>
    /// �g���G�f�B�^�[�p.
    /// �^�C������ҏW.
    /// </summary>
    /// <param name="x">���̃C���f�b�N�X</param>
    /// <param name="y">�c�̃C���f�b�N�X</param>
    /// <param name="type">�ύX����^�C�����</param>
    public void SetTileType(int x, int y, TileType type)
    {
        grid[x, y].tileType = type;
    }

    /// <summary>
    /// �g���G�f�B�^�[�p.
    /// �Ֆʏ���ۑ�.
    /// </summary>
    public void SaveGrid()
    {
        var data = new GridData { width = width, height = height };

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                data.cells.Add(new CellData(x, y, grid[x, y].tileType));
            }
        }

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        print("�Ֆʃf�[�^��ۑ����܂���: " + savePath);
    }

    /// <summary>
    /// �g���G�f�B�^�[�p.
    /// �Ֆʏ���ǂݍ���.
    /// </summary>
    public void LoadGrid()
    {
        savePath = Gl_Const.GRID_JSON_PATH;

        if (File.Exists(savePath))
        {
            var json = File.ReadAllText(savePath);
            var data = JsonUtility.FromJson<GridData>(json);

            width = data.width;
            height = data.height;
            grid = new GridCell[width, height];

            foreach (var cell in data.cells)
            {
                grid[cell.x, cell.y] = new GridCell(cell.tileType);
            }

            print("�Ֆʃf�[�^��ǂݍ��݂܂���");
        }
        else
        {
            Debug.LogWarning("�ۑ��f�[�^������܂���");
        }
    }

    /// <summary>
    /// �Ֆʂ̐V�K�쐬
    /// </summary>
    /// <param name="w">��</param>
    /// <param name="h">����</param>
    public void CreateNewGrid(int w, int h)
    {
        width = w;
        height = h;
        grid = new GridCell[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = new GridCell(TileType.EMPTY);
            }
        }

        print($"�V���� {width}x{height} �̔Ֆʂ��쐬���܂���");
    }

    /// <summary>
    /// �אڂ��Ă���}�X���擾.
    /// </summary>
    /// <param name="position">�����̊�ƂȂ�ʒu</param>
    /// <returns>���W</returns>
    public List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = {
        new Vector2Int(1, 0),   // �E
        new Vector2Int(-1, 0),  // ��
        new Vector2Int(0, 1),   // ��
        new Vector2Int(0, -1),  // ��
        };

        foreach (var dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if (IsInsideGrid(neighborPos) && !IsObstacle(neighborPos))
            {
                neighbors.Add(neighborPos);
            }
        }
        return neighbors;
    }

    /// <summary>
    /// �ʒu���O���b�h�����ǂ����𔻒肷�郁�\�b�h
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsPositionValid(Vector2Int position)
    {
        // �ʒu���O���b�h�͈͓̔��Ɏ��܂��Ă��邩�m�F
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    /// <summary>
    /// �}�X����Q�����ǂ�������.
    /// </summary>
    /// <param name="position">�ʒu</param>
    /// <returns>��Q�����ǂ���</returns>
    public bool IsObstacle(Vector2Int position)
    {
        return grid[position.x, position.y].tileType == TileType.OBSTACLE      ||
               grid[position.x, position.y].tileType == TileType.RIDE_OBSTACLE ||
               grid[position.x, position.y].tileType == TileType.WALL;
    }

    /// <summary>
    /// �Ֆʂ͈̔̓`�F�b�N.
    /// </summary>
    /// <param name="position">�ʒu</param>
    /// <returns>�Ֆʂ͈͓̔����ǂ���</returns>
    public bool IsInsideGrid(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }
}