using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Custom/Create Card Data", order = 2)]
public class CardClass : ScriptableObject
{
    public string CardName = "Default"; //The name of the card
    public int CardNumber = 0;
    public CardType type; //Monster, Action, and Item ??
    public int Tier; // 0 if not a monster, 1-5 for monsters, 1 needs no discard, 2 needs 1 discard, etc. till 5 needs 4 discard
    public int Health;
    public int Attack;
    public GameObject cardModel;
    public Sprite cardArt;
    public List<EffectClass> effects; // the effect of the card
    public string FlavorText = "This card is a default test card. It has no effect, and no characteristics.";
    // for exmaple purposes
    public CardElement element;

}
