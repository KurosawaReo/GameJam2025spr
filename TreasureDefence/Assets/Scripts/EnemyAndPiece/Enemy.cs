using Gloval;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("���݂ǂ̃}�X�ɂ���̂����Z�b�g")]
    Vector2Int currentPos;
    
    [Tooltip("�ڕW�ƂȂ�}�Xor��̈ʒu���Z�b�g")]
    Vector2Int targetPos;
    
    [Tooltip("�O���b�h�}�l�[�W���[���Z�b�g")]
    GridManager gridManager;

    void Start()
    {
        // �O���b�h�}�l�[�W���[���擾
        gridManager = FindObjectOfType<GridManager>();

        // ���݂̈ʒu���Z���T�C�Y�Ŋ����Ăǂ̃}�X�ɂ���̂�����肷��
        currentPos = new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / Gl_Const.CELL_SIZE),
                                    Mathf.RoundToInt(transform.localPosition.y / Gl_Const.CELL_SIZE));
        
        // ��̈ʒu���擾
        targetPos = gridManager.FindTreasurePosition();
        
        // �ړ�����
        StartCoroutine(MoveToTarget());
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
            if (!gridManager.IsPositionValid(currentPos) || !gridManager.IsPositionValid(targetPos))
            {
                Debug.LogError("�����Ȉʒu�ł��I");
                yield break; // �R���[�`���𑁊��ɏI��
            }

            var path = AStarPathfinding(currentPos, targetPos);

            // ��Ԉُ�̏ꍇ�͏����������X�L�b�v

            if (path.Count > 1)
            {
                Vector2Int nextPos = path[1]; // ���̈ړ�����擾
                yield return MoveTo(nextPos);
                currentPos = nextPos;
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
        var end = new Vector3(nextPos.x * Gl_Const.CELL_SIZE, nextPos.y * Gl_Const.CELL_SIZE, 0);
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
    private List<Vector2Int> AStarPathfinding(Vector2Int start, Vector2Int goal)
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

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // �}���n�b�^������
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
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