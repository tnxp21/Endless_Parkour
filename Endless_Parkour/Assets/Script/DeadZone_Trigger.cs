using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone_Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()!=null)
        {
            GameManager.instance.GameEnded();       
        }
    }
}
