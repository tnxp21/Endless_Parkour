using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lastScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI coinCollectedText;

    void Start()
    {
        LoadInfo();
    }

    void LoadInfo()
    {
        string tempScore;
        string temphighScore;
        GameManager.instance.LoadScore(out tempScore, out temphighScore);
        lastScoreText.text = tempScore;
        highScoreText.text = temphighScore;
        coinCollectedText.text = GameManager.instance.LoadCoins().ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
