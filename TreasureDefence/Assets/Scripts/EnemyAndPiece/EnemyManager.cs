using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("�����Ă���G�̃��X�g")]
    List<Enemy> enemies = new List<Enemy>();

    [Tooltip("�G�̃v���n�u���Z�b�g")]
    [SerializeField] GameObject enemyPrefab;

    [Tooltip("�G���܂Ƃ߂�e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] Transform enemyParent;

    [Tooltip("�O���b�h�}�l�[�W���[���Z�b�g")]
    public GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Init();

        // �G�𐶐�
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ����������
    /// </summary>
    void Init()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    /// <summary>
    /// �G�̒ǉ�
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    /// <summary>
    /// �G��`��
    /// </summary>
    void SpawnEnemies()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (gridManager.grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    var enemy = Instantiate(enemyPrefab, enemyParent);
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.CELL_SIZE, y * Gl_Const.CELL_SIZE);
                }
            }
        }
    }
}
