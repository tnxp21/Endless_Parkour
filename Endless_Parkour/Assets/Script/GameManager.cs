using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Color platformHeaderColor;
    public int coins;
    void Awake(){
        instance=this;
    }
    public void RestartLevel()=>SceneManager.LoadScene(0);
}
