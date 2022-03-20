using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextBehaviour : MonoBehaviour
{
    Text text;
    string fullText;
    [Range(0.01f,0.2f)]
    public float writeSpeed;
    int pointer;
    float timer;
    public bool isWriting;
    public TextPromptBehaviour prompt;
    public Image backing;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        fullText = "";
        timer = 0;
        isWriting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWriting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Skip();
            }
            else
            {
                UpdateText();
            }
        }
        else if (backing.enabled)
        {
            //If shown and end of dialogue
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
            {
                Hide();
            }
        }
    }

    public void WriteText(string text)
    {
        Show();
        prompt.Hide();
        fullText = text;
        this.text.text = "";
        pointer = 0;
        timer = 0;
        isWriting = true;
    }

    void UpdateText()
    {
        timer += Time.deltaTime;
        if(timer < writeSpeed)
        {
            return;
        }

        if (pointer != fullText.Length)
        {
            this.text.text += fullText[pointer];
            pointer++;
            timer = 0;
        }
        else
        {
            isWriting = false;
            prompt.Show();
        }
    }
    
    void Skip()
    {
        isWriting = false;
        this.text.text = fullText;
        prompt.Show();
    }


    void Hide()
    {
        backing.enabled = false;
        text.enabled = false;
        prompt.Hide();
    }
    void Show()
    {
        backing.enabled = true;
        text.enabled = true;
    }
}
