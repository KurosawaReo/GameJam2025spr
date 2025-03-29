using Gloval;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Piece : MonoBehaviour
{
    [Tooltip("HP�c��")]
    int hp;

    [Tooltip("�U����")]
    int atk;

    [Tooltip("���݂ǂ̃}�X�ɂ���̂����Z�b�g")]
    public Vector2Int currentPos;

    [Tooltip("���ScriptableObject���Z�b�g")]
    public EntityBase pieceData;

    [Tooltip("�G���X�e�[�g���Ǘ�����p")]
    public EntityState currentState = EntityState.STOP;

    [Tooltip("�O���b�h�}�l�[�W���[���Z�b�g")]
    GridManager gridManager;

    [Tooltip("GameManager���Z�b�g")]
    GameManager gameManager;

    [Tooltip("�U���̑Ώ�")]
    GameObject target;

    [Tooltip("1�x�����������Ȃ��Ƃ���Ŏg���t���O")]
    bool isFirst;

    [Tooltip("�����Ɏg���z��")]
    Vector2Int[] targetCheckList = {
        new Vector2Int(1,1),    // ����
        new Vector2Int(0,1),    // ��
        new Vector2Int(-1,1),   // �E��
        new Vector2Int(-1,0),   // �E
        new Vector2Int(-1,-1),  // �E��
        new Vector2Int(0,-1),   // ��
        new Vector2Int(1,-1),   // ����
        new Vector2Int(-1,0),   // ��
    };

    // Start is called before the first frame update
    void Start()
    {
        // ������
        Init();
    }

    // Update is called once per frame
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
        hp = pieceData.hp;
        atk = pieceData.atk;

        // �O���b�h�}�l�[�W���[���擾
        gridManager = FindObjectOfType<GridManager>();
        // �Q�[���}�l�[�W���[���擾
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// �U�����󂯂����̏���
    /// </summary>
    /// <param name="dmg">�󂯂��_���[�W</param>
    public void Damage(int dmg)
    {
        hp -= dmg;

        // ���S�����ꍇ
        if (hp <= 0)
        {
            hp = 0;
            var index = 0;

            for (var i = 0; i < gridManager.activePieceObjList.Count; i++)
            {
                if (gridManager.activePieceObjList[i] == gameObject)
                {
                    // �������g�̏ꍇ
                    index = i;
                    break;
                }
            }

            // �擾�����C���f�b�N�X�̗v�f���폜����
            gridManager.activePieceList.RemoveAt(index);
            gridManager.activePieceObjList.RemoveAt(index);

            // ��̃J�E���g-1
            gameManager.AddPlyPieceCnt(-1);

            Destroy(gameObject); // ���g��j������
        }
    }

    /// <summary>
    /// �X�e�[�g��Stop�̎��̏���
    /// </summary>
    void StopState()
    {
        var index = 0;
        for (var i = 0; i < gridManager.activePieceObjList.Count; i++)
        {
            if (gridManager.activePieceObjList[i] == gameObject)
            {
                // �������g�̏ꍇ
                //print($"�����������܂���");
                index = i;
                break;
            }
        }

        // �����̎��͂ɓG�����邩����������
        for (var i = 0; i < targetCheckList.Length; i++)
        {
            for (var j = 0; j < gridManager.activeEnemyList.Count; j++)
            {
                //print($"targetCheckList[i]:{targetCheckList[i]}");
                //print($"gridManager.activePieceList[index] + targetCheckList[i]:{gridManager.activePieceList[index] + targetCheckList[i]}");
                //if (gridManager.activePieceList[index] + targetCheckList[i] == gridManager.activeEnemyList[j])
                //{
                //    print("�����̎��͂ɓG�����܂�");
                //}
            }
        }
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
            print("�A�^�b�N�X�e�[�g��");
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
        print("�U��");
        if (target != null)
        {
            target.GetComponent<Piece>().Damage(atk);
        }

        isFirst = true; // �U���̃X�e�[�g�Ŏg���̂Ńt���O��߂��Ă���
        currentState = EntityState.STOP; // �X�e�[�g��Move�ɕύX
    }
}
