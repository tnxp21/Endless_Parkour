using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    int amountOfCoins;
    [SerializeField] int minQuantity;
    [SerializeField] int maxQuantity;

    void Start()
    {
        amountOfCoins = Random.Range(minQuantity, maxQuantity);
        for (int i = 0; i < amountOfCoins; i++)
        {
            Vector3 offset = new(i - amountOfCoins / 2, 0);
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
