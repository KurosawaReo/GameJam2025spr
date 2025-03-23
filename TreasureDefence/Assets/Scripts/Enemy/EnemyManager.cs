using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("�����Ă���G�̃��X�g")]
    List<Enemy> enemies = new List<Enemy>();

    [Tooltip("�G�̃v���n�u���Z�b�g")]
    [SerializeField] GameObject enemyPrefab;

    [Tooltip("�G���܂Ƃ߂�e�I�u�W�F�N�g���Z�b�g")]
    [SerializeField] Transform enemyParent;

    [Tooltip("�G��ScriptableObject���Z�b�g")]
    [SerializeField] List<EnemyData> enemyDatas = new List<EnemyData>();

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
        for (int x = 0; x < Gl_Const.BOARD_GRID_WID; x++)
        {
            for (int y = 0; y < Gl_Const.BOARD_GRID_HEI; y++)
            {
                if (gridManager.grid[x, y].tileType == TileType.ENEMY_SPAWN)
                {
                    // �m���ŏ������X�L�b�v���ēG�𐶐����Ȃ��悤�ɂ���
                    var rnd = Random.Range(0,100);

                    if (rnd < 30)
                    {
                        // �������X�L�b�v
                        continue;
                    }

                    // �G�̐����|�C���g����������
                    // �G�̎�ނ��璊�I�����Ăǂ̓G���o���̂����߂�
                    var enemyIndex = Random.Range(0, enemyDatas.Count);
                    //print($"�G�̔ԍ��F{enemyIndex}");

                    // �G����
                    var enemy = Instantiate(enemyPrefab, enemyParent);

                    // Enemy��ScriptableObject���Z�b�g
                    enemy.GetComponent<Enemy>().enemyData = enemyDatas[enemyIndex];

                    // �������W�𒲐�
                    enemy.transform.localPosition = new Vector2(x * Gl_Const.BOARD_CELL_SIZE, y * Gl_Const.BOARD_CELL_SIZE);

                    // �G�̉摜��ύX
                    enemy.GetComponent<Image>().sprite = enemyDatas[enemyIndex].sprite;

                    // ���X�g�ɓG��ǉ�
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
