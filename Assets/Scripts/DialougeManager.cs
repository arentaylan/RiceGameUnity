using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialougeManager : MonoBehaviour
{
    public static DialougeManager Instance { get; set; } 
    public GameObject dialougePanel; //Panel that text is printed on
    public GameObject ynPanel; //Panel for prompts
    public GameObject BattleAcivator;
    //public GameObject choicePanel; //Panel that has yes/no for prompts
    public List<string> dialougeLines = new List <string>();
    private bool talking = false; //for text scrolling
    private int txtBuff = 1; //for text scrolling
    private string prToScreen; //for text scrolling. The string currently being printed on screen.
    private bool txtDone = false; //for text scrolling. Makes sure the text finished printing before going to next box
    private int scrlBuff = 0;//for text scrolling. Increase the number on line 53 to increase time between letters printing on screen
    private string yesDialouge; //for prompts
    private string noDialouge;
    private float selCD = 0.0f; //selection cooldown.
    private float vertSel; //for selecting in prompts
    private bool lastTextBoxActive = false; //for the response textbox
    

    private bool prompt = false;//if theres a prompt in this dialouge
    private bool battle = false;//if theres a battle in this dialouge

    Text dialougeText; //placeholder for the text
    int dialougeIndex; //placeholder for navigation

    Image ynSelect; //placeholder for the selector
    private int selection; //what selection is being chosen.
    private Vector3 imageOrigPos;

    // Start is called before the first frame update
    void Awake()
    {
        dialougeText = dialougePanel.transform.Find("Text").GetComponent<Text>();
        ynSelect = ynPanel.transform.Find("ptr2").GetComponent<Image>();
        imageOrigPos = ynSelect.rectTransform.position;
        selection = 1;

        dialougePanel.SetActive(false);
        ynPanel.SetActive(false);


        if(Instance != null  && Instance != this) //catch
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && dialougeLines.Count > 0)//Continue dialouge with Mouse1
        {
            if (txtDone && !ynPanel.activeSelf && !lastTextBoxActive) //only if the dialogue has completed printing, can the next box appear
            {
                continueDialouge();
                txtDone = false;
            }
        }

  
        if(dialougePanel.activeSelf && talking) // this is some fuckshit so it's gonna be a bit of explanation
        {
            prToScreen = dialougeLines[dialougeIndex].Substring(0, txtBuff); // this variable is whats being printed to the screen.
            scrlBuff++; // Using this variable and txtBuff, the substirng is slowly expanded untl it becomes the whole string.
            if (scrlBuff >= 2) //Every [3] frames, the substring is expanded
            {
                txtBuff++; //expand substring
                scrlBuff = 0;
            }
            dialougeText.text = prToScreen; //sets text on screen to substring
            if(txtBuff > dialougeLines[dialougeIndex].Length) //is the text done expanding?
            {//then reset everything
                talking = false;
                txtBuff = 1;
                txtDone = true;
            }
            if(txtDone && dialougeIndex >= dialougeLines.Count - 1 && prompt)//time to put up a prompt
            {
                ynPanel.SetActive(true);
            }
        }
        if (ynPanel.activeSelf)
        {
            if (!lastTextBoxActive)
            { 
                vertSel = Input.GetAxis("Vertical");
                if (vertSel != 0 && selCD == 0.0f) //go select other thing
                {
                    if (selection == 1)//if at yes
                    {
                        ynSelect.rectTransform.Translate(0.0f, -30.0f, 0.0f);//go to no
                        selection = 0;
                        selCD = 0.3f;
                    }
                    else if (selection == 0)//if at no
                    {
                        ynSelect.rectTransform.position = imageOrigPos;//back to yes
                        selection = 1;
                        selCD = 0.3f;
                    }
                }
                if (selCD != 0.0f) //SWITCHING COOLDOWN SCRIPT.
                { // so it doesn't change rapidly
                    selCD -= Time.deltaTime;
                    if (selCD <= 0.0)
                    {
                        selCD = 0.0f;
                    }
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    ynPanel.SetActive(false);
                    lastTextBoxActive = true;

                }
            }
            
        }
        if (lastTextBoxActive)
        {
            if (selection == 0)
            {
                prToScreen = noDialouge.Substring(0, txtBuff); // this variable is whats being printed to the screen.
            }
            else if (selection == 1)
            {
                prToScreen = yesDialouge.Substring(0, txtBuff); // this variable is whats being printed to the screen.
            }
            scrlBuff++; // Using this variable and txtBuff, the substirng is slowly expanded untl it becomes the whole string.
            if (scrlBuff >= 2) //Every [3] frames, the substring is expanded
            {
                txtBuff++; //expand substring
                scrlBuff = 0;
            }
            dialougeText.text = prToScreen; //sets text on screen to substring
            if (selection == 1)
            {
                if (txtBuff > yesDialouge.Length) //is the text done expanding?
                {//then reset everything
                    talking = false;
                    txtBuff = 1;
                    txtDone = true;
                    lastTextBoxActive = false;
                }
            }
            else if (selection == 0)
            {
                if (txtBuff > noDialouge.Length) //is the text done expanding?
                {//then reset everything
                    talking = false;
                    txtBuff = 1;
                    txtDone = true;
                    lastTextBoxActive = false;
                }
            }
                
        }
    }

    public void AddNewDialouge(string[] lines, bool p, bool f, string yes, string no) //Called by NPC, addes text to list to print.
    {
        dialougeIndex = 0;
        dialougeLines = new List<string>(lines.Length);
        dialougeLines.AddRange(lines);

        prompt = p;
        battle = f;

        yesDialouge = yes;
        noDialouge = no;

        createDialouge();
    }

    public void createDialouge() //Actually sets the text on screen to the appropriate text
    {
        dialougeText.text = dialougeLines[dialougeIndex].Substring(0, 1);
        dialougePanel.SetActive(true);
        talking = true;
    }

    public void continueDialouge() //Continue text or stop it.
    {
        if (dialougeIndex < dialougeLines.Count - 1)
        {
            dialougeIndex++;
            dialougeText.text = dialougeLines[dialougeIndex].Substring(0, 1);
            talking = true;
        }
        else
        {
            if (!prompt && !battle) //There is no prompt or battle, so this is the end of the conversation
            {
                dialougePanel.SetActive(false);
            }
            else if(prompt && !battle) // a prompt, but not battle
            {
                dialougePanel.SetActive(false); //just a conversation so everything should be taken care of..
            }
            else if (prompt && battle) // a prompt, AND a battle
            {
                if(selection == 1)
                {
                    Debug.Log("A battle has begun!");
                    Instantiate(BattleAcivator);
                }
                dialougePanel.SetActive(false);
                //dialougePanel.SetActive(false); //everything should be taken care of..
            }
            else if (!prompt && battle)// no prompt, forced battle
            {
                Debug.Log("A battle has begun!");
                Instantiate(BattleAcivator);
                dialougePanel.SetActive(false);
                //dialougePanel.SetActive(false); //everything should be taken care of..
            }
            else //catch
            {
                dialougePanel.SetActive(false);
            }
        }
    }

}
