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
            "Howdy. My name is Chip, and I am down in the dumps. No matter how hard I try, I can't make this dam. The water just flows around my logs.",
            "If you think you can manage it, I will be impressed.",
        },
        ["Happy_Beaver"] = new string[]{
            "Damn! You did it!",
            "Thank you so much! I can't wait to show off this amazing construction to the other beavers.",
        },
        ["Sad_Shark"] = new string[]{
            "The names Jawshua, but I'm not much of a shark anymore. Haven't stretched my flippers in such a long time.",
            "I wish I could swim around more, being stuck in this tiny pond is no fun.",
        },
        ["Happy_Shark"] = new string[]{
            "Wooo look at me! I can do back flips now!",
            "I don't know how this happened, but look how big the lake is now!",
        },
        ["Sad_Mouse"] = new string[]{
            "Hello, my name is David Attenburrow. I was exploring the local wildlife for my new documentary when I dropped my cheese and now I can't find it.",
            "I'm so hungry I can't even concentrate on this rare species of beetle.",
        },
        ["Happy_Mouse"] = new string[]{
            "Oh, this Wensleydale is absolutely divine! Thank you kindly.",
            "Please look out for my latest film, 'Our Cheese'."
        },
        ["Sad_Sheep"] = new string[]{
            "Hello my name is Baaabara. With all this hot weather my wool has become far too warm and my usual hairdresser is off sick.",
            "I'm overheating and Jawshua gets angry if I drink too much from his pond.",
        },
        ["Happy_Sheep"] = new string[]{
            "Ahhh that feels so much better.",
            "I can finally feel the breeze on my skin - do you take future bookings?"
        },
        ["Sad_Snake"] = new string[]{
            "Hello my name is Mrs Fangtastic. All of my children are tied up together, and I can't get them apart.",
            "I just bought them their own baskets and everything.",
        },
        ["Happy_Snake"] = new string[]{
            "Oh, look how cute they all look in their own little baskets.",
            "Thank you! If you ever need help climbing trees please let me know."
        },
        ["Sad_Kids"] = new string[]{
            "Please help us sir",
            "We can't moooove.",
        },
        ["Happy_Kids"] = new string[]{
            "Thank you sir!",
        },
    };

    string defaultDialogue = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,";

    private void Start()
    {
        game = GetComponent<AnimalMinigameTrigger>();
    }

    public void NextDialogue()
    {
        if (textBehaviour.isWriting)
        {
            return;
        }

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
