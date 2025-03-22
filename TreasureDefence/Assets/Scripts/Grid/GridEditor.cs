using Gloval;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class GridEditor : EditorWindow
{
    [Tooltip("�K�C�h�}�l�[�W���[���Z�b�g")]
    GridManager gridManager;

    [Tooltip("�I�𒆂̃^�C���^�C�v")]
    TileType selectedTileType = TileType.EMPTY;
    
    [Tooltip("�V�K�Ֆʂ̕�")]
    int newWidth = 10;
    
    [Tooltip("�V�K�Ֆʂ̍���")]
    int newHeight = 10;

    [MenuItem("Tools/Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridEditor>("Grid Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Editor", EditorStyles.boldLabel);

        // �ΏۂƂȂ� GridManager �̃I�u�W�F�N�g���w��
        gridManager = (GridManager)EditorGUILayout.ObjectField("Grid Manager", gridManager, typeof(GridManager), true);

        if (gridManager == null)
        {
            EditorGUILayout.HelpBox("GridManager ���V�[�����őI�����Ă�������", MessageType.Warning);
            return;
        }

        // �Ֆʃf�[�^���Ȃ��ꍇ�A�V�K�쐬�{�^����\��
        if (gridManager.grid == null || gridManager.grid.Length == 0)
        {
            EditorGUILayout.HelpBox("�Ֆʃf�[�^������܂���B�V�����쐬���Ă��������B", MessageType.Warning);

            newWidth = EditorGUILayout.IntField("Width", newWidth);
            newHeight = EditorGUILayout.IntField("Height", newHeight);

            if (GUILayout.Button("�V�����Ֆʂ��쐬"))
            {
                gridManager.CreateNewGrid(newWidth, newHeight);
                EditorUtility.SetDirty(gridManager);
            }

            if (GUILayout.Button("�Ֆʃf�[�^��ǂݍ��� (JSON)"))
            {
                LoadGridFromJson();
            }

            return;
        }

        // �^�C���̎�ނ�I��
        selectedTileType = (TileType)EditorGUILayout.EnumPopup("Tile Type", selectedTileType);

        // �Ֆʂ�GUI�ŕ`��
        DrawGrid();

        GUILayout.Space(10);

        // JSON�̕ۑ��E�ǂݍ��݃{�^��
        if (GUILayout.Button("�Ֆʃf�[�^��ۑ� (JSON)"))
        {
            SaveGridToJson();
        }

        if (GUILayout.Button("�Ֆʃf�[�^��ǂݍ��� (JSON)"))
        {
            LoadGridFromJson();
        }
    }

    /// <summary>
    /// �Ֆʂ̕`��
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

                // �^�C���̐F�ݒ�
                switch (tile)
                {
                    case TileType.EMPTY: style.normal.textColor = Color.white; break;
                    case TileType.OBSTACLE: style.normal.textColor = Color.red; break;
                    case TileType.RIDE_OBSTACLE: style.normal.textColor = Color.cyan; break;
                    case TileType.WALL: style.normal.textColor = Color.blue; break;
                    case TileType.TREASURE: style.normal.textColor = Color.yellow; break;
                    case TileType.ENEMY_SPAWN: style.normal.textColor = Color.green; break;
                }

                // �{�^���̃X�^�C���ݒ�
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
    /// �w�肵���F�̃e�N�X�`�����쐬
    /// </summary>
    /// <param name="width">��</param>
    /// <param name="height">����</param>
    /// <param name="color">�F</param>
    /// <returns>�e�N�X�`��</returns>
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
        Debug.Log("�Ֆʃf�[�^��ۑ����܂���: " + path);
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
        Debug.Log("�Ֆʃf�[�^��ǂݍ��݂܂���");
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