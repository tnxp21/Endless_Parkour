using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemToSell
{
    public string name;
    public Color color;
    public int price;
}

public enum ColorType
{
    platformColor,
    playerColor
}

public class UI_Shop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notifyText;
    [Header("Platform color")]
    [SerializeField] Transform platformColorParent;
    [SerializeField] GameObject platformColorItem;
    [SerializeField] ItemToSell[] platformColors;
    [SerializeField] Image displayPlatform;

    [Header("Player color")]
    [SerializeField] Transform playerColorParent;
    [SerializeField] GameObject playerColorItem;
    [SerializeField] Image displayPlayer;
    [SerializeField] ItemToSell[] playerColors;

    [Header("Coins")]
    [SerializeField] TextMeshProUGUI totalCoins;

    // Start is called before the first frame update
    void Start()
    {
        totalCoins.text = GameManager.instance.LoadCoins().ToString();
        foreach (var item in platformColors)
        {
            GameObject newItem = Instantiate(platformColorItem, platformColorParent);
            newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            newItem.transform.GetChild(1).GetComponent<Image>().color = item.color;
            newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.price.ToString();
            newItem.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(item.color, item.price, ColorType.platformColor));
        }
        foreach (var item in playerColors)
        {
            GameObject newItem = Instantiate(playerColorItem, playerColorParent);
            newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            newItem.transform.GetChild(1).GetComponent<Image>().color = item.color;
            newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.price.ToString();
            newItem.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(item.color, item.price, ColorType.playerColor));
        }

    }

    // Update is called once per frame
    public void PurchaseColor(Color color, int price, ColorType colorType)
    {
        if (!EnoughMoney(price)){
            StartCoroutine(Notify("Not enough money!", 1));
            return;
        }
        if (colorType == ColorType.platformColor)
        {
            GameManager.instance.platformHeaderColor = color;
            displayPlatform.color = color;
            GameManager.instance.SaveColor(color, "PlatformColor");
        }
        else
        {
            GameManager.instance.sr.color = color;
            displayPlayer.color = color;
            GameManager.instance.SaveColor(color, "PlayerColor");
        }
        StartCoroutine(Notify("Purchase succesful!!", 1f));
    }

    private bool EnoughMoney(int price)
    {
        int playerCoins = PlayerPrefs.GetInt("Coins");
        if (playerCoins < price) return false;
        int newAmountOfCoins = playerCoins - price;
        PlayerPrefs.SetInt("Coins", newAmountOfCoins);
        totalCoins.text = newAmountOfCoins.ToString();
        return true;
    }
    IEnumerator Notify(string text, float seconds)
    {
        notifyText.text = text;
        yield return new WaitForSeconds(seconds);
        notifyText.text = "Tap to buy";
    }
}
