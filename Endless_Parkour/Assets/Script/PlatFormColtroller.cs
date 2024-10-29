using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatFormColtroller : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] SpriteRenderer headerSr;
    void Start()
    {
        headerSr.transform.parent = transform.parent;
        headerSr.transform.localScale= new Vector3(sr.bounds.size.x, .2f);
        headerSr.transform.position = new Vector3(transform.position.x, sr.bounds.max.y-.1f);
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.GetComponent<Player>()!=null){
            headerSr.color= GameManager.instance.platformHeaderColor;
        }
    }
    
}
