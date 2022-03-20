using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMinigameTrigger : MonoBehaviour
{
    public string sceneName = "";

    public void Load()
    {
        Debug.Log("Called");
        TransitionBehaviour.TriggerFade(sceneName);
    }
}
