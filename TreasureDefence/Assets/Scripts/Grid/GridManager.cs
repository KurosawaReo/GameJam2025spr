/*
   - GridManager.cs -

   伊野原先輩作.
   盤面の縦横グリッド数は可変値っぽいため、ここから取得する.
*/
using Gloval;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Tooltip("盤面の横の個数")]
    public int width = 10;

    [Tooltip("盤面の縦の個数")]
    public int height = 10;

    [Tooltip("盤面のプレハブをセット")]
    [SerializeField] GameObject gridPrefab;

    [Tooltip("盤面をまとめる親オブジェクトをセット")]
    [SerializeField] Transform gridParent;

    [Tooltip("駒のプレハブをセット")]
    [SerializeField] GameObject piecePrefab;

    [Tooltip("駒をまとめる親オブジェクトをセット")]
    [SerializeField] Transform pieceParent;

    [Tooltip("盤面の情報をセット")]
    public GridCell[,] grid;

    [Tooltip("盤面の情報をセット")]
    string savePath;

    [Tooltip("ノードリスト")]
    List<Node> nodes = new List<Node>();

    private void Awake()
    {
        // 盤面情報を読み込む
        LoadGrid();

        // デバッグ用
        //InitGrid();
        //PrintGrid();

        // 盤面を生成
        GenerateGridObjects();
    }

    void Start()
    {
        
    }

    /// <summary>
    /// デバッグ用.
    /// 盤面の初期化.
    /// </summary>
    void InitGrid()
    {
        grid = new GridCell[width, height];

        // すべてを空の状態にする
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = new GridCell(Gloval.TileType.EMPTY);
            }
        }
    }

    /// <summary>
    /// デバッグ用.
    /// 盤面のプリント.
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
    /// 目標位置を探す
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
                    print($"宝が見つかりました:{x},{y}");
                    return new Vector2Int(x, y);
                }
            }
        }
        print("宝が見つかりませんでした");
        return Vector2Int.zero;  // 見つからない場合（0, 0）
    }

    /// <summary>
    /// 盤面オブジェクトを生成.
    /// </summary>
    void GenerateGridObjects()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                // todo 画像を変更できるようにする

                //var sprite = GetPrefab(grid[x, y].tileType);
                //if (sprite != null)
                //{
                //    var obj = Instantiate(gridPrefab, new Vector3(x, 0, y), Quaternion.identity);
                //    obj.GetComponent<Image>().sprite = sprite;
                //}
       
                var sprite = GetPrefab(grid[x, y].tileType);
                if (sprite != null)
                {
                    // 生成
                    var obj = Instantiate(gridPrefab, gridParent);

                    // 位置を移動
                    obj.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);

                    // 画像を変更
                    obj.GetComponent<Image>().sprite = sprite;
                }

                // 生成
                //var obj = Instantiate(gridPrefab, gridParent);

                // 位置を移動
                //obj.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE,y * Gl_Const.BOARD_CELL_SIZE);
            }
        }
    }

    /// <summary>
    /// Imageを取得.
    /// </summary>
    /// <param name="_type">タイルタイプ</param>
    /// <returns>盤面のプレハブ</returns>
    Sprite GetPrefab(TileType _type)
    {
        Sprite sprite;
        try
        {
            // 送られてきたtypeを使って画像をロード
            sprite = Resources.Load<Sprite>(Gl_Const.TILE_TYPE_IMAGES_PATH[(int)_type]);
            return sprite;
        }
        catch
        {
            print("タイル画像をロードできませんでした。");
            return null;
        }
    }

    /// <summary>
    /// 駒を置けるか判定.
    /// </summary>
    /// <param name="x">横のインデックス</param>
    /// <param name="y">縦のインデックス</param>
    /// <returns>置けるかどうか</returns>
    bool CanPlacePiece(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            // 範囲外
            return false;
        }

        if (grid[x, y].tileType == TileType.OBSTACLE || grid[x, y].tileType == TileType.WALL ||
            grid[x, y].tileType == TileType.TREASURE || grid[x, y].tileType == TileType.ENEMY_SPAWN)
        {
            // 障害物や壁、宝、敵生成ポイントの上には置けない
            return false;
        }

        if (grid[x, y].isOccupied)
        {
            // すでにコマがある場合は置けない
            return false;
        }

        // それ以外はOK
        return true;
    }

    /// <summary>
    /// 駒を生成.
    /// </summary>
    /// <param name="x">横のインデックス</param>
    /// <param name="y">縦のインデックス</param>
    void PlacePiece(int x, int y)
    {
        if (!CanPlacePiece(x, y))
        {
            print("ここにはコマを置けません！");
            return;
        }

        var piece = Instantiate(piecePrefab, pieceParent);
        piece.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);
        grid[x, y].isOccupied = true;
    }

#if false
    /// <summary>
    /// 敵を描画
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
    /// 拡張エディター用.
    /// タイル情報を取得.
    /// </summary>
    /// <param name="x">横のインデックス</param>
    /// <param name="y">縦のインデックス</param>
    /// <returns>タイル情報</returns>
    public TileType GetTileType(int x, int y)
    {
        return grid[x, y].tileType;
    }

    /// <summary>
    /// 拡張エディター用.
    /// タイル情報を編集.
    /// </summary>
    /// <param name="x">横のインデックス</param>
    /// <param name="y">縦のインデックス</param>
    /// <param name="type">変更するタイル情報</param>
    public void SetTileType(int x, int y, TileType type)
    {
        grid[x, y].tileType = type;
    }

    /// <summary>
    /// 拡張エディター用.
    /// 盤面情報を保存.
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
        print("盤面データを保存しました: " + savePath);
    }

    /// <summary>
    /// 拡張エディター用.
    /// 盤面情報を読み込む.
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

            print("盤面データを読み込みました");
        }
        else
        {
            Debug.LogWarning("保存データがありません");
        }
    }

    /// <summary>
    /// 盤面の新規作成
    /// </summary>
    /// <param name="w">幅</param>
    /// <param name="h">高さ</param>
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

        print($"新しい {width}x{height} の盤面を作成しました");
    }

    /// <summary>
    /// 隣接しているマスを取得.
    /// </summary>
    /// <param name="position">検索の基準となる位置</param>
    /// <returns>座標</returns>
    public List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = {
        new Vector2Int(1, 0),   // 右
        new Vector2Int(-1, 0),  // 左
        new Vector2Int(0, 1),   // 上
        new Vector2Int(0, -1),  // 下
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
    /// 位置がグリッド内かどうかを判定するメソッド
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsPositionValid(Vector2Int position)
    {
        // 位置がグリッドの範囲内に収まっているか確認
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    /// <summary>
    /// マスが障害物かどうか判定.
    /// </summary>
    /// <param name="position">位置</param>
    /// <returns>障害物かどうか</returns>
    public bool IsObstacle(Vector2Int position)
    {
        return grid[position.x, position.y].tileType == TileType.OBSTACLE      ||
               grid[position.x, position.y].tileType == TileType.RIDE_OBSTACLE ||
               grid[position.x, position.y].tileType == TileType.WALL;
    }

    /// <summary>
    /// 盤面の範囲チェック.
    /// </summary>
    /// <param name="position">位置</param>
    /// <returns>盤面の範囲内かどうか</returns>
    public bool IsInsideGrid(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }
}