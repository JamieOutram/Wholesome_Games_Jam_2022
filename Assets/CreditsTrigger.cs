using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreditsTrigger : MonoBehaviour
{
    public string sceneName = "";
    public enum Flag
    {
        SHEEP,
        SHARK,
        BEAVER,
        MOUSE,
        SNAKE,
    }
    public void Load()
    {
        Dictionary<Flag, Func<bool>> flags = new Dictionary<Flag, Func<bool>>
        {
            [Flag.SHEEP] = delegate { return ProgressTracker.Instance.sheep; },
            [Flag.BEAVER] = delegate { return ProgressTracker.Instance.beaver; },
            [Flag.MOUSE] = delegate { return ProgressTracker.Instance.mouse; },
            [Flag.SNAKE] = delegate { return ProgressTracker.Instance.snake; },
        };
        bool isDone = true;
        foreach (var f in flags.Values)
        {
            if (!f.Invoke())
            {
                isDone = false;
            }
        }
        if (isDone)
        {
            Debug.Log("Credits!!");
            TransitionBehaviour.TriggerFade(sceneName);
        }
    }
}
