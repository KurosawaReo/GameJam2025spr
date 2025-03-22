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

    [Tooltip("���L���X�g�^�C��")]
    float recastTime;

    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Init();

        StartCoroutine(SpawnEnemies());
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
        recastTime = Gl_Const.ENEMY_DEFAULT_RECAST_TIME;
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
    IEnumerator SpawnEnemies()
    {
        // todo Enemy�𒊑I����
        // todo ���I���ʂ̓G��ScrptableObject���擾or�g�p���ēG�̉摜��ύX����
        // todo �ł���Ίe�G�X�|�[���̂Ƃ��납�烉���_���Ȏ��ԂœG���o�Ă���悤�ɂ���

        for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++)
        {
            for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++)
            {
                if (gridManager.grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    var enemy = Instantiate(enemyPrefab, enemyParent);
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.CELL_SIZE, y * Gl_Const.CELL_SIZE);
                    AddEnemy(enemy.GetComponent<Enemy>());
                }
            }
        }

        yield return new WaitForSeconds(recastTime);
        // �������Ԃ����񂾂�Z������
        recastTime -= 0.01f;
        StartCoroutine(SpawnEnemies());
    }
}
