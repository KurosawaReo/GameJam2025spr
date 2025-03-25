using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    static public TitleManager instance;

    bool titleMove = false;

    [SerializeField] GameObject titleButton;
    [SerializeField] GameObject title;

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

    private void Update()
    {
        if (titleMove == true)
        {
            titleButton.transform.position += Vector3.down * 3f * Time.deltaTime;
        }

        if (titleMove == true)
        {
            title.transform.position += Vector3.up * 3f * Time.deltaTime;
        }
    }
    public void OnClickedButton()
    {
        titleMove = true;
        Invoke("Difficulty", 3f);
    }

    public void Difficulty()
    {
        titleMove = false;
        SceneManager.LoadScene("DifficultyScene");
    }
}
