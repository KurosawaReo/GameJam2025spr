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
    private int   m_coin;  //������.
    private float m_timer; //�^�C�}�[.

    private int   m_plyPieceNowCnt; //�u������̌��ݐ�.
    private int   m_plyPieceMaxCnt; //�u�����̍ő吔.

    //������(�R���X�g���N�^)
    public GameData(Phase _phase, int _coin, float _timer, int _plyPieceNowCnt, int _plyPieceMaxCnt)
    {
        m_phase = _phase;
        m_coin  = _coin;
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
    public int coin
    {
        get { return m_coin; }
        set { m_coin = value; }
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
    [SerializeField] GameObject objMidText; //��ʒ����e�L�X�g.
    [SerializeField] GameObject objDisTxt1; //�\���e�L�X�g1.
    [SerializeField] GameObject objDisTxt2; //�\���e�L�X�g2.

    DifficultyManager scptDifMng;

    //�Q�[���f�[�^.
    public GameData gameData = new GameData(
        Phase.READY, //�t�F�[�Y.
        0,           //������.
        0,           //�^�C�}�[.
        0,           //�u������̌��ݐ�.
        0            //�u�����̍ő吔.
    );

    void Start()
    {
        //��Փx�I���V�[���̏����󂯎��.
        scptDifMng = FindObjectOfType<DifficultyManager>();
        
        //��Փx��.
        switch (scptDifMng.selectDif)
        {
            case Difficulty.EASY:
                //�f�[�^�̐ݒ�.
                gameData.coin = 1000;
                gameData.plyPieceMaxCnt = 15;
                break;

            case Difficulty.NORMAL:
                //�f�[�^�̐ݒ�.
                gameData.coin = 800;
                gameData.plyPieceMaxCnt = 12;
                break;

            case Difficulty.HARD:
                //�f�[�^�̐ݒ�.
                gameData.coin = 600;
                gameData.plyPieceMaxCnt = 10;
                break;
        }

        StartCoroutine(TimePassSec()); //������s.
    }

    void Update()
    {
        //�t�F�[�Y��.
        switch (gameData.phase)
        {
            case Phase.READY:   UpdateReady();   break;
            case Phase.DEFENSE: UpdateDefense(); break;
            case Phase.RESULT:  UpdateResult();  break;

            default: break;
        }

        DisplayUI();
    }

    /// <summary>
    /// ���Ԍo��(�b�P��)
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimePassSec()
    {
        //�o�߂����b��.
        switch (gameData.timer) {

            case 0:
                DisplayMidText(0); //�\�����s.
                break;
            
            case 3:
                DisplayMidText(1); //�\�����s.
                break;

            case Gl_Const.READY_PHASE_TIME:
                DisplayMidText(2); //�\�����s.
                gameData.phase = Phase.DEFENSE;
                break;
        }
         
        yield return Gl_Func.Delay(1); //1�b�̒x��.
        gameData.timer++;              //1�b�o��.

        StartCoroutine(TimePassSec());
    }

    /// <summary>
    /// �X�V����: �����t�F�[�Y.
    /// </summary>
    private void UpdateReady()
    {
        
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
        objDisTxt1.GetComponent<Text>().text = "�R�C��: " + gameData.coin;
        objDisTxt2.GetComponent<Text>().text = "�u����: " + setAbleCnt;
    }

    /// <summary>
    /// ��ʒ����e�L�X�g�̕\��.
    /// </summary>
    /// <param name="_textNo">���Ԗڂ̃e�L�X�g��\�����邩</param>
    private void DisplayMidText(int _textNo)
    {
        //�z����I�[�o�[����ԍ�������ꂽ�Ȃ�.
        if (_textNo >= Gl_Const.MID_TEXT.Length)
        {
            Debug.LogError("[Error] DisplayMidText: _textNo value is invalid.");
        }

        //�qobj�̎擾.
        var objText = objMidText.transform.GetChild(0);

        //�e�L�X�g�̐ݒ�(Gloval����Ⴄ)
        objText.GetComponent<Text>().text = Gl_Const.MID_TEXT[_textNo];
        //�A�j���[�V�����Đ�.
        objMidText.GetComponent<Animator>().SetTrigger("AnimStart");
    }

    /// <summary>
    /// �v���C���[��ő吔�ɒB�������ǂ���.
    /// </summary>
    public bool IsPlyPieceMax()
    {
        return (gameData.plyPieceNowCnt >= gameData.plyPieceMaxCnt);
    }
    /// <summary>
    /// �v���C���[��̃J�E���g���Z.
    /// </summary>
    /// <param name="_add">���Z�l</param>
    public void AddPlyPieceCnt(int _add)
    {
        gameData.plyPieceNowCnt += _add;
    }
    /// <summary>
    /// �R�C���̉��Z.
    /// </summary>
    /// <param name="_add">���Z�l</param>
    public void AddCoin(int _add)
    {
        gameData.coin += _add;
    }
}
