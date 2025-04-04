using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dead : MonoBehaviour
{
    public List<Image> imagesToRemove = new List<Image>();
    private int currentIndex = 0;
    bool isCollidingThisFrame = false;
    [SerializeField] private AudioClip ouch;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(ResetCollidingFlag());
        if (isCollidingThisFrame)
        {
            Debug.Log("meow");
            return;
        }
        isCollidingThisFrame = true;
        
        if (collision.gameObject.CompareTag("Mousetrap")|| collision.gameObject.CompareTag("Death")|| collision.gameObject.CompareTag("Spider"))
        {
            Debug.Log("iv");
                // Remove the image from the UI
                if (currentIndex < imagesToRemove.Count-1)
                {
                    audioSource.PlayOneShot(ouch); // Play the pickup audio clip
                    // Remove the current image from the UI
                    imagesToRemove[currentIndex].gameObject.SetActive(false);
                    currentIndex++;
                }

          
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }


    }
    private IEnumerator ResetCollidingFlag()
    {
        yield return new WaitForEndOfFrame();
        isCollidingThisFrame = false;
    }
}
