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


    public Canvas battleCanvas;

    private activePlayer whoPlays = activePlayer.Player;
    private Phase curPhase = Phase.Draw;
    private GameObject BG;
    private string place = Environment.CurrentEnvironment;
    
    // Start is called before the first frame update
    void Awake()
    {
        NPC = Environment.fightingThis;
        Vector3 spwnPos = new Vector3(0, 0, 0);
        if (place == "Field")
        {
           BG = Instantiate(forestBG);
        }
        else if(place == "Desert")
        {
           BG = Instantiate(desertBG);
        }
        BG.transform.position = spwnPos;

        var a = 0;
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
        Debug.Log("Turn: " + whoPlays + "Phase: " + curPhase);
        if (c1M != null)
        {
            Debug.Log("P1 Monster: " + c1M.name + "\n P1 Health: " + c1M.Health + "\n P1 Attack: " + c1M.Attack);
            if(c1M.Health <= 0)
            {
                Destroy(p1MonsterObj);
                activeCards.Remove(c1M);
                c1M = null;
                p1Life--;
            }
        }
        if (c2M != null)
        {
            Debug.Log("P2 Monster: " + c2M.name + "\n P2 Health: " + c2M.Health + "\n P2 Attack: " + c2M.Attack);
            if (c2M.Health <= 0)
            {
                Destroy(p2MonsterObj);
                activeCards.Remove(c2M);
                c2M = null;
                p2Life--;
            }

        }


        //PLAYER
        if (whoPlays == activePlayer.Player)
        {
            if(curPhase == Phase.Draw)
            {
                pHand.Add(pDeck[0]);
                var temp = pHand[pHand.Count-1];
                pDeck.Remove(temp);

                checkEffects(Phase.Draw);
                curPhase = Phase.Placement;
                selection = 0;

                GameObject cardObject = new GameObject();
                displayHand.Add(Instantiate(cardObject, battleCanvas.transform));
                cardImage = displayHand[displayHand.Count-1].AddComponent<Image>();
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
                            c1M = pHand[selection];
                            activeCards.Add(pHand[selection]);
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
                    curPhase = Phase.Battle;
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
                    curPhase = Phase.End;
                }
            }
            else if (curPhase == Phase.End)
            {
                checkEffects(Phase.End);
                whoPlays = activePlayer.Enemy;
                curPhase = Phase.Draw;
            }
        }
        //ENEMY
        else if (whoPlays == activePlayer.Enemy)
        {
            if (curPhase == Phase.Draw)
            {
                eHand.Add(eDeck[0]);
                var temp = eHand[0];
                eDeck.Remove(temp);
                checkEffects(Phase.Draw);
                curPhase = Phase.Placement;
                selection = 0;

            }
            else if (curPhase == Phase.Placement)
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
                        curPhase = Phase.End;
                    }
                    else if(found)
                    {
                        c2M = eHand[selection];
                        activeCards.Add(eHand[selection]);
                        //Destroy(displayHand[selection]);
                       // displayHand.RemoveAt(selection);
                        eHand.RemoveAt(selection);
                        checkEffects(Phase.Placement);
                        p1MonsterObj = Instantiate(c2M.cardModel);
                        p1MonsterObj.transform.position = p2M.transform.position;
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

                    curPhase = Phase.Battle;
            }
            else if (curPhase == Phase.Battle)
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
            else if (curPhase == Phase.End)
            {
                checkEffects(Phase.End);
                whoPlays = activePlayer.Player;
                curPhase = Phase.Draw;
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
                if (ef.whenActivated == phaseToCheck)
                {
                    ef.EffectedCard = setTargetCard(ef);
                    ef.doEffect();
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



