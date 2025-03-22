using Gloval;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    static public DifficultyManager/*������*/ instance;

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

    public Difficulty currentDifficulty;�@//���݂̓�Փx

    public void OnClickedButtonEasy() //Easy�{�^��
    {
        setDifficulty(Difficulty.Easy); //��Փx��Easy�ɃZ�b�g
    }
    public void OnClickedButtonNomal() //Nomal�{�^��
    {
        setDifficulty(Difficulty.Nomal); //��Փx��Nomal�ɃZ�b�g
    }
    public void OnClickedButtonHard() //Hard�{�^��
    {
        setDifficulty (Difficulty.Hard); //��Փx��Hard�ɃZ�b�g
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
