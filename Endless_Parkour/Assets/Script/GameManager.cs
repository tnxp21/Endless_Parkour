using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Color platformHeaderColor;
    public int coins;

    public Player player;
    void Awake()
    {
        instance = this;
    }
    public void RestartLevel() => SceneManager.LoadScene(0);
    public void RunBegin() => player.runBegin = true;
}
