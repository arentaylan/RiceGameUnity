using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public NPCclass NPCData; // Abstract Class where all the neccessary NPC Data will be stored. 
    public bool interactedWith = false; // Used to send dialouge to dialouge manager

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(interactedWith)//When the player tells an NPC it's been interacted with, it will send it's dialouge to the dialouge manager to print on screen.
        {
            Environment.fightingThis = NPCData;
            Interact();
            interactedWith = false; // NPC only needs to send ONCE since it's text is a LIST
        }
    }

    public void Interact()
    {
        DialougeManager.Instance.AddNewDialouge(NPCData.text, NPCData.prompt, NPCData.battle, NPCData.yesText, NPCData.noText); //Send text list to dialougeManager
    }
}
