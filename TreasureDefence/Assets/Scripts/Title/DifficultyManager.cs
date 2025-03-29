using Gloval;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��Փx�I����ʂ���c�葱����N���X.
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    //�V�[�����z���Ďg����悤�ɂ���ݒ�.
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

    public Difficulty selectDif { get; set; } //�I���Փx.

    //Text targetText;
    //float speed = 1.0f;

    /// <summary>
    /// Easy�{�^��.
    /// </summary>
    public void OnClickedButtonEasy()
    {
        selectDif = Difficulty.EASY;
        Invoke("SelectedDifficulty", 3f);
        
        //color();
    }
    /// <summary>
    /// Normal�{�^��.
    /// </summary>
    public void OnClickedButtonNomal()
    {
        selectDif = Difficulty.NORMAL;
        Invoke("SelectedDifficulty", 3f);
    }
    /// <summary>
    /// Hard�{�^��.
    /// </summary>
    public void OnClickedButtonHard()
    {
        selectDif = Difficulty.HARD;
        Invoke("SelectedDifficulty", 3f);
    }

    /// <summary>
    /// ��Փx�I����̏���.
    /// </summary>
    public void SelectedDifficulty()
    {
        SceneManager.LoadScene("GameScene"); //�Q�[���V�[����.
    }

#if false
    //???
    void color()
    {
        float alpha = Mathf.PingPong(Time.time * speed, 1.0f); // 0~1���J��Ԃ�
        Color newColor = targetText.color;
        newColor.a = alpha;
        targetText.color = newColor;
    }
#endif
}
