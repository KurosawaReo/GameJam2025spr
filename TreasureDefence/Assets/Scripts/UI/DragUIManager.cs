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

    [Header("- prefab -")]
    [SerializeField] UIPrefab prfb;
    [SerializeField] UIAnim   anim;

    GameObject nowActionObj; //現在のアクションのobj.

    void Update()
    {
        DragUI();
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

                bool isSucsess = false;

                //盤面内である場合.
                if (x >= 0 && x < scptGridMng.width &&
                    y >= 0 && y < scptGridMng.height)
                {
                    //設置可能なマスなら.
                    if (scptGridMng.grid[x, y].tileType == TileType.EMPTY ||
                        scptGridMng.grid[x, y].tileType == TileType.RIDE_OBSTACLE)
                    {
                        Gl_Func.PlaceOnBoard(nowActionObj, x, y); //ボード座標を元に設置.
                        isSucsess = true;
                    }
                }

                //成功したなら.
                if (isSucsess)
                {
                    //成功アニメーション.
                    var obj = Instantiate(anim.circle, anim.inObj.transform);
                    Gl_Func.PlaceOnBoard(obj, x, y); //ボード座標を元に設置.
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
            case PlyAction.TEST01: 
                nowActionObj = Instantiate(prfb.test01, prfb.inObj.transform); 
                break;

            case PlyAction.TEST02: 
                nowActionObj = Instantiate(prfb.test02, prfb.inObj.transform); 
                break;

            case PlyAction.TEST03: 
                nowActionObj = Instantiate(prfb.test03, prfb.inObj.transform); 
                break;
        }
    }
}
