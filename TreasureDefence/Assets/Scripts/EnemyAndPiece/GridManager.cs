using Gloval;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Tooltip("グリッドの横の個数")]
    [SerializeField] int width = 10;

    [Tooltip("グリッドの縦の個数")]
    [SerializeField] int height = 10;
    
    [Tooltip("グリッドのプレハブをセット")]
    [SerializeField] GameObject gridPrefab;

    [Tooltip("グリッドのプレハブをセット")]
    [SerializeField] Transform gridParent;

    [Tooltip("グリッドの情報をセット")]
    GridCell[,] grid;

    void Start()
    {
        // デバッグ用
        InitGrid();
        PrintGrid();

        // グリッドを生成
        GenerateGridObjects();
    }

    /// <summary>
    /// デバッグ用.
    /// グリッドの初期化.
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
    /// グリッドのプリント.
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
    /// グリッドオブジェクトを生成.
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

                // 生成
                var obj = Instantiate(gridPrefab, gridParent);

                // 位置を移動
                obj.transform.localPosition = new Vector2(x * Gl_Const.CELL_SIZE,y * Gl_Const.CELL_SIZE);
            }
        }
    }

    /// <summary>
    /// Imageを取得.
    /// </summary>
    /// <param name="_type">タイルタイプ</param>
    /// <returns>グリッドのプレハブ</returns>
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
}