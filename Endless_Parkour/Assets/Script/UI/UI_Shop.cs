using System;
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

public class UI_Shop : MonoBehaviour
{
    [SerializeField] Transform platformColorParent;
    [SerializeField] GameObject platformColorItem;
    [SerializeField] ItemToSell[] platformColor;
    [SerializeField] Image DisplayPlatform;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in platformColor)
        {
            GameObject newItem = Instantiate(platformColorItem, platformColorParent);
            newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name;
            newItem.transform.GetChild(1).GetComponent<Image>().color = item.color;
            newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.price.ToString();
            newItem.GetComponent<Button>().onClick.AddListener(() => PurchaseColor(item.color, item.price));
        }

    }

    // Update is called once per frame
    public void PurchaseColor(Color color, int price)
    {
       // if (EnoughMoney(price))
        {
            GameManager.instance.platformHeaderColor = color;
            DisplayPlatform.color = color;
        }
    }

    private bool EnoughMoney(int price)
    {
        int playerCoins = PlayerPrefs.GetInt("Coins");
        if (playerCoins < price)
        {
            Debug.Log("You dont have enough coins!");
            return false;
        }
        int newAmountOfCoins = playerCoins - price;
        PlayerPrefs.SetInt("Coins", newAmountOfCoins);
        Debug.Log("Purchase successful");
        return true;
    }
}
