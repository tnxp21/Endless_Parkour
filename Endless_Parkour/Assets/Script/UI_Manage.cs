using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manage : MonoBehaviour
{
    // Start is called before the first frame update
    public void SwitchMenuTo(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }
    public void StartGame() => GameManager.instance.RunBegin();
}
