using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour //really should be called player behavior but oops
{
    public float movespeed = 5; //how fast the player moves. if you change this variable, change the one in CameraScript.cs to be the same!!!!
    public GameObject dPanel; // GameObject assigned to the display panel which acts as the Text Box, used to pause player's movement when appropriate
    private NPCBehavior colObj; // A placeholder variable for NPC's that the character interacts with
    private RaycastHit hit; // for speaking to NPC
    private bool paused = false; // not actually used yet but it there
    private bool interacting = false; // used for NPC COOLDOWN so the npc isnt immediately interacted with after conversation is over
    private float timeLeft = 2.0f; // also used for NPC Cooldown
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //CHECK INTERACTION
        if (!paused && !dPanel.activeSelf && !interacting) // Interactions can only happen if the game is 1. Not Paused, 2. Textbox is not open, or 3. Not already Interacting, 
        { 
            if (Input.GetButtonDown("Fire1")) //Currently Mouse1
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 40.0f)) //Sends a ray 40.0f forward out of the player to check for NPCS
                {
                    if (hit.collider != null) //if an NPC has been interacted with, then tell it that it has been interactedWith
                    {
                        colObj = hit.collider.gameObject.GetComponent<NPCBehavior>(); //get NPC's script.
                        colObj.interactedWith = true; //See NPCBehavior for how this works
                        interacting = true; //begin NPC cooldown
                    }
                }
            }
        }
            if(interacting && !dPanel.activeSelf) //NPC COOLDOWN SCRIPT.
            { // two seconds until the player can talk to another NPC
                timeLeft -= Time.deltaTime;
                if(timeLeft <= 0.0)
                {
                    timeLeft = 2.0f;
                    interacting = false;
                }
            }


        //MOVEMENT
        if (!paused && !dPanel.activeSelf) //Only move if 1. Not Paused, 2. Textbox is Not open.
        {
            float horizontal = Input.GetAxis("Horizontal"); //Horizontal and Vertical movement axis. Uses WASD/ULDR
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(horizontal, 0.0f, vertical); //movement vector
            if (horizontal != 0 || vertical != 0) //Odd glitch where the player shuffles after stopping. Fix this if possible
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.15F);
            }

            transform.Translate(move * movespeed * Time.deltaTime, Space.World); //actually moving character
        }

    }
}
