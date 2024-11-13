using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI distance;
    [SerializeField] TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        GameManager gm = GameManager.instance;
        coins.text = gm.coins.ToString();
        distance.text = gm.distance.ToString("#.0") + " M";
        score.text = gm.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
