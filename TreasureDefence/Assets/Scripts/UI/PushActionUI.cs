using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Gloval;

/// <summary>
/// ボタンに共通でつけるscript.
/// </summary>
public class PushActionUI : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] DragUIManager scptDragUI;

    [Header("- value -")]
    [SerializeField] PlyAction action;

    void Awake()
    {
        /*
           注意点:

           1.using UnityEngine.EventSystems;の導入が必要
           2.Unity上でEventTriggerのコンポーネントを追加する必要がある(nullエラーになる)
        */

        EventTrigger.Entry ev = new EventTrigger.Entry();         //イベント作成.
        ev.eventID = EventTriggerType.PointerDown;                //何のイベントか.
        ev.callback.AddListener((data) => { MouseClickDown(); }); //実行させたい関数の登録.

        //コンポーネント取得.
        var cmpEv = GetComponent<EventTrigger>();
        //コンポーネントが存在するなら.
        if (cmpEv != null)
        {
            cmpEv.triggers.Add(ev); //script上で反映.
        }
        else
        {
            Debug.LogError($"[Error] {gameObject.name} にEventTriggerコンポーネントを追加してください");
        }
    }

    /// <summary>
    /// マウスのクリック押下感知用.
    /// </summary>
    public void MouseClickDown()
    {
        scptDragUI.SendPlyAction(action);
    }
}
