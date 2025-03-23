using System;
using UnityEngine;

using Gloval;

/// <summary>
/// prefabをまとめたクラス.
/// </summary>
[Serializable]
public class UIPrefab
{
    //private変数.
    [SerializeField] GameObject m_test01;
    [SerializeField] GameObject m_test02;
    [SerializeField] GameObject m_test03;
    [SerializeField] GameObject m_test04;
    [Space]
    [SerializeField] GameObject m_inObj;

    //get, set.
    public GameObject test01 
    {
        get => m_test01;
    }
    public GameObject test02
    {
        get => m_test02;
    }
    public GameObject test03
    {
        get => m_test03;
    }
    public GameObject test04
    {
        get => m_test04;
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
    [SerializeField] UIPrefab prfb;
    [SerializeField] UIAnim   anim;

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
                var (x, y) = Gl_Func.WPosToBoardPos(mPos); //ボード座標に変換.

                //設置できるなら.
                if (scptGridMng.CanPlacePiece(x, y))
                {
                    //ScriptableObjectから情報を取得.
                    scptGridMng.grid[x, y].entity = nowActionObj.GetComponent<Piece>().pieceData;
                    //駒カウント+1
                    scptGameMng.AddPlyPieceCnt(1);

                    //成功アニメーション.
                    var objAnim = Instantiate(anim.circle, anim.inObj.transform);
                    //ボード座標を元に設置.
                    Gl_Func.PlaceOnBoard(nowActionObj, x, y); //操作obj.                    
                    Gl_Func.PlaceOnBoard(objAnim, x, y);      //アニメーションobj.
                }
                else
                {
                    //失敗アニメーション.
                    var obj = Instantiate(anim.batu, anim.inObj.transform);
                    obj.transform.position = mPos;

                    Destroy(nowActionObj); //objは消去する.
                }

                nowActionObj = null; //もう操作しないためobjデータを破棄.
            }
        }
    }

    /// <summary>
    /// 各Actionボタンから列挙体を送る用.
    /// </summary>
    public void SendPlyAction(PlyAction _plyAction)
    {
        //アクション別.
        switch (_plyAction)
        {
            case PlyAction.PIECE01: 
                nowActionObj = Instantiate(prfb.test01, prfb.inObj.transform); 
                break;

            case PlyAction.PIECE02: 
                nowActionObj = Instantiate(prfb.test02, prfb.inObj.transform); 
                break;

            case PlyAction.PIECE03: 
                nowActionObj = Instantiate(prfb.test03, prfb.inObj.transform); 
                break;

            case PlyAction.PIECE04:
                nowActionObj = Instantiate(prfb.test04, prfb.inObj.transform);
                break;
        }
    }
}
