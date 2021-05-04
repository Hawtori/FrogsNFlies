using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip boss_spawn, bullet_shoot, death, jump, trigger, gameMusic;
    public static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        boss_spawn = Resources.Load<AudioClip>("boss_spawn");
        bullet_shoot = Resources.Load<AudioClip>("bullet_shoot");
        death = Resources.Load<AudioClip>("death");
        jump = Resources.Load<AudioClip>("jump");
        trigger = Resources.Load<AudioClip>("trigger");
        gameMusic = Resources.Load<AudioClip>("gameMusic");
        
        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip) {
        if(clip == "death") audioSrc.PlayOneShot(death);
        if(clip == "jump") audioSrc.PlayOneShot(jump);
        if(clip == "bullet_shoot") audioSrc.PlayOneShot(bullet_shoot);
        if(clip == "boss_spawn") audioSrc.PlayOneShot(boss_spawn);
        if(clip == "trigger") {
            audioSrc.volume = 0.5f;
            audioSrc.PlayOneShot(trigger);
            audioSrc.volume = 1;
        }
        if(clip == "gameMusic") audioSrc.PlayOneShot(gameMusic);
    }
}
