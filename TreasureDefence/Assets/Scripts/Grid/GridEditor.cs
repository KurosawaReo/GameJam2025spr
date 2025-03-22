using Gloval;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class GridEditor : EditorWindow
{
    [Tooltip("ガイドマネージャーをセット")]
    GridManager gridManager;

    [Tooltip("選択中のタイルタイプ")]
    TileType selectedTileType = TileType.EMPTY;
    
    [Tooltip("新規盤面の幅")]
    int newWidth = 10;
    
    [Tooltip("新規盤面の高さ")]
    int newHeight = 10;

    [MenuItem("Tools/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditor>("Grid Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Editor", EditorStyles.boldLabel);

        // 対象となる GridManager のオブジェクトを指定
        gridManager = (GridManager)EditorGUILayout.ObjectField("Grid Manager", gridManager, typeof(GridManager), true);

        if (gridManager == null)
        {
            EditorGUILayout.HelpBox("GridManager をシーン内で選択してください", MessageType.Warning);
            return;
        }

        // 盤面データがない場合、新規作成ボタンを表示
        if (gridManager.grid == null || gridManager.grid.Length == 0)
        {
            EditorGUILayout.HelpBox("盤面データがありません。新しく作成してください。", MessageType.Warning);

            newWidth = EditorGUILayout.IntField("Width", newWidth);
            newHeight = EditorGUILayout.IntField("Height", newHeight);

            if (GUILayout.Button("新しい盤面を作成"))
            {
                gridManager.CreateNewGrid(newWidth, newHeight);
                EditorUtility.SetDirty(gridManager);
            }

            if (GUILayout.Button("盤面データを読み込み (JSON)"))
            {
                LoadGridFromJson();
            }

            return;
        }

        // タイルの種類を選択
        selectedTileType = (TileType)EditorGUILayout.EnumPopup("Tile Type", selectedTileType);

        // 盤面をGUIで描画
        DrawGrid();

        GUILayout.Space(10);

        // JSONの保存・読み込みボタン
        if (GUILayout.Button("盤面データを保存 (JSON)"))
        {
            SaveGridToJson();
        }

        if (GUILayout.Button("盤面データを読み込み (JSON)"))
        {
            LoadGridFromJson();
        }
    }

    /// <summary>
    /// 盤面の描画
    /// </summary>
    private void DrawGrid()
    {
        if (gridManager == null)
        {
            return;
        }

        for (int y = gridManager.height - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < gridManager.width; x++)
            {
                var tile = gridManager.GetTileType(x, y);
                var style = new GUIStyle(GUI.skin.button);

                // タイルの色設定
                switch (tile)
                {
                    case TileType.EMPTY: style.normal.textColor = Color.white; break;
                    case TileType.OBSTACLE: style.normal.textColor = Color.red; break;
                    case TileType.RIDE_OBSTACLE: style.normal.textColor = Color.cyan; break;
                    case TileType.WALL: style.normal.textColor = Color.blue; break;
                    case TileType.TREASURE: style.normal.textColor = Color.yellow; break;
                    case TileType.ENEMY_SPAWN: style.normal.textColor = Color.green; break;
                }

                // ボタンのスタイル設定
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.normal.background = MakeTexture(30, 30, style.normal.textColor);

                if (GUILayout.Button("", buttonStyle, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    gridManager.SetTileType(x, y, selectedTileType);
                    EditorUtility.SetDirty(gridManager);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// 指定した色のテクスチャを作成
    /// </summary>
    /// <param name="width">幅</param>
    /// <param name="height">高さ</param>
    /// <param name="color">色</param>
    /// <returns>テクスチャ</returns>
    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private void SaveGridToJson()
    {
        if (gridManager == null) return;

        var path = Path.Combine(Application.persistentDataPath, "gridData.json");
        if (string.IsNullOrEmpty(path)) return;

        var data = new GridData { width = gridManager.width, height = gridManager.height };

        for (var x = 0; x < gridManager.width; x++)
        {
            for (var y = 0; y < gridManager.height; y++)
            {
                data.cells.Add(new CellData(x, y, gridManager.GetTileType(x, y)));
            }
        }

        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("盤面データを保存しました: " + path);
    }

    private void LoadGridFromJson()
    {
        if (gridManager == null) return;

        string path = EditorUtility.OpenFilePanel("Load Grid Data", "", "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = File.ReadAllText(path);
        GridData data = JsonUtility.FromJson<GridData>(json);

        gridManager.width = data.width;
        gridManager.height = data.height;

        for (int x = 0; x < data.width; x++)
        {
            for (int y = 0; y < data.height; y++)
            {
                CellData cell = data.cells.Find(c => c.x == x && c.y == y);
                if (cell != null)
                {
                    gridManager.SetTileType(x, y, cell.tileType);
                }
            }
        }

        EditorUtility.SetDirty(gridManager);
        Debug.Log("盤面データを読み込みました");
    }
}

[Serializable]
public class GridData
{
    public int width;
    public int height;
    public List<CellData> cells = new List<CellData>();
}

[Serializable]
public class CellData
{
    public int x;
    public int y;
    public TileType tileType;

    public CellData(int x, int y, TileType tileType)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;
    }
}