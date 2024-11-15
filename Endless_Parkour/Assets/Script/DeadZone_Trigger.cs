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
        if (collision.GetComponent<Player>()!=null && !GameManager.instance.player.GetIsDead())
        {
            AudioManager.instance.PlaySFX(4);
            GameManager.instance.player.SetIsDead(true);
            AudioManager.instance.StopAllBGM();
            StartCoroutine("delayUIAfterDead");
        }
    }

    IEnumerator delayUIAfterDead() {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1.2f);
        GameManager.instance.GameEnded();
    }
}
