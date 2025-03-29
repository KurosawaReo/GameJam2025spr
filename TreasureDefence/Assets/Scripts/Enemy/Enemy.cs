using Gloval;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("HP�c��")]
    int hp;

    [Tooltip("�U����")]
    int atk;

    [Tooltip("���݂ǂ̃}�X�ɂ���̂����Z�b�g")]
    public Vector2Int currentPos;
    
    [Tooltip("�ڕW�ƂȂ�}�Xor��̈ʒu���Z�b�g")]
    Vector2Int targetPos;
    
    [Tooltip("�O���b�h�}�l�[�W���[���Z�b�g")]
    GridManager gridManager;

    [Tooltip("�G���X�e�[�g���Ǘ�����p")]
    public EntityState currentState = EntityState.STOP;

    [Tooltip("�G��ScriptableObject���Z�b�g")]
    public EntityBase enemyData;

    [Tooltip("�U���̑Ώ�")]
    GameObject target;

    [Tooltip("1�x�����������Ȃ��Ƃ���Ŏg���t���O")]
    bool isFirst;

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (currentState)
        {
            case EntityState.STOP:
                StopState();
                break;

            case EntityState.MOVE:
                MoveState();
                break;
            
            case EntityState.ATTACK:
                AttackState();
                break;
            
            case EntityState.DEATH:
                DeathState();
                break;
            
            case EntityState.NONE:
                break;
        }
    }

    /// <summary>
    /// ����������
    /// </summary>
    void Init()
    {
        isFirst = true;

        // HP�ƍU���͂��f�[�^�ŏ���������
        hp  = enemyData.hp;
        atk = enemyData.atk;

        // �O���b�h�}�l�[�W���[���擾
        gridManager = FindObjectOfType<GridManager>();

        // ���݂̈ʒu���Z���T�C�Y�Ŋ����Ăǂ̃}�X�ɂ���̂�����肷��
        currentPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / Gl_Const.BOARD_CELL_SIZE),
                                    Mathf.RoundToInt(transform.localPosition.y / Gl_Const.BOARD_CELL_SIZE));

        // �G�̈ʒu��ێ�������
        gridManager.activeEnemyList.Add(currentPos);

        // ��̈ʒu���擾
        targetPos = gridManager.FindTreasurePosition();

        // �ړ�����
        StartCoroutine(MoveToTarget());
    }

    /// <summary>
    /// �U�����󂯂����̏���
    /// </summary>
    /// <param name="atk">�U���������̍U����</param>
    public void Damage(int atk)
    {
        hp -= atk;

        hp = 0;
        var index = 0;

        for (var i = 0; i < gridManager.activeEnemyObjList.Count; i++)
        {
            if (gridManager.activeEnemyObjList[i] == gameObject)
            {
                // �������g�̏ꍇ
                index = i;
                break;
            }
        }

        // �擾�����C���f�b�N�X�̗v�f���폜����
        gridManager.activeEnemyList.RemoveAt(index);
        gridManager.activeEnemyObjList.RemoveAt(index);

        Destroy(gameObject); // ���M��j������
    }

    /// <summary>
    /// �ڕW�Ɍ������Ĉړ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToTarget()
    {
        // �ڕW�ɓ��B����܂őҋ@
        while (currentPos != targetPos)
        {
            if (!gridManager.IsInsideGrid(currentPos) || !gridManager.IsInsideGrid(targetPos))
            {
                Debug.LogError("�����Ȉʒu�ł��I");
                yield break; // �R���[�`���𑁊��ɏI��
            }

            var path = AStarPathfinding(currentPos, targetPos);

            // ��Ԉُ�̏ꍇ�͏����������X�L�b�v

            if (path.Count > 1)
            {
                var isEncount = false; // �G���J�E���g�������ǂ���
                var nextPos = path[1]; // ���̈ړ�����擾
                
                //print($"���݂̈ʒu�F{currentPos}�A�ړ���̈ʒu�F{nextPos}");

                // ���̈ړ���ɋ���邩�m�F����

                var count = 0;
                foreach (var piece in gridManager.activePieceList)
                {
                    if (piece == nextPos)
                    {
                        // ���̈ړ���ɋ�����ꍇ�������X�L�b�v����
                        print("�G���J�E���g���܂���");
                        isEncount = true; // �G���J�E���g�����̂�true�ɂ���
                        target = gridManager.activePieceObjList[count];
                        break;
                    }

                    count++;
                }

                if (isEncount)
                {
                    //print("�G���J�E���g���܂���");
                    currentState = EntityState.ATTACK; // �G���J�E���g���Ă���̂ōU��������
                    //break; // �G���J�E���g���Ă����珈�������Ȃ�
                }
                else
                {
                    yield return MoveTo(nextPos);                   // �ړ�������҂��Č㑱�̏����𑱍s����
                    currentPos = nextPos;                           // �ړ�����̂ňړ���̈ʒu�ŏ㏑������

                    // ���݂̈ʒu���Z���T�C�Y�Ŋ����Ăǂ̃}�X�ɂ���̂�����肷��
                    currentPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / Gl_Const.BOARD_CELL_SIZE),
                                                Mathf.RoundToInt(transform.localPosition.y / Gl_Const.BOARD_CELL_SIZE));

                    //print($"currentPos:{currentPos}");
                    //gridManager.activeEnemyList[count] = currentPos;    // �G�̈ʒu��ێ�������
                }
            }
            else
            {
                break; // ���[�g��������Ȃ��ꍇ�͒�~
            }

            yield return null;
        }
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    /// <param name="nextPos">���̈ړ���</param>
    /// <returns></returns>
    IEnumerator MoveTo(Vector2Int nextPos)
    {
        var start = transform.localPosition;
        var end = new Vector3(nextPos.x * Gl_Const.BOARD_CELL_SIZE, nextPos.y * Gl_Const.BOARD_CELL_SIZE, 0);
        var duration = 0.2f;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localPosition = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime / 10;
            yield return null;
        }

        transform.localPosition = end;
    }

    /// <summary>
    /// ���[�g�T������
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    List<Vector2Int> AStarPathfinding(Vector2Int start, Vector2Int goal)
    {
        // ������A*�A���S���Y���̎����i�O���b�h�����ɍœK���[�g���v�Z�j
        var path = new List<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float>();
        var fScore = new Dictionary<Vector2Int, float>();
        var openSet = new PriorityQueue<Vector2Int>();

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);
        openSet.Enqueue(start, fScore[start]);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Vector2Int neighbor in gridManager.GetNeighbors(current))
            {
                float tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }
        return path; // ���[�g��������Ȃ��ꍇ
    }

    float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // �}���n�b�^������
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    /// <summary>
    /// �X�e�[�g��Stop�̎��̏���
    /// </summary>
    void StopState()
    {
    }

    /// <summary>
    /// �X�e�[�g��Move�̎��̏���
    /// </summary>
    void MoveState()
    {
    }

    /// <summary>
    /// �X�e�[�g��Attack�̎��̏���
    /// </summary>
    void AttackState()
    {
        // 1�x�����ʂ�Ȃ��悤�ɂ���
        if (isFirst)
        {
            //print("�A�^�b�N�X�e�[�g��");
            // �U���������Ă�
            StartCoroutine(Attack());
            
            isFirst = false;
        }
    }

    /// <summary>
    /// �X�e�[�g��Death�̎��̏���
    /// </summary>
    void DeathState()
    {
        // ���S����
    }

    /// <summary>
    /// �X�e�[�g��ύX���鎞�̏���
    /// </summary>
    public void ChangeState(EntityState newState)
    {
        // �ύX��̃X�e�[�g���ύX�O�Ɠ����ł͂Ȃ����m�F����
        if (currentState == newState)
        {
            return;
        }

        print($"State changed: {currentState} �� {newState}");
        currentState = newState;
    }

    /// <summary>
    /// �U������
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // �����f�B���C���������̂ŏ���������҂�
        yield return Gl_Func.Delay(1f);

        // ���ۂɍU������
        //print("�U��");
        if (target != null)
        {
            target.GetComponent<Piece>().Damage(atk);
        }

        isFirst = true; // �U���̃X�e�[�g�Ŏg���̂Ńt���O��߂��Ă���
        currentState = EntityState.MOVE; // �X�e�[�g��Move�ɕύX
    }
}

public class PriorityQueue<T>
{
    List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
        elements = elements.OrderBy(e => e.Value).ToList();
    }

    public T Dequeue()
    {
        var bestItem = elements[0];
        elements.RemoveAt(0);
        return bestItem.Key;
    }

    public bool Contains(T item)
    {
        return elements.Any(e => EqualityComparer<T>.Default.Equals(e.Key, item));
    }
}