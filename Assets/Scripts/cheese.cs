using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cheese : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            Destroy(gameObject);
            ProgressTracker.Instance.mouse = true;
            TransitionBehaviour.TriggerFade("Village");
        }
    }
}
