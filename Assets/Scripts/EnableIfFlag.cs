using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnableIfFlag : MonoBehaviour
{
    public enum Flag
    {
        SHEEP,
        SHARK,
        BEAVER,
        MOUSE,
        SNAKE,
    }
    public Flag flag;
    public bool state;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<Flag, Func<bool>> flags = new Dictionary<Flag, Func<bool>>
        {
            [Flag.SHEEP] = delegate { return ProgressTracker.Instance.sheep; },
            [Flag.SHARK] = delegate { return ProgressTracker.Instance.shark; },
            [Flag.BEAVER] = delegate { return ProgressTracker.Instance.beaver; },
            [Flag.MOUSE] = delegate { return ProgressTracker.Instance.mouse; },
            [Flag.SNAKE] = delegate { return ProgressTracker.Instance.snake; },
        };

        gameObject.SetActive(flags[flag].Invoke() == state);
    }
}
