using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ingame : MonoBehaviour
{
    Player player;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI coinsText;

    [SerializeField] Image heartEmpty;
    [SerializeField] Image heartFull;
    [SerializeField] Image SlideCooldown;

    float distance;
    float coins;

    void Start()
    {
        player = GameManager.instance.player;
        InvokeRepeating("UpdateInfo", 0, 0.3f);
    }


    // Update is called once per frame
    void UpdateInfo()
    {
        distance = GameManager.instance.distance;
        coins = GameManager.instance.coins;
        if (distance > 0)
            distanceText.text = distance.ToString("#.0") + "  M";
        if (coins > 0)
            coinsText.text = coins.ToString();
        heartEmpty.enabled = !player.extraLife;
        heartFull.enabled = player.extraLife;
    }
    private void FixedUpdate()
    {
        SlideCooldown.color = new Color(1,1,1,1-GameManager.instance.player.GetSlideTimeCounterPercent());
    }
}
