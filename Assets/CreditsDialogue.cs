using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDialogue : MonoBehaviour
{
    public DialogueTextBehaviour textBehaviour;

    int pointer = 0;
    CreditsTrigger game;
    List<string> text = new List<string>(); 

    private void Start()
    {
        game = GetComponent<CreditsTrigger>();
        if (!ProgressTracker.Instance.beaver)
        {
            text.Add("A Beaver could use some cheering up.");
        }
        if (!ProgressTracker.Instance.snake)
        {
            text.Add("A Snake could use some cheering up.");
        }
        if (!ProgressTracker.Instance.mouse)
        {
            text.Add("A Mouse could use some cheering up.");
        }
        if (!ProgressTracker.Instance.sheep)
        {
            text.Add("A Sheep could use some cheering up.");
        }
        text.Add("...");
    }

    public void NextDialogue()
    {
        if (textBehaviour.isWriting)
        {
            return;
        }

        if (pointer >= text.Count)
        {
            pointer = 0;
            if (!ReferenceEquals(game, null))
            {
                game.Load();
            }
            return;
        }

        textBehaviour.WriteText(text[pointer]);
        pointer++;
    }
}
