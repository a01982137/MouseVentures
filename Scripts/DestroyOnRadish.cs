using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnRadish : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI popupText;
    public float pickupRange = 2f;
    public bool check = true;
    void Start()
    {
        popupText.gameObject.SetActive(false);
    }
    private void Update()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in nearbyColliders)
            if (collider.CompareTag("door")&&check)
            {
                DisplayPopupMessage("Press F to interact");
            }
        // Check for nearby Rigidbody objects with a specific tag
        nearbyColliders = Physics.OverlapSphere(transform.position, 2f);
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("meoww");
            foreach (Collider collider in nearbyColliders)
                {
                Debug.Log("meow1");

                if (collider.CompareTag("door"))
                    {
                    check = false;
                    Debug.Log("meow2");
                    DisplayPopupMessage("The statue seems to want a radish...");
                    break;
                    }
                }
           
        }

    }

    private void DisplayPopupMessage(string message)
    {
        // Set the pop-up message text and activate the UI element
        popupText.text = message;
        popupText.gameObject.SetActive(true);
    }


}
