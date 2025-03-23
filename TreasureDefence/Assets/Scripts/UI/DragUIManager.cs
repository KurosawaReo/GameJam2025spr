using System;
using UnityEngine;

using Gloval;

/// <summary>
/// prefabをまとめたクラス.
/// </summary>
[Serializable]
public class PiecePrefab
{
    //private変数.
    [SerializeField] GameObject m_piece01;
    [SerializeField] GameObject m_piece02;
    [SerializeField] GameObject m_piece03;
    [SerializeField] GameObject m_piece04;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject piece01 
    {
        get => m_piece01;
    }
    public GameObject piece02
    {
        get => m_piece02;
    }
    public GameObject piece03
    {
        get => m_piece03;
    }
    public GameObject piece04
    {
        get => m_piece04;
    }
    public GameObject inObj
    {
        get => m_inObj;
    }
}

/// <summary>
/// アニメーションをまとめたクラス.
/// </summary>
[Serializable]
public class UIAnim
{
    //private変数.
    [SerializeField] GameObject m_batu;
    [SerializeField] GameObject m_circle;
    [SerializeField] GameObject m_dotCircle;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject batu
    {
        get => m_batu;
    }
    public GameObject circle
    {
        get => m_circle;
    }
    public GameObject dotCircle
    {
        get => m_dotCircle;
    }
    public GameObject inObj
    {
        get => m_inObj;
    }
}

/// <summary>
/// UIの操作.
/// </summary>
public class DragUIManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GridManager scptGridMng;
    [SerializeField] GameManager scptGameMng;

    [Header("- prefab -")]
    [SerializeField] PiecePrefab prfb;
    [SerializeField] UIAnim      anim;

    GameObject nowActionObj; //現在のアクションのobj.

    void Update()
    {
        //まだ駒を置けるなら.
        if (!scptGameMng.IsPlyPieceMax())
        {
            DragUI();
        }
    }

    /// <summary>
    /// ドラッグ処理.
    /// </summary>
    private void DragUI()
    {
        //操作objがある間(=マウスクリック中)
        if (nowActionObj != null)
        {
            var mPos = Gl_Func.GetMousePos();       //マウス座標取得.
            nowActionObj.transform.position = mPos; //移動.

            //マウスボタンを離した瞬間.
            if (Input.GetMouseButtonUp(0))
            {
                //設置可能アニメーション消去.
                EraseSetAble(); 

                var (x, y) = Gl_Func.WPosToBoardPos(mPos); //ボード座標に変換.

                //設置できるなら.
                if (scptGridMng.CanPlacePiece(x, y))
                {
                    DragSucsess(x, y); //成功処理.
                }
                else
                {
                    DragFailure(mPos); //失敗処理.
                }

                nowActionObj = null; //もう操作しないためobjデータを破棄.
            }
        }
    }

    /// <summary>
    /// ドラッグ成功.
    /// </summary>
    /// <param name="x">ボード座標x</param>
    /// <param name="y">ボード座標y</param>
    private void DragSucsess(int x, int y)
    {
        //ScriptableObjectから情報を取得.
        scptGridMng.grid[x, y].entity = nowActionObj.GetComponent<Piece>().pieceData;
        //駒カウント+1
        scptGameMng.AddPlyPieceCnt(1);

        //成功アニメーション.
        var objAnim = Instantiate(anim.circle, anim.inObj.transform);

        scptGridMng.activePieceList.Add(new Vector2Int(x, y)); // 設置した駒の位置を保持する
        scptGridMng.activePieceObjList.Add(nowActionObj); // 設置した駒のオブジェクトを保持する

        //ボード上に配置.
        Gl_Func.PlaceOnBoard(nowActionObj, x, y); //操作obj.                    
        Gl_Func.PlaceOnBoard(objAnim, x, y);      //アニメーションobj.
    }
    /// <summary>
    /// ドラッグ失敗.
    /// </summary>
    /// <param name="mPos">マウス座標</param>
    private void DragFailure(Vector2 mPos)
    {
        //失敗アニメーション.
        var obj = Instantiate(anim.batu, anim.inObj.transform);
        obj.transform.position = mPos;

        Destroy(nowActionObj); //objは消去する.
    }

    /// <summary>
    /// 各Actionボタンから列挙体を送る用.
    /// </summary>
    public void SendPlyAction(PlyAction _plyAction)
    {
        //まだ駒を置けるなら.
        if (!scptGameMng.IsPlyPieceMax())
        { 
            //アクション別.
            switch (_plyAction)
            {
                case PlyAction.PIECE01: 
                    nowActionObj = Instantiate(prfb.piece01, prfb.inObj.transform); 
                    break;

                case PlyAction.PIECE02: 
                    nowActionObj = Instantiate(prfb.piece02, prfb.inObj.transform); 
                    break;

                case PlyAction.PIECE03: 
                    nowActionObj = Instantiate(prfb.piece03, prfb.inObj.transform); 
                    break;

                case PlyAction.PIECE04:
                    nowActionObj = Instantiate(prfb.piece04, prfb.inObj.transform);
                    break;
            }

            DisSetAble();
        }
    }

    /// <summary>
    /// 設置可能マスの表示.
    /// </summary>
    private void DisSetAble()
    {
        //全マスループ.
        for (int i = 0; i < scptGridMng.width; i++) {
            for (int j = 0; j < scptGridMng.height; j++) {

                //設置可能なら.
                if (scptGridMng.CanPlacePiece(i, j))
                {
                    var objAnim = Instantiate(anim.dotCircle, anim.inObj.transform);

                    //ボード上に配置.
                    Gl_Func.PlaceOnBoard(objAnim, i, j);
                }
            }
        }
    }
    /// <summary>
    /// 設置可能マス消去.
    /// </summary>
    private void EraseSetAble()
    {
        //設置可能サインのobj全取得.
        var setAbles = GameObject.FindGameObjectsWithTag("set_able");
        //全て消去.
        foreach (var i in setAbles)
        {
            Destroy(i);
        }
    }
}
