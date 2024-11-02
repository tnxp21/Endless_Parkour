using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected float chanceToSpawn;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (chanceToSpawn < Random.Range(0, 100)) Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().Damage();
        }
    }
}
