using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gloval;

/// <summary>
/// �Q�[���S�̂Ŏg���f�[�^.
/// </summary>
public class GameData
{
    //private�ϐ�.
    private Phase m_phase; //�t�F�[�Y.
    private int   m_money; //������.
    private float m_timer; //�^�C�}�[.

    //������(�R���X�g���N�^)
    public GameData(Phase _phase, int _money, float _timer)
    {
        m_phase = _phase;
        m_money = _money;
        m_timer = _timer;
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

}

/// <summary>
/// �Q�[���̑S�̊Ǘ��v���O����.
/// </summary>
public class GameManager : MonoBehaviour
{
    //�Q�[���f�[�^.
    GameData gameData = new GameData(
        Phase.DEFENSE, //�t�F�[�Y.
        0,             //������.
        0              //�^�C�}�[.
    );

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
}
