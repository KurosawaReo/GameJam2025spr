using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    static public DifficultyManager instance;


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

    public enum Difficulty //��Փx�̗񋓑�
    {
        Easy,
        Nomal,
        Hard,
    }

    Text targetText;
    float speed = 1.0f;

    public Difficulty currentDifficulty;�@//���݂̓�Փx

    void Start()
    {
        Text text = this.GetComponent<Text>();
    }

    public void OnClickedButtonEasy() //Easy�{�^��
    {
        color();
        Invoke("setEasy", 3f);
    }
    public void OnClickedButtonNomal() //Nomal�{�^��
    {
        Invoke("setNomal", 3f);
    }
    public void OnClickedButtonHard() //Hard�{�^��
    {
        Invoke("setHard", 3f);
    }

    void setEasy()
    {
        setDifficulty(Difficulty.Easy); //��Փx��Easy�ɃZ�b�g
    }

    void setNomal()
    {
        setDifficulty(Difficulty.Nomal); //��Փx��Nomal�ɃZ�b�g
    }

    void setHard()
    {
        setDifficulty(Difficulty.Hard); //��Փx��Hard�ɃZ�b�g
    }

    void color()
    {
        float alpha = Mathf.PingPong(Time.time * speed, 1.0f); // 0~1���J��Ԃ�
        Color newColor = targetText.color;
        newColor.a = alpha;
        targetText.color = newColor;
    }

    public void setDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty; //�I��������Փx�ɕύX

        switch (currentDifficulty) {
            case Difficulty.Easy: //Easy�̏ꍇ
                SceneManager.LoadScene("EasyGameScene"); //��ՓxEasy��
                break;

            case Difficulty.Nomal: //Nomal�̏ꍇ
                SceneManager.LoadScene("NomalGameScene"); //��ՓxNomal��
                break;

            case Difficulty.Hard: //Hard�̏ꍇ
                SceneManager.LoadScene("HardGameScene"); //��ՓxHard��
                break;
        }
    }

}
