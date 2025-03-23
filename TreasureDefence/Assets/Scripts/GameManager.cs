using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gloval;
using UnityEngine.UI;

/// <summary>
/// �Q�[���S�̂Ŏg���f�[�^.
/// </summary>
public class GameData
{
    //private�ϐ�.
    private Phase m_phase; //�t�F�[�Y.
    private int   m_money; //������.
    private float m_timer; //�^�C�}�[.

    private int   m_plyPieceNowCnt; //�u������̌��ݐ�.
    private int   m_plyPieceMaxCnt; //�u�����̍ő吔.

    //������(�R���X�g���N�^)
    public GameData(Phase _phase, int _money, float _timer, int _plyPieceNowCnt, int _plyPieceMaxCnt)
    {
        m_phase = _phase;
        m_money = _money;
        m_timer = _timer;

        m_plyPieceNowCnt = _plyPieceNowCnt;
        m_plyPieceMaxCnt = _plyPieceMaxCnt;
    }

    //get, set
    public Phase phase 
    { 
        get { return m_phase; }
        set { m_phase = value; }
    }
    public int money
    {
        get { return m_money; }
        set { m_money = value; }
    }
    public float timer
    {
        get { return m_timer; }
        set { m_timer = value; }
    }
    public int plyPieceNowCnt
    {
        get { return m_plyPieceNowCnt; }
        set { m_plyPieceNowCnt = value; }
    }
    public int plyPieceMaxCnt
    {
        get { return m_plyPieceMaxCnt; }
        set { m_plyPieceMaxCnt = value; }
    }
}

/// <summary>
/// �Q�[���̑S�̊Ǘ��v���O����.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("- text -")]
    [SerializeField] GameObject objDisTxt1; //�\���e�L�X�g1.
    [SerializeField] GameObject objDisTxt2; //�\���e�L�X�g2.

    //�Q�[���f�[�^.
    public GameData gameData = new GameData(
        Phase.DEFENSE, //�t�F�[�Y.
        0,             //������.
        0,             //�^�C�}�[.
        0,             //�u������̌��ݐ�.
        15              //�u�����̍ő吔.
    );

    void Update()
    {
        //�t�F�[�Y��.
        switch (gameData.phase)
        {
            case Phase.READY: UpdateReady(); break;
            case Phase.DEFENSE: UpdateDefense(); break;
            case Phase.RESULT: UpdateResult(); break;

            default: break;
        }

        DisplayUI();
    }

    /// <summary>
    /// �X�V����: �����t�F�[�Y.
    /// </summary>
    private void UpdateReady()
    {
        //1�b��+1.
        gameData.timer += Time.deltaTime;

        //�����t�F�[�Y�I��.
        if (gameData.timer >= Gl_Const.READY_PHASE_TIME)
        {
            gameData.phase = Phase.DEFENSE;
            gameData.timer = 0;
        }
    }
    /// <summary>
    /// �X�V����: �h��t�F�[�Y.
    /// </summary>
    private void UpdateDefense()
    {

    }
    /// <summary>
    /// �X�V����: ���U���g�t�F�[�Y.
    /// </summary>
    private void UpdateResult()
    {

    }

    /// <summary>
    /// UI�̕\��.
    /// </summary>
    private void DisplayUI()
    {
        //�u�����̎c�萔.
        int setAbleCnt = gameData.plyPieceMaxCnt - gameData.plyPieceNowCnt;

        //�e�L�X�g���e.
        objDisTxt1.GetComponent<Text>().text = "�Ȃɂ�";
        objDisTxt2.GetComponent<Text>().text = "�u����: " + setAbleCnt;
    }

    /// <summary>
    /// �v���C���[��̃J�E���g���Z.
    /// </summary>
    public void AddPlyPieceCnt(int _add)
    {
        gameData.plyPieceNowCnt += _add;
    }
    /// <summary>
    /// �v���C���[��ő吔�ɒB�������ǂ���.
    /// </summary>
    public bool IsPlyPieceMax()
    {
        return (gameData.plyPieceNowCnt >= gameData.plyPieceMaxCnt);
    }
}
