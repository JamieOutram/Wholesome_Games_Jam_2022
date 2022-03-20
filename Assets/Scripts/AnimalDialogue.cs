using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDialogue : MonoBehaviour
{
    public DialogueTextBehaviour textBehaviour;
    
    public string dialogueKey = "Sad_Shark";
    int pointer = 0;
    AnimalMinigameTrigger game;

    Dictionary<string, string[]> dialogue = new Dictionary<string, string[]>{ 
        ["Sad_Beaver"] = new string[]{
            "I am a sad Beaver", 
            "Really Very Sad",
        },
        ["Happy_Beaver"] = new string[]{
            "I am a happy Beaver",
            "Super Duper Happy!",
        },
        ["Sad_Shark"] = new string[]{
            "I am a sad Shark",
            "Really Very Sad",
        },
        ["Happy_Shark"] = new string[]{
            "I am a happy Shark",
            "Super Duper Happy!",
        },
    };

    string defaultDialogue = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,";

    private void Start()
    {
        game = GetComponent<AnimalMinigameTrigger>();
    }

    public void NextDialogue()
    {
        string[] text;
        if (dialogue.ContainsKey(dialogueKey))
        {
            text = dialogue[dialogueKey];
        }
        else
        {
            text = new string[] { defaultDialogue };
        }


        if (pointer >= text.Length)
        {
            pointer = 0; 
            if(!ReferenceEquals(game, null))
            {
                game.Load();
            }
            return;
        }
        textBehaviour.WriteText(text[pointer]);
        pointer++;
    }
}
