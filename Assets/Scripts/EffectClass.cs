using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectClass : ScriptableObject
{
    public Phase whenActivated; //the phase in which this effect can be activated
    public ActivationType activeType; //how often this effect is activated
    public List<Condition> conditions; //conditions to be met.
    public bool activated = false; //if the card has already been activated, and cannot be activated again until further specification
    public CardClass EffectedCard;
    public activePlayer target;
    public Places targetArea;
    public EffectSum effSum;
    public abstract void doEffect();
    public void resetActivaton()
    {
        activated = false;
    }
}



[CreateAssetMenu(fileName = "ChangeATK", menuName = "Custom/Effects/ChangeATK", order = 1)]
public class ChangeATK : EffectClass
{
    public int amt;
    public bool add;

    public override void doEffect()
    {
        if (!activated)
        {

                bool passAllConditions = true;
                foreach (Condition cond in conditions)
                {
                    if (!cond.checkCondition())
                    {
                        passAllConditions = false;
                        break;
                    }
                }

                if (passAllConditions)
                {
                    EffectedCard.Attack = add ? EffectedCard.Attack + amt : EffectedCard.Attack - amt;
                    if (activeType == ActivationType.Continuous)
                    {
                        //
                    }
                    else if(activeType == ActivationType.Summon && !activated)
                    {
                        activated = true;
                    }
                }
        }
    }
} 

[CreateAssetMenu(fileName = "ChangeHP", menuName = "Custom/Effects/ChangeHP", order = 2)]
public class ChangeHP : EffectClass
{
    public int amt;
    public bool add;

    public override void doEffect()
    {
        if (!activated)
        {
                bool passAllConditions = true;
                foreach (Condition cond in conditions)
                {
                    if (!cond.checkCondition())
                    {
                        passAllConditions = false;
                        break;
                    }
                }

                if (passAllConditions)
                {
                    EffectedCard.Health = add ? EffectedCard.Health + amt : EffectedCard.Health - amt;
                    if(activeType == ActivationType.Continuous)
                    {
                        //
                    }
                    else if(activeType == ActivationType.Summon && !activated)
                    {
                        activated = true;
                    }
                }
        }
    }
}