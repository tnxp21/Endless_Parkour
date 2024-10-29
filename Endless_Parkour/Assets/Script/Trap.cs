using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>()!=null)
        {
            collision.GetComponent<Player>().Damage();
        }
    }
}
