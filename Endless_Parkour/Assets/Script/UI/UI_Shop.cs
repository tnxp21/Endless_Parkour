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


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < platformColor.Length; i++)
        {
            GameObject newItem = Instantiate(platformColorItem, platformColorParent);
            newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = platformColor[i].name;
            newItem.transform.GetChild(1).GetComponent<Image>().color = platformColor[i].color;
            newItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = platformColor[i].price.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
