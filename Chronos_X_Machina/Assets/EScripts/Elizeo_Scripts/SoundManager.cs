using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* [Nava, Elizeo]
 * [January 28, 2025]
 * This is the Sound Manager script that will store ALL of the sounds in the game.
 */
public class SoundManager : MonoBehaviour
{
    //This Enum helps manage which music is playing in the background.
    public enum BGMusic
    {
        MENU,
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
    }

    public BGMusic music;

    private void MusicSetup()
    {
        switch (music)
        {
            case BGMusic.MENU:
                GameSource.clip = menuMusic;
                break;
        }
    }

    //This holds the source for the background music.
    [Header("Music Source")]
    [SerializeField] private AudioSource GameSource;

    //This holds the source for the sound effects.
    [Header("Sfx Source")]
    [SerializeField] private AudioSource SfxSource;

    [Header("Audio Clips")]
    public AudioClip menuMusic;
    //public AudioClip level1Music;
    public AudioClip selectSound;

    //this is where the game music starts to play.
    public void Start()
    {
        MusicSetup();
        GameSource.Play();
    }

    //This is a general code where any sound effect will play from.
    public void SFXPlay(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }




}
