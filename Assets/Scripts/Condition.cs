using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject
{

    public abstract bool checkCondition();
    // SYNTAX:
    /*
     * subConditionList: [element,plant], [atk,50]
     * subConditionBooleanOps: ==, > 
     * combineBooleanOps: ||
     * means: this condition states that in order for a card to be effected, must be of plant type OR have atk above 50
     
    public List<string> subConditionList;
    public List<string> subConditionBooleanOps;
    public List<string> combineBooleanOps;
    */
    /*bool classifySubcondition(string sc, string op, CardClass card)
    {


        // break up sub condition into two parts
        string[] splitSC = sc.Split(',');
        string predicate = splitSC[0];
        string value = splitSC[1];

        if (predicate == "element")
        {
            CardElement element;

            if (value == "plant") element = CardElement.Plant;
            if (value == "fire") element = CardElement.Fire;
            if (value == "water") element = CardElement.Water;
            else { return false; }

            if (op == "==") return card.efct.cType == CardType.Monster && card.element == element;
            else return false; // cannot quantifiably compare card elements
        }
        else if (predicate == "atk")
        {
            int num;
            int.TryParse(value, out num);
            if (op == "==") return card.efct.cType == CardType.Monster && card.Attack == num;
            else if (op == ">") return card.efct.cType == CardType.Monster && card.Attack > num;
            else if (op == "<") return card.efct.cType == CardType.Monster && card.Attack < num;
            else if (op == ">=") return card.efct.cType == CardType.Monster && card.Attack >= num;
            else if (op == "<=") return card.efct.cType == CardType.Monster && card.Attack <= num;
            else if (op == "!=") return card.efct.cType == CardType.Monster && card.Attack != num;
            else return false;
        }
        return false;
        // keep adding else ifs for potential predicates
    }

    public bool evaluate(CardClass card)
    {
        if (subConditionList.Count == 0) return true;
        bool current = false;
        for (int i = 0; i < subConditionList.Count; ++i)
        {
            bool newCond = classifySubcondition(subConditionList[i], subConditionBooleanOps[i], card);
            if (i != 0)
            {
                if (combineBooleanOps[i - 1] == "||") current = current || newCond;
                else if (combineBooleanOps[i - 1] == "&&") current = current && newCond;
                else current = current || newCond;
            }
            else
            {
                current = newCond;
            }
        }
        return current;
    }*/
}

[CreateAssetMenu(fileName = "NoCondition", menuName = "Custom/Conditions/None", order = 1)]
public class NoCondition : Condition
{
    public override bool checkCondition()
    {
            return true;
    }
}

[CreateAssetMenu(fileName = "LessHP", menuName = "Custom/Conditions/LessHP", order = 2)]
public class LessHP : Condition
{
    public CardClass Comparison;
    public CardClass Self;

    public override bool checkCondition()
    {
        if(Self.Health < Comparison.Health)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}