using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    private int bgmIndex = 0;
    // Start is called before the first frame update
    private void Awake() => instance = this;

    void Update()
    {
        if (GameManager.instance.player.GetIsDead()) return;
        if (!bgm[bgmIndex].isPlaying) {PlayRandomBGM();}
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0,bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int index)
    {
        for (int i = 0; i < bgm.Length; i++)
            if (bgm[i].isPlaying) bgm[i].Stop();  
        bgm[index].Play();
    }

    public void StopAllBGM()
    {
        foreach (var i in bgm) i.Stop();
    }

    public void PlaySFX(int index)
    {
        if (index >= sfx.Length) return;
        sfx[index].Play();
        sfx[index].pitch = Random.Range(.85f, 1.1f);
    }

    // Update is called once per frame
}
