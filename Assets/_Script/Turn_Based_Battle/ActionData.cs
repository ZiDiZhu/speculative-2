using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


//TODO: think about how to handle composed actions (e.g. attack + heal)
public enum ActionType { ATTACK, HEAL, ITEM, DEFEND, FLEE }
public enum TargetType { SINGLE, PARTY_SELF, PARTY_OTHER }

[CreateAssetMenu(fileName = "new action data", menuName = "Turn Based Battle/Action Data")]
public class ActionData : ScriptableObject
{
    public string actionName;
    public ActionType actionType;
    public TargetType targetType;
    public List<ActionData> additionalActions = new List<ActionData>(); //used for composed actions

    [SerializeField][TextArea] public string actionDescription;
    public int mpCost;

    public float multiplyDamage; //multiplies the base damage of the attack.
    public int addDamage; //adds damage directly. to be added after multiplyDamage
    public int addHealing; //adds directly to the hp of the target. can be negative to damage,and overwrites defence


    private void OnValidate()
    {
        if (actionType == ActionType.ATTACK)
        {

        }else if (actionType == ActionType.HEAL)
        {
            
        }
        else if (actionType == ActionType.ITEM)
        {
            
        }
        else if (actionType == ActionType.DEFEND)
        {
            
        }
        else if (actionType == ActionType.FLEE)
        {
            
        }
    }
}
