using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gloval;

/// <summary>
/// ゲーム全体で使うデータ.
/// </summary>
public class GameData
{
    //private変数.
    private Phase m_phase; //フェーズ.
    private int   m_money; //所持金.
    private float m_timer; //タイマー.

    //初期化(コンストラクタ)
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
/// ゲームの全体管理プログラム.
/// </summary>
public class GameManager : MonoBehaviour
{
    //ゲームデータ.
    GameData gameData = new GameData(
        Phase.DEFENSE, //フェーズ.
        0,             //所持金.
        0              //タイマー.
    );

    void Update()
    {
        //フェーズ別.
        switch (gameData.phase) 
        { 
            case Phase.READY:   UpdateReady();   break;
            case Phase.DEFENSE: UpdateDefense(); break;
            case Phase.RESULT:  UpdateResult();  break;

            default: break;
        }
    }

    /// <summary>
    /// 更新処理: 準備フェーズ.
    /// </summary>
    private void UpdateReady()
    {
        //1秒で+1.
        gameData.timer += Time.deltaTime;

        //準備フェーズ終了.
        if (gameData.timer >= Gl_Const.READY_PHASE_TIME)
        {
            gameData.phase = Phase.DEFENSE;
            gameData.timer = 0;
        }
    }
    /// <summary>
    /// 更新処理: 防御フェーズ.
    /// </summary>
    private void UpdateDefense()
    {

    }
    /// <summary>
    /// 更新処理: リザルトフェーズ.
    /// </summary>
    private void UpdateResult()
    {

    }
}
