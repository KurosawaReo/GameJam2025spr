using Gloval;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 難易度選択画面から残り続けるクラス.
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    //シーンを越えて使えるようにする設定.
    public static DifficultyManager instance;
    void Awake()
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

    public Difficulty selectDif { get; set; } //選択難易度.

    //Text targetText;
    //float speed = 1.0f;

    /// <summary>
    /// Easyボタン.
    /// </summary>
    public void OnClickedButtonEasy()
    {
        selectDif = Difficulty.EASY;
        Invoke("SelectedDifficulty", 3f);
        
        //color();
    }
    /// <summary>
    /// Normalボタン.
    /// </summary>
    public void OnClickedButtonNomal()
    {
        selectDif = Difficulty.NORMAL;
        Invoke("SelectedDifficulty", 3f);
    }
    /// <summary>
    /// Hardボタン.
    /// </summary>
    public void OnClickedButtonHard()
    {
        selectDif = Difficulty.HARD;
        Invoke("SelectedDifficulty", 3f);
    }

    /// <summary>
    /// 難易度選択後の処理.
    /// </summary>
    public void SelectedDifficulty()
    {
        SceneManager.LoadScene("GameScene"); //ゲームシーンへ.
    }

#if false
    //???
    void color()
    {
        float alpha = Mathf.PingPong(Time.time * speed, 1.0f); // 0~1を繰り返す
        Color newColor = targetText.color;
        newColor.a = alpha;
        targetText.color = newColor;
    }
#endif
}
