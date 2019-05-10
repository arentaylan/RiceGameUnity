using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needed
public enum Phase
{
    Draw, Placement, Battle, End
}
public enum CardType
{
    Monster, Action, Item
}
public enum ActivationType
{
    None, Summon, Continuous, Attack, Attacked, Blocked, Blocking, Discard, Whenever
}
public enum activePlayer
{
    Player, Enemy
}

public enum EffectSum
{
    ChangeNum, Discard, SearchDeck
}

// fuck idk
public enum CardElement
{
    Plant, Fire, Water
}

public enum Places
{
    Main, Left, Right, EnemyMain, EnemyLeft, EnemyRight, SelfHand, EnemyHand
}

public enum Placement
{
    Faceup, Facedown
}

public enum ActivationTiming
{
    Anytime, SelfMain, SelfBattle, SelfEnd, EnemMain, EnemBattle, EnemEnd
}

public class Enums : MonoBehaviour
{

}
