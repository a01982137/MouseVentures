using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollect : MonoBehaviour
{
    int counter = 0;
    [SerializeField] TextMeshProUGUI CheeseText;
    [SerializeField] private AudioClip collect;
    private AudioSource audioSource;
    bool isCollidingThisFrame = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isCollidingThisFrame)
            return;
        isCollidingThisFrame = true;
        StartCoroutine(ResetCollidingFlag());
        if (other.gameObject.CompareTag("Cheese"))
        {
            Destroy(other.gameObject);
            counter++;
            CheeseText.text = "Cheese: " + counter;
            audioSource.PlayOneShot(collect); 
        }
    }
    private IEnumerator ResetCollidingFlag()
    {
        yield return new WaitForEndOfFrame();
        isCollidingThisFrame = false;
    }
}
