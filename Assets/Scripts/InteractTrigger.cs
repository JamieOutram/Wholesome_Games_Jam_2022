using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractTrigger : MonoBehaviour
{
    public UnityEvent method;
    bool isInside = false;
    bool isTunePlayed = false;
    AudioSource characterTune;

    void Start()
    {
        characterTune = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Invoking Interactable");
            method.Invoke();
            if (!characterTune.isPlaying && !isTunePlayed)
            {
                characterTune.Play();
                isTunePlayed = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        isInside = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        isInside = false;
        isTunePlayed = false;
    }
}
