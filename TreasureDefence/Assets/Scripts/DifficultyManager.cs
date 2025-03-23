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

    public enum Difficulty //難易度の列挙体
    {
        Easy,
        Nomal,
        Hard,
    }

    Text targetText;
    float speed = 1.0f;

    public Difficulty currentDifficulty;　//現在の難易度

    void Start()
    {
        Text text = this.GetComponent<Text>();
    }

    public void OnClickedButtonEasy() //Easyボタン
    {
        color();
        Invoke("setEasy", 3f);
    }
    public void OnClickedButtonNomal() //Nomalボタン
    {
        Invoke("setNomal", 3f);
    }
    public void OnClickedButtonHard() //Hardボタン
    {
        Invoke("setHard", 3f);
    }

    void setEasy()
    {
        setDifficulty(Difficulty.Easy); //難易度をEasyにセット
    }

    void setNomal()
    {
        setDifficulty(Difficulty.Nomal); //難易度をNomalにセット
    }

    void setHard()
    {
        setDifficulty(Difficulty.Hard); //難易度をHardにセット
    }

    void color()
    {
        float alpha = Mathf.PingPong(Time.time * speed, 1.0f); // 0~1を繰り返す
        Color newColor = targetText.color;
        newColor.a = alpha;
        targetText.color = newColor;
    }

    public void setDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty; //選択した難易度に変更

        switch (currentDifficulty) {
            case Difficulty.Easy: //Easyの場合
                SceneManager.LoadScene("EasyGameScene"); //難易度Easyへ
                break;

            case Difficulty.Nomal: //Nomalの場合
                SceneManager.LoadScene("NomalGameScene"); //難易度Nomalへ
                break;

            case Difficulty.Hard: //Hardの場合
                SceneManager.LoadScene("HardGameScene"); //難易度Hardへ
                break;
        }
    }

}
