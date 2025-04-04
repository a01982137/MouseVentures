using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class replay2 : MonoBehaviour
{
        public void RestartPlayer()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
        }

}
