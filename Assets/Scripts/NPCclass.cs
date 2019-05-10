using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDATA", menuName = "Custom/Create NPC Data", order = 1)]
public class NPCclass : ScriptableObject
{
    public string NPCName = "Default"; //The name of the npc, for organizational purposes
    public bool prompt = false; //if battle is false, the NPC iwll not ask a question
    public bool battle = false; //if battle is false, the NPC doesnt want to fight you/vice versa
    public string[] text = { "This is the default conversation. What's up!" }; //What the NPC actually says.
    public string yesText = "Default Yes Text";
    public string noText = "Defualt No Text";
    //If the NPC should want to battle you, then he will have a deck to fight you with that will be sent to
    //the dialouge manager when it activates the battle scene.
    public List<CardClass> Deck;
}
