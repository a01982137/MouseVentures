using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    private bool isCarryingObject = false;
    private Rigidbody objectToPickup;
    private Collider[] objectColliders;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private AudioClip pickupAudioClip;
    [SerializeField] private AudioClip dropAudioClip;
    private AudioSource audioSource;
    public float pickupRange = 1f;

    void Start()
    {
        popupText.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        popupText.gameObject.SetActive(false);
        if (!isCarryingObject)
        {
            // Check for nearby Rigidbody objects with a specific tag
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 1f);
            foreach (Collider collider in nearbyColliders)
            {
                if (collider.CompareTag("Pickupable"))
                {
                    DisplayPopupMessage("Press E to pickup", GetComponent<Collider>().transform);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isCarryingObject)
            {
                // Check for nearby Rigidbody objects with a specific tag
                Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 1f);
                foreach (Collider collider in nearbyColliders)
                {
                    if (collider.CompareTag("Pickupable"))
                    {
                        // Pick up the Rigidbody object
                        objectToPickup = collider.GetComponent<Rigidbody>();
                        objectColliders = objectToPickup.GetComponentsInChildren<Collider>();
                        objectToPickup.isKinematic = true;
                        objectToPickup.transform.SetParent(transform);

                        foreach (Collider coll in objectColliders)
                        {
                            coll.enabled = false;
                        }

                        isCarryingObject = true;
                        HidePopupMessage();
                        audioSource.PlayOneShot(pickupAudioClip); // Play the pickup audio clip
                        break;
                    }
                }
            }
            else
            {
                // Place down the Rigidbody object
                objectToPickup.isKinematic = false;
                objectToPickup.transform.SetParent(null);
                foreach (Collider coll in objectColliders)
                {
                    coll.enabled = true;
                }

                objectToPickup = null;
                isCarryingObject = false;
                audioSource.PlayOneShot(dropAudioClip); // Play the drop audio clip
            }
        }
    }

    private void DisplayPopupMessage(string message, Transform objectTransform)
    {
        // Set the pop-up message text and activate the UI element
        popupText.text = message;
        popupText.gameObject.SetActive(true);

        // Position the popup message near the object
        Vector3 popupPosition = Camera.main.WorldToScreenPoint(objectTransform.position);
        popupText.rectTransform.position = popupPosition;
    }

    private void HidePopupMessage()
    {
        // Deactivate the pop-up message UI element
        popupText.gameObject.SetActive(false);
    }
}
