using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer _instance;
    public AudioClip[] clips;
    public AudioSource audio;

    public static MusicPlayer Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = GameObject.FindObjectOfType<MusicPlayer>();

            //Tell unity not to destroy this object when loading a new scene!
            DontDestroyOnLoad(_instance.gameObject);

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        audio = FindObjectOfType<AudioSource>();
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    private void Update()
    {
        if (audio.isPlaying) return;

        var nextClip = GetRandomClip();
        audio.clip = nextClip;
        audio.Play();
    }
}