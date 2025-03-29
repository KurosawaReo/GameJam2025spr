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
    private int   m_coin;  //所持金.
    private float m_timer; //タイマー.

    private int   m_plyPieceNowCnt; //置いた駒の現在数.
    private int   m_plyPieceMaxCnt; //置ける駒の最大数.

    //初期化(コンストラクタ)
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
/// ゲームの全体管理プログラム.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("- text -")]
    [SerializeField] GameObject objMidText; //画面中央テキスト.
    [SerializeField] GameObject objDisTxt1; //表示テキスト1.
    [SerializeField] GameObject objDisTxt2; //表示テキスト2.

    DifficultyManager scptDifMng;

    //ゲームデータ.
    public GameData gameData = new GameData(
        Phase.READY, //フェーズ.
        0,           //所持金.
        0,           //タイマー.
        0,           //置いた駒の現在数.
        0            //置ける駒の最大数.
    );

    void Start()
    {
        //難易度選択シーンの情報を受け取る.
        scptDifMng = FindObjectOfType<DifficultyManager>();
        
        //難易度別.
        switch (scptDifMng.selectDif)
        {
            case Difficulty.EASY:
                //データの設定.
                gameData.coin = 1000;
                gameData.plyPieceMaxCnt = 15;
                break;

            case Difficulty.NORMAL:
                //データの設定.
                gameData.coin = 800;
                gameData.plyPieceMaxCnt = 12;
                break;

            case Difficulty.HARD:
                //データの設定.
                gameData.coin = 600;
                gameData.plyPieceMaxCnt = 10;
                break;
        }

        StartCoroutine(TimePassSec()); //初回実行.
    }

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

        DisplayUI();
    }

    /// <summary>
    /// 時間経過(秒単位)
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimePassSec()
    {
        //経過した秒数.
        switch (gameData.timer) {

            case 0:
                DisplayMidText(0); //表示実行.
                break;
            
            case 3:
                DisplayMidText(1); //表示実行.
                break;

            case Gl_Const.READY_PHASE_TIME:
                DisplayMidText(2); //表示実行.
                gameData.phase = Phase.DEFENSE;
                break;
        }
         
        yield return Gl_Func.Delay(1); //1秒の遅延.
        gameData.timer++;              //1秒経過.

        StartCoroutine(TimePassSec());
    }

    /// <summary>
    /// 更新処理: 準備フェーズ.
    /// </summary>
    private void UpdateReady()
    {
        
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
        objDisTxt1.GetComponent<Text>().text = "コイン: " + gameData.coin;
        objDisTxt2.GetComponent<Text>().text = "置ける駒数: " + setAbleCnt;
    }

    /// <summary>
    /// 画面中央テキストの表示.
    /// </summary>
    /// <param name="_textNo">何番目のテキストを表示するか</param>
    private void DisplayMidText(int _textNo)
    {
        //配列をオーバーする番号が送られたなら.
        if (_textNo >= Gl_Const.MID_TEXT.Length)
        {
            Debug.LogError("[Error] DisplayMidText: _textNo value is invalid.");
        }

        //子objの取得.
        var objText = objMidText.transform.GetChild(0);

        //テキストの設定(Glovalから貰う)
        objText.GetComponent<Text>().text = Gl_Const.MID_TEXT[_textNo];
        //アニメーション再生.
        objMidText.GetComponent<Animator>().SetTrigger("AnimStart");
    }

    /// <summary>
    /// プレイヤー駒が最大数に達したかどうか.
    /// </summary>
    public bool IsPlyPieceMax()
    {
        return (gameData.plyPieceNowCnt >= gameData.plyPieceMaxCnt);
    }
    /// <summary>
    /// プレイヤー駒のカウント加算.
    /// </summary>
    /// <param name="_add">加算値</param>
    public void AddPlyPieceCnt(int _add)
    {
        gameData.plyPieceNowCnt += _add;
    }
    /// <summary>
    /// コインの加算.
    /// </summary>
    /// <param name="_add">加算値</param>
    public void AddCoin(int _add)
    {
        gameData.coin += _add;
    }
}
