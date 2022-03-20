using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapBackground : MonoBehaviour
{
    public SpriteRenderer renderer;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        if (ProgressTracker.Instance.beaver)
        {
            renderer.sprite = sprite;
        }
    }
}
