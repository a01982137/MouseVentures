using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public AudioClip[] musicClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; // Enable looping
        DontDestroyOnLoad(gameObject); // Keep the manager object alive across scene changes
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < musicClips.Length)
        {
            audioSource.clip = musicClips[sceneIndex];
            audioSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBackgroundMusic();
    }
}
