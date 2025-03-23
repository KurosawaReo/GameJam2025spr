using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gloval;
using UnityEngine.UI;

/// <summary>
/// ゲーム全体で使うデータ.
/// </summary>
public class GameData
{
    //private変数.
    private Phase m_phase; //フェーズ.
    private int   m_money; //所持金.
    private float m_timer; //タイマー.

    private int   m_plyPieceNowCnt; //置いた駒の現在数.
    private int   m_plyPieceMaxCnt; //置ける駒の最大数.

    //初期化(コンストラクタ)
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
/// ゲームの全体管理プログラム.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("- text -")]
    [SerializeField] GameObject objDisTxt1; //表示テキスト1.
    [SerializeField] GameObject objDisTxt2; //表示テキスト2.

    //ゲームデータ.
    public GameData gameData = new GameData(
        Phase.DEFENSE, //フェーズ.
        0,             //所持金.
        0,             //タイマー.
        0,             //置いた駒の現在数.
        15              //置ける駒の最大数.
    );

    void Update()
    {
        //フェーズ別.
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

    /// <summary>
    /// UIの表示.
    /// </summary>
    private void DisplayUI()
    {
        //置ける駒の残り数.
        int setAbleCnt = gameData.plyPieceMaxCnt - gameData.plyPieceNowCnt;

        //テキスト内容.
        objDisTxt1.GetComponent<Text>().text = "なにか";
        objDisTxt2.GetComponent<Text>().text = "置ける駒数: " + setAbleCnt;
    }

    /// <summary>
    /// プレイヤー駒のカウント加算.
    /// </summary>
    public void AddPlyPieceCnt(int _add)
    {
        gameData.plyPieceNowCnt += _add;
    }
    /// <summary>
    /// プレイヤー駒が最大数に達したかどうか.
    /// </summary>
    public bool IsPlyPieceMax()
    {
        return (gameData.plyPieceNowCnt >= gameData.plyPieceMaxCnt);
    }
}
