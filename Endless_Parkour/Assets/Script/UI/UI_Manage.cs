using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manage : MonoBehaviour
{
    bool pauseGame = false;
    bool gameMute = false;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject inGameMenuUI;
    [SerializeField] GameObject endGameMenuUI;

    [Header("Volume")]
    [SerializeField] UI_AudioMixerSlider[] slider;
    [SerializeField] Image displaySoundButtonUIMain;
    [SerializeField] Image displaySoundButtonUIInGame;
    [SerializeField] Image soundOn;
    [SerializeField] Image soundOff;

    void Awake()
    {
        SwitchMenuTo(mainMenuUI);
    }
    void Start()
    {
        Time.timeScale = 1;
        AudioListener.volume = !gameMute ? 1 : 0;
        foreach (var item in slider) item.SetupSlider();
    }

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
    public void MuteButton()
    {
        gameMute = !gameMute;
        AudioListener.volume = !gameMute ? 1 : 0;
        displaySoundButtonUIMain.sprite = gameMute ? soundOff.sprite : soundOn.sprite;
        displaySoundButtonUIInGame.sprite = gameMute ? soundOff.sprite : soundOn.sprite;
    }
}
