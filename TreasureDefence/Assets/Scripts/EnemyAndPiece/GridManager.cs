using Gloval;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Tooltip("�O���b�h�̉��̌�")]
    [SerializeField] int width = 10;

    [Tooltip("�O���b�h�̏c�̌�")]
    [SerializeField] int height = 10;
    
    [Tooltip("�O���b�h�̃v���n�u���Z�b�g")]
    [SerializeField] GameObject gridPrefab;

    [Tooltip("�O���b�h�̃v���n�u���Z�b�g")]
    [SerializeField] Transform gridParent;

    [Tooltip("�O���b�h�̏����Z�b�g")]
    GridCell[,] grid;

    void Start()
    {
        // �f�o�b�O�p
        InitGrid();
        PrintGrid();

        // �O���b�h�𐶐�
        GenerateGridObjects();
    }

    /// <summary>
    /// �f�o�b�O�p.
    /// �O���b�h�̏�����.
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
    /// �O���b�h�̃v�����g.
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
    /// �O���b�h�I�u�W�F�N�g�𐶐�.
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

                // ����
                var obj = Instantiate(gridPrefab, gridParent);

                // �ʒu���ړ�
                obj.transform.localPosition = new Vector2(x * Gl_Const.CELL_SIZE,y * Gl_Const.CELL_SIZE);
            }
        }
    }

    /// <summary>
    /// Image���擾.
    /// </summary>
    /// <param name="_type">�^�C���^�C�v</param>
    /// <returns>�O���b�h�̃v���n�u</returns>
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
}