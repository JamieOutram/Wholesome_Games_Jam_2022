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
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        fullText = "";
        timer = 0;
        isWriting = false;
        WriteText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
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
    }

    public void WriteText(string text)
    {
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
}
