/*
   - TestSingleton.cs -

   Unity特有のシングルトンという機能.
   このscriptがアタッチされたobjを、シーンを変えても残り続ける状態にする.

   [注意点]
   このscriptをアタッチしただけでは機能しない.
   ゲームを実行し、アタッチされたobjのあるシーンに行って初めてシングルトン化される.
*/
using UnityEngine;

/// <summary>
/// シングルトン用クラス.
/// シーンを越えて保存したい情報をまとめる.
/// </summary>
public class TestSingleton : MonoBehaviour
{
    public static TestSingleton instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

