using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject forestBG;
    public GameObject desertBG;

    public NPCclass NPC;

    public int p1Life = 5;
    public int p2Life = 5;

    public List<CardClass> pDeck;
    public List<CardClass> eDeck;

    public List<CardClass> pHand;
    public List<CardClass> eHand;

    public GameObject p1Left;
    public GameObject p1Right;
    public GameObject p1M;
    public GameObject p2Left;
    public GameObject p2Right;
    public GameObject p2M;

    public GameObject p1MonsterObj;
    public GameObject p2MonsterObj;

    public Image cardImage;
    public CardClass c1Left = null;
    public CardClass c1Right = null;
    public CardClass c1M = null;
    public CardClass c2Left = null;
    public CardClass c2Right = null;
    public CardClass c2M = null;


    public List<GameObject> displayHand;
    private float cardSel;
    private float cardSelCD = 0.0f;
    public List<CardClass> activeCards;
    private int selection;
    private float minHandX;
    private float maxHandX;

    public GameObject questionPanel;

    private float phaseCD = 0.0f;
    public float phaseCDmax = 5.0f; //change this number to lengthen or shorten phase cooldowns
    public bool paused = false;


    public GameObject battleCanvas;

    public Text debugText1;
    public Text debugText2;
    public Text debugTextMiddle;

    private activePlayer whoPlays = activePlayer.Player;
    private Phase curPhase = Phase.Draw;
    private GameObject BG;
    private string place = Environment.CurrentEnvironment;
    
    // Start is called before the first frame update
    void Awake()
    {
        NPC = Environment.fightingThis;
        Vector3 spwnPos = new Vector3(0, 0, 0);

        Transform child = battleCanvas.transform.Find("debug1");
        debugText1 = child.GetComponent<Text>();
        debugText1.text = "LIFE = " + p2Life + " \nMonster = N/A";

        child = battleCanvas.transform.Find("debug2");
        debugText2 = child.GetComponent<Text>();
        debugText2.text = "LIFE = " + p1Life + " \nMonster = N/A"; ;

        child = battleCanvas.transform.Find("phase");
        debugTextMiddle = child.GetComponent<Text>();
        debugTextMiddle.text = "DEBUG TEXT TEST";

        /*GameObject  bTexObj = new GameObject("myTextGO");
        bTexObj.transform.SetParent(battleCanvas.transform);

        Text debugText1 = bTexObj.AddComponent<Text>();
        debugText1.text = "DEBUGTEXT 1";
        bTexObj.SetActive(true);
        bTexObj.transform.position = new Vector3(0.0f, 0.0f, 0.0f); */

        //set the background/gorund of the field.
        if (place == "Field")
        {
           BG = Instantiate(forestBG);
        }
        else if(place == "Desert")
        {
           BG = Instantiate(desertBG);
        }
        BG.transform.position = spwnPos;

        var a = 0; //set up each card deck
        foreach (CardClass card in NPC.Deck)
        {
            eDeck.Add(card);
            pDeck.Add(card);
            a++;
        }

        //shuffle decks
        for (int i = 0; i < pDeck.Count; i++)
        {
            CardClass tem = pDeck[i];
            int randomIndex = Random.Range(i, pDeck.Count);
            pDeck[i] = pDeck[randomIndex];
            pDeck[randomIndex] = tem;
        }
        for (int i = 0; i < eDeck.Count; i++)
        {
            CardClass tem = eDeck[i];
            int randomIndex = Random.Range(i, eDeck.Count);
            eDeck[i] = eDeck[randomIndex];
            eDeck[randomIndex] = tem;
        }
        //disply hand
        pHand.Add(pDeck[0]);
        var temp = pHand[0];
        pDeck.Remove(temp);
        pHand.Add(pDeck[1]);
        temp = pHand[1];
        pDeck.Remove(temp);
        pHand.Add(pDeck[2]);
        temp = pHand[2];
        pDeck.Remove(temp);
        pHand.Add(pDeck[3]);
        temp = pHand[3];
        pDeck.Remove(temp);
        pHand.Add(pDeck[4]);
        temp = pHand[4];
        pDeck.Remove(temp);

        eHand.Add(eDeck[0]);
        temp = eHand[0];
        eDeck.Remove(temp);
        eHand.Add(eDeck[1]);
        temp = eHand[1];
        eDeck.Remove(temp);
        eHand.Add(eDeck[2]);
        temp = eHand[2];
        eDeck.Remove(temp);
        eHand.Add(eDeck[3]);
        temp = eHand[3];
        eDeck.Remove(temp);
        eHand.Add(eDeck[4]);
        temp = eHand[4];
        eDeck.Remove(temp); 

        a = 0; 
        foreach (CardClass card in pHand)
        {
            //card image is an Image, cardObject is the GameObject
            GameObject cardObject = new GameObject();
            displayHand.Add(Instantiate(cardObject, battleCanvas.transform));
            cardImage = displayHand[a].AddComponent<Image>();
            cardImage.sprite = pHand[a].cardArt;
            displayHand[a].SetActive(true);
            Destroy(cardObject);
            a++;
        }
        a = 0;
        float f = 0.0f;
        //render hand
        minHandX = 200.0f;
        maxHandX = 800.0f;
        foreach (GameObject cimage in displayHand)
        {
            cimage.transform.position = new Vector3(minHandX+ (maxHandX / (displayHand.Count) * f), f, 0.0f);
            f+=1.0f;
            //
        }


    }

    // Update is called once per frame
    void Update()
    {
        cardSel = Input.GetAxis("Horizontal");
        var f = 0.0f;
        debugTextMiddle.text = " Turn: " + whoPlays + " Phase: " + curPhase;
        if (c1M != null)
        {
            debugText2.text = "LIFE = " +p1Life + " \nMonster = " + c1M.CardName + "\n HP: " + c1M.Health + " ATK: " + c1M.Attack;
            if(c1M.Health <= 0)
            {
                Destroy(p1MonsterObj.GetComponent<Transform>());
                Destroy(p1MonsterObj);
                activeCards.Remove(c1M);
                c1M = null;
                p1Life--;
                debugText2.text = "LIFE = " + p1Life + " \nMonster = N/A";
            }
        }
        if (c2M != null)
        {
            debugText1.text = "LIFE = " +p2Life + " \nMonster = " + c2M.CardName + "\n HP: " + c2M.Health + " ATK: " + c2M.Attack;

            if (c2M.Health <= 0)
            {
                Destroy(p2MonsterObj.GetComponent<Transform>());
                Destroy(p2MonsterObj);
                activeCards.Remove(c2M);
                c2M = null;
                p2Life--;
                debugText1.text = "LIFE = " + p2Life + " \nMonster = N/A";
            }

        }

        if (phaseCD != 0.0f) //PHASE COOLDOWN SCRIPT.
        { // so it doesn't change rapidly
            phaseCD -= Time.deltaTime;
            if (phaseCD <= 0.0)
            {
                phaseCD = 0.0f;
                paused = false;
            }
        }


        //PLAYER
        if (whoPlays == activePlayer.Player)
        {
            if (curPhase == Phase.Draw)
            {
                if (!paused)
                { 
                    pHand.Add(pDeck[0]);
                    var temp = pHand[pHand.Count - 1];
                    pDeck.Remove(temp);

                    checkEffects(Phase.Draw);
                    curPhase = Phase.Placement;
                    phaseCD = phaseCDmax;
                    selection = 0;

                    GameObject cardObject = new GameObject();
                    displayHand.Add(Instantiate(cardObject, battleCanvas.transform));
                    cardImage = displayHand[displayHand.Count - 1].AddComponent<Image>();
                    cardImage.sprite = pHand[displayHand.Count - 1].cardArt;
                    displayHand[displayHand.Count - 1].SetActive(true);
                    Destroy(cardObject);
                    foreach (GameObject cimage in displayHand)
                    {
                        cimage.transform.position = new Vector3(minHandX + (maxHandX / (displayHand.Count) * f), f, 0.0f);
                        f += 1.0f;
                        //
                    }
                    displayHand[selection].transform.position = new Vector3(displayHand[selection].transform.position.x, 70.0f, displayHand[selection].transform.position.z);
                }
            }
            else if (curPhase == Phase.Placement)
            {
                //move
                if (cardSel != 0 && cardSelCD == 0.0f) //move
                {
                    displayHand[selection].transform.position = new Vector3(displayHand[selection].transform.position.x, 0.0f, displayHand[selection].transform.position.z);

                    if (cardSel < 0)
                    {
                        if (selection == 0)
                        {
                            selection = displayHand.Count - 1;
                        }
                        else
                        {
                            selection--;
                        }
                    }
                    else if (cardSel > 0)
                    {
                        if (selection == displayHand.Count-1)
                        {
                            selection = 0;
                        }
                        else
                        {
                            selection++;
                        }
                    }
                    displayHand[selection].transform.position = new Vector3(displayHand[selection].transform.position.x, 70.0f, displayHand[selection].transform.position.z);
                    cardSelCD = 0.4f;
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    if (pHand[selection].type == CardType.Monster)
                    {
                        if (c1M == null) //if player doesnt alraedy have a monster out
                        {
                            //in future it will prompt but for now itll auto play
                            CardClass clone = Instantiate(pHand[selection]) as CardClass;
                            c1M = clone;
                            activeCards.Add(clone);
                            Destroy(displayHand[selection]);
                            displayHand.RemoveAt(selection);
                            pHand.RemoveAt(selection);
                            checkEffects(Phase.Placement);
                            p1MonsterObj = Instantiate(c1M.cardModel);
                            p1MonsterObj.transform.position = p1M.transform.position;
                            //reset positions.
                            foreach (GameObject cimage in displayHand)
                            {
                                cimage.transform.position = new Vector3(minHandX + (maxHandX / (displayHand.Count) * f), 0.0f, 0.0f);
                                f += 1.0f;
                                //
                            }
                            selection = 0;
                        }
                        else
                        {
                            Debug.Log("There is already a monster on the field.");
                        }
                    }
                    else if (pHand[selection].type == CardType.Action)
                    {
                        foreach(EffectClass efs in pHand[selection].effects)
                        {
                            //normally i would have to check if the card CAN before its used,
                            //and the player wouldnt be able to use the card if they couldnt
                            //however with this current code, the Action item is used regardless of whether it
                            //functions or not.
                            //It also assumes that its being used on the player's active monster
                            if (c1M != null)
                            {
                                efs.EffectedCard = setTargetCard(efs);
                                efs.doEffect();
                                Destroy(displayHand[selection]);
                                displayHand.RemoveAt(selection);
                                pHand.RemoveAt(selection);
                                foreach (GameObject cimage in displayHand)
                                {
                                    cimage.transform.position = new Vector3(minHandX + (maxHandX / (displayHand.Count) * f), 0.0f, 0.0f);
                                    f += 1.0f;
                                    //
                                }
                                selection = 0;
                            }
                            else
                            {
                                //normally this would mean dont let it activate.
                            }
                        }
                        //activate the card.
                    }
                    else if (pHand[selection].type == CardType.Item) 
                    {
                        //prompt for setting the card
                    }

                }
               
                //cooldown
                if (cardSelCD != 0.0f) //SWITCHING COOLDOWN SCRIPT.
                { // so it doesn't change rapidly
                    cardSelCD -= Time.deltaTime;
                    if (cardSelCD <= 0.0)
                    {
                        cardSelCD = 0.0f;
                    }
                }

                if (Input.GetButtonDown("Next"))
                {
                    if (phaseCD <= 0.0f)
                    {
                        curPhase = Phase.Battle;
                        phaseCD = phaseCDmax;
                    }
                    else
                    {
                        Debug.Log("ERROR: Wait a bit to go to the next phase!");
                    }
                }


            }
            else if (curPhase == Phase.Battle)
            {
                foreach (GameObject cimage in displayHand)
                {
                    cimage.transform.position = new Vector3(minHandX + (maxHandX / (displayHand.Count) * f), 0.0f, 0.0f);
                    f += 1.0f;
                    //
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    //attack also choose left from right?
                    //worry abt that later
                    if(c2M != null && c1M != null)
                    {
                        c2M.Health -= c1M.Attack;
                        checkEffects(Phase.Battle);
                        curPhase = Phase.End;
                    }
                    else
                    {
                        //attacking cannot happen
                    }
                }

                else if(Input.GetButtonDown("Next"))
                {
                    if (phaseCD <= 0.0f)
                    {
                        curPhase = Phase.End;
                        phaseCD = phaseCDmax;
                        paused = true;
                    }
                    else
                    {
                        Debug.Log("ERROR: Wait a bit to go to the next phase!");
                    }
                }
            }
            else if (curPhase == Phase.End)
            {
                if (!paused)
                {
                    checkEffects(Phase.End);
                    whoPlays = activePlayer.Enemy;
                    curPhase = Phase.Draw;
                    paused = true;
                    phaseCD = phaseCDmax;
                }
            }
        }
        //ENEMY
        else if (whoPlays == activePlayer.Enemy)
        {
            if (curPhase == Phase.Draw)
            {
                if (!paused)
                {
                    eHand.Add(eDeck[0]);
                    var temp = eHand[0];
                    eDeck.Remove(temp);
                    checkEffects(Phase.Draw);
                    curPhase = Phase.Placement;
                    selection = 0;
                    phaseCD = phaseCDmax;
                    paused = true;
                }
            }
            else if (curPhase == Phase.Placement)
            {
                if (!paused)
                {
                    if (c2M == null)
                    {
                        var a = 0;
                        var found = false;
                        foreach (CardClass card in eHand)
                        {
                            if (card.type == CardType.Monster)
                            {
                                selection = a;
                                found = true;
                            }
                            a++;
                        }
                        if (!found)
                        {
                            phaseCD = phaseCDmax;
                            paused = true;
                            curPhase = Phase.End;
                        }
                        else if (found)
                        {
                            CardClass clone = Instantiate(eHand[selection]) as CardClass;
                            c2M = clone;
                            activeCards.Add(clone);
                            //Destroy(displayHand[selection]);
                            // displayHand.RemoveAt(selection);
                            eHand.RemoveAt(selection);
                            checkEffects(Phase.Placement);
                            p2MonsterObj = Instantiate(c2M.cardModel);
                            p2MonsterObj.transform.position = p2M.transform.position;
                            phaseCD = phaseCDmax;
                            paused = true;
                            curPhase = Phase.Battle;
                        }
                    }
                    else if (c2M != null)
                    {
                        foreach (CardClass card in eHand) //use every action that it can use
                        {
                            foreach (EffectClass efs in card.effects)
                            {
                                if (card.type == CardType.Action)
                                {
                                    efs.EffectedCard = setEnemyTargetCard(efs);
                                    efs.doEffect();
                                }
                                else
                                {
                                    //normally this would mean dont let it activate.
                                }
                            }
                            //activate the card.
                        }
                    }
                    phaseCD = phaseCDmax;
                    paused = true;
                    curPhase = Phase.Battle;
                }
            }
            else if (curPhase == Phase.Battle)
            {
                if (!paused)
                {
                    if (true) //if AI has decided to go
                    {
                        //attack also choose left from right?
                        //worry abt that later
                        if (c2M != null && c1M != null)
                        {
                            c1M.Health -= c2M.Attack;
                            checkEffects(Phase.Battle);
                            curPhase = Phase.End;
                        }
                        else
                        {
                            phaseCD = phaseCDmax;
                            paused = true;
                            curPhase = Phase.End;
                            //attacking cannot happen
                        }
                    }
                    else
                    {
                        //if ai doesnt wanna go
                        curPhase = Phase.End;
                    }
                }

            }
            else if (curPhase == Phase.End)
            {
                if (!paused)
                {
                    checkEffects(Phase.End);
                    whoPlays = activePlayer.Player;
                    curPhase = Phase.Draw;
                    phaseCD = phaseCDmax;
                    paused = true;
                }
            }
        }
    }
    public CardClass setTargetCard(EffectClass read)
    {
        if(read.targetArea == Places.Main)
        {
            if (c1M != null)
            {
                read.EffectedCard = c1M;
                return read.EffectedCard;
            }
        }

        else if (read.targetArea == Places.Left)
        {
            if (c1Left != null)
            {
                read.EffectedCard = c1Left;
                return read.EffectedCard;
            }
        }

        else if (read.targetArea == Places.Right)
        {
            if (c1Right != null)
            {
                read.EffectedCard = c1Right;
                return read.EffectedCard;
            }
        }

        else if (read.targetArea == Places.EnemyMain)
        {
            if (c2M != null)
            {
                read.EffectedCard = c2M;
                return read.EffectedCard;
            }
        }
        else if (read.targetArea == Places.EnemyLeft)
        {
            if (c2Left != null)
            {
                read.EffectedCard = c2Left;
                return read.EffectedCard;
            }
        }
        else if (read.targetArea == Places.EnemyRight)
        {
            if (c2Right != null)
            {
                read.EffectedCard = c2Right;
                return read.EffectedCard;
            }
        }
        /*
        else if (read.targetArea == Places.EnemyHand)
        {

        }
        else if (read.targetArea == Places.SelfHand)
        {

        } */
        else
        {
            Debug.Log("Error happened with choosing correct target card");
            return read.EffectedCard;
        }
        return read.EffectedCard;
    }

    public void checkEffects(Phase phaseToCheck)
    {
        foreach (CardClass activeCard in activeCards)
        {
            foreach (EffectClass ef in activeCard.effects)
            {
                if (phaseToCheck != null)
                { 
                    if (ef.whenActivated == phaseToCheck)
                    {
                        ef.EffectedCard = setTargetCard(ef);
                        ef.doEffect();
                    }
                }
            }
        }
    }
    public CardClass setEnemyTargetCard(EffectClass read)
    {
        if (read.targetArea == Places.Main)
        {
            if (c2M != null)
            {
                read.EffectedCard = c2M;
                return read.EffectedCard;
            }
        }

        else if (read.targetArea == Places.Left)
        {
            if (c2Left != null)
            {
                read.EffectedCard = c2Left;
                return read.EffectedCard;
            }
        }

        else if (read.targetArea == Places.Right)
        {
            if (c2Right != null)
            {
                read.EffectedCard = c2Right;
                return read.EffectedCard;
            }
        }

        else if (read.targetArea == Places.EnemyMain)
        {
            if (c1M != null)
            {
                read.EffectedCard = c1M;
                return read.EffectedCard;
            }
        }
        else if (read.targetArea == Places.EnemyLeft)
        {
            if (c1Left != null)
            {
                read.EffectedCard = c1Left;
                return read.EffectedCard;
            }
        }
        else if (read.targetArea == Places.EnemyRight)
        {
            if (c1Right != null)
            {
                read.EffectedCard = c1Right;
                return read.EffectedCard;
            }
        }
        /*
        else if (read.targetArea == Places.EnemyHand)
        {

        }
        else if (read.targetArea == Places.SelfHand)
        {

        } */
        else
        {
            Debug.Log("Error happened with choosing correct target card");
            return read.EffectedCard;
        }
        return read.EffectedCard;
    }


}



