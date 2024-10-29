using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Color platformHeaderColor;
    public int coins;
    void Awake(){
        instance=this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
