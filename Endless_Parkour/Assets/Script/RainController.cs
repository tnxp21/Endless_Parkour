using System.Collections;
using System.Collections.Generic;
using DigitalRuby.RainMaker;
using UnityEngine;

public class RainController : MonoBehaviour
{
    RainScript2D rainController => GetComponent<RainScript2D>();
    [Range(0f, 1f)]
    [SerializeField] float intesity;
    [SerializeField] float targetIntensity;
    [SerializeField] float changingRate;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] float chanceToRain;
    [SerializeField] float rainCheckCooldown;
    float rainCheckTimer = 0;
    bool canChangeIntensity;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rainCheckTimer -=Time.deltaTime;
        rainController.RainIntensity = intesity;
        CheckForRain();
        if (canChangeIntensity) ChangeIntensity();
    }

    void CheckForRain()
    {
        if (rainCheckTimer<0)
        {
            rainCheckTimer = rainCheckCooldown;
            canChangeIntensity = true;

            if (Random.Range(0, 100) < chanceToRain)
                targetIntensity = Random.Range(minValue, maxValue);
            else targetIntensity = 0;
        }
    }
    void ChangeIntensity()
    {
        if (intesity < targetIntensity)
        {
            intesity += changingRate * Time.deltaTime;
            if (intesity >= targetIntensity)
            {
                intesity = targetIntensity;
                canChangeIntensity = false;
            }
        }
        if (intesity > targetIntensity)
        {
            intesity -= changingRate * Time.deltaTime;
            if (intesity <= targetIntensity)
            {
                intesity = targetIntensity;
                canChangeIntensity = false;
            }
        }
    }
}
