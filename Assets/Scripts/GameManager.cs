using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager gameM;
    public bool isGameOver = false; 

    public List<AudioClip> soundtrack;

    public AudioSource music;
    public AudioSource SFX;

    private void Awake()
    {
        if (gameM == null)
        {
            gameM = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameM != this)
        {
            Destroy(gameObject);
        }           
    }

    public void TogglePause()
    {
        if (music.isPlaying)
        {
            music.Pause();
        } else
        {
            music.UnPause();
        }
    }

    public void CambiarCancion(int index)
    {
        music.clip = soundtrack[index];
        music.loop = true;
        music.Play();
        StartCoroutine(FadeInMusic());
    }

    public void ReiniciarCancion()
    {
        music.Stop();
        music.Play();
    }

    private IEnumerator FadeInMusic()
    {
        float volumenOriginal = music.volume;
        music.volume = 0;
        while (music.volume < volumenOriginal)
        {
            Debug.Log(music.volume);
            music.volume += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}