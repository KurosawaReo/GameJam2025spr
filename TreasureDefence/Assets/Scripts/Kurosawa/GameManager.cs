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

public class GameManager : MonoBehaviour
{
    //�Q�[���f�[�^.
    GameData gameData = new GameData(
        Phase.DEFENSE, //�t�F�[�Y.
        100,           //������.
        0              //�^�C�}�[.
    );
}
