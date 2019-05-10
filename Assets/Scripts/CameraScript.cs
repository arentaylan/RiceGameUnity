using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float movespeed = 5; // Match with PlayerMovement's Movespeed
    public GameObject dPanel; //GameObject assigned to the display panel which acts as the Text Box, used to pause movement when appropriate
    private bool paused = false; //Not implemented yet but its a pause variable
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused && !dPanel.activeSelf) // Only move if 1. Not Paused, 2. Textbox is Not open.
        {
            float horizontal = Input.GetAxis("Horizontal"); //Horizontal and Vertical movement axis. Uses WASD/ULDR
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(horizontal, 0.0f, vertical); //Movement vector
            transform.Translate(move * movespeed * Time.deltaTime, Space.World);
        }
    }
}
