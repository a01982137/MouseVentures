using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyStatue : MonoBehaviour
{
    [SerializeField] GameObject itemTag;
    public string targetObjectName;
    [SerializeField] TextMeshProUGUI popupText;
    private void OnCollisionEnter(Collision collision)
    {
        targetObjectName = itemTag.name;
        if (collision.gameObject.name == targetObjectName)
        {
            Destroy(gameObject);
            HidePopupMessage();
        }
    }
    private void HidePopupMessage()
    {
        // Deactivate the pop-up message UI element
        popupText.gameObject.SetActive(false);
    }
}
