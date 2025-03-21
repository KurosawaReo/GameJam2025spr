using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    int num ;

    public enum DifficultyButton
    {
        Easy,
        Nomal,
        Hard,
    }

    public DifficultyButton buttonType;

    private void Start()
    {
        num = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (num)
        {
            case 1: SceneManager.LoadScene("EasyGameSene");
            break;

            case 2: SceneManager.LoadScene("NomalGameSene");
            break;

            case 3: SceneManager.LoadScene("HardGameSene");
            break;
        }
    }

    
}
