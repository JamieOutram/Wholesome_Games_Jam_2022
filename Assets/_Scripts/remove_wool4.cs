using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class remove_wool4 : MonoBehaviour
{
    private float wool = 0; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "wool")
        {
            wool++;
            Destroy(other.gameObject);
            if (wool == 37)
            {
                SceneManager.LoadScene("SheepMinigame1");
            }
        }
    }
}
