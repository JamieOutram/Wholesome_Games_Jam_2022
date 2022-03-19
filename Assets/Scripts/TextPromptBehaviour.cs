using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextPromptBehaviour : MonoBehaviour
{
    public float flashInterval = 0.25f;
    Image img;
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
        img.enabled = false;
    }

    public void Hide()
    {
        CancelInvoke(nameof(Toggle));
        img.enabled = false;
    }

    public void Show()
    {
        InvokeRepeating(nameof(Toggle), 0, flashInterval);
    }

    void Toggle()
    {
        img.enabled = !img.enabled;
    }
}
