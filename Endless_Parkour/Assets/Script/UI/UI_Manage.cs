using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manage : MonoBehaviour
{
    bool pauseGame = false;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject inGameMenuUI;
    [SerializeField] GameObject endGameMenuUI;

    void Awake()
    {
        SwitchMenuTo(mainMenuUI);
    }
    void Start()
    {
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }
    public void StartGameButton()
    {
        SwitchMenuTo(inGameMenuUI);
        GameManager.instance.RunBegin();
    }

    public void Pause_PlayGameButton()
    {
        Time.timeScale = pauseGame ? 1 : 0;
        pauseGame = !pauseGame;                   //switch pause-play everytime player click the button
    }
    public void RestartButton()
    {
        GameManager.instance.RestartLevel();
    }

    public void OpenEndGameMenu()
    {
        SwitchMenuTo(endGameMenuUI);
    }
}
