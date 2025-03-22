using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    static public DifficultyManager/*仮名称*/ instance;

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

    public Difficulty currentDifficulty;　//現在の難易度

    public void OnClickedButtonEasy() //Easyボタン
    {
        setDifficulty(Difficulty.Easy); //難易度をEasyにセット
    }
    public void OnClickedButtonNomal() //Nomalボタン
    {
        setDifficulty(Difficulty.Nomal); //難易度をNomalにセット
    }
    public void OnClickedButtonHard() //Hardボタン
    {
        setDifficulty (Difficulty.Hard); //難易度をHardにセット
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
