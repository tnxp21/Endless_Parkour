using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegdeDetection : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Player player;
    bool canDetect = true;
    BoxCollider2D boxCd => GetComponent<BoxCollider2D>();

    void Update()
    {
        if (canDetect)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, groundMask);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetect = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCd.bounds.center, boxCd.size, 0);
        // foreach (var hit in colliders){
        //  if(hit.gameObject.GetComponent<LevelGenerator>)
        // }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetect = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
