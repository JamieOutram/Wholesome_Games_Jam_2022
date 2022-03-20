using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDialogue : MonoBehaviour
{
    public DialogueTextBehaviour textBehaviour;
    public string dialogueKey = "Sad_Shark";
    int pointer = 0;

    Dictionary<string, string[]> dialogue = new Dictionary<string, string[]>{ 
        ["Sad_Beaver"] = new string[]{
            "I am a sad Beaver", 
            "Really Very Sad",
        },
        ["Happy_Beaver"] = new string[]{
            "I am a Happy Beaver",
            "Super Duper Happy!",
        },
        ["Sad_Shark"] = new string[]{
            "I am a sad Shark",
            "Really Very Sad",
        },
        ["Happy_Shark"] = new string[]{
            "I am a sad Beaver",
            "Super Duper Happy!",
        },
    };

    string defaultDialogue = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,";

    public void NextDialogue()
    {
        if (pointer >= dialogue[dialogueKey].Length)
        {
            pointer = 0; 
            return;
        }
        textBehaviour.WriteText(dialogue[dialogueKey][pointer]);
        pointer++;
    }
}
