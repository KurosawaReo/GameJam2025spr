using Gloval;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyPathfinding pathfinding;
    private int currentPathIndex = 0;
    private List<Vector2Int> path = new List<Vector2Int>();

    public float moveSpeed = 2.0f;  // �G�̈ړ����x
    private RectTransform rectTransform;  // UI�v�f�Ƃ��Ă�RectTransform

    public Vector2Int treasurePosition;  // ��̈ʒu
    public GridManager gridManager;  // GridManager�Q��

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();  // RectTransform�̎擾
        pathfinding = GetComponent<EnemyPathfinding>();
        gridManager = GameObject.FindObjectOfType<GridManager>();

        // �G�̐����ʒu���X�^�[�g�ʒu�Ƃ��Đݒ�
        var startPosition = new Vector2Int((int)transform.localPosition.x / Gl_Const.CELL_SIZE, (int)transform.localPosition.y / Gl_Const.CELL_SIZE);

        // �S�[���ʒu��Ֆʂ���擾
        var goalPosition = gridManager.FindTreasurePosition();

        if (goalPosition != Vector2Int.zero)
        {
            // ���[�g�T��
            pathfinding.FindPath(startPosition, goalPosition);
            path = pathfinding.GetPath();

            print($"path.Count:{path.Count}");

            // �o�H�������������m�F
            if (path.Count == 0)
            {
                Debug.LogError("�o�H��������܂���ł����I�X�^�[�g�n�_: " + startPosition + " �S�[���n�_: " + goalPosition);
            }
            else
            {
                print("�o�H��������܂����I�o�H��: " + path.Count);
                // �ŏ��̈ʒu�Ɉړ�
                rectTransform.anchoredPosition = new Vector2(path[0].x, path[0].y);  // RectTransform�̈ʒu��ݒ�
                currentPathIndex = 1;
                StartCoroutine(MoveAlongPath()); // �R���[�`�����J�n
            }
        }
        else
        {
            Debug.LogError("�S�[���ʒu��������܂���ł����I");
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (currentPathIndex < path.Count)
        {
            var targetPosition = new Vector2(path[currentPathIndex].x, path[currentPathIndex].y);

            print($"targetPosition:{targetPosition}");

            // �^�[�Q�b�g�Ɍ������Ĉړ�
            while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0.1f)
            {
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;  // 1�t���[���ҋ@
            }

            currentPathIndex++;  // ���̃|�C���g�֐i��
        }
    }
}
