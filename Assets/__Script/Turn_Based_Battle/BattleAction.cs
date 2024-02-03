using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: think about how to handle composed actions (e.g. attack + heal)
public enum ActionType { ATTACK, HEAL, ITEM, DEFEND, FLEE }
public enum TargetType { SINGLE, ALL_PARTY, ALL_ALLY, ALL_OPPONENT }

[CreateAssetMenu(fileName = "new action data", menuName = "Turn Based Battle/Action Data")]
public class BattleAction : ScriptableObject
{
    public string actionName;
    public ActionType actionType; //I want an editor tool that changes the value whenever this is changed
    public TargetType targetType;
    [SerializeField][TextArea] public string actionDescription;
    public int mpCost;

    public float multiplyDamage; //multiplies the base damage of the attack.
    public int addDamage; //adds damage directly. to be added after multiplyDamage
    public int addHealing; //adds directly to the hp of the target. can be negative to damage,and overwrites defence


    private void OnValidate()
    {
        if (actionType == ActionType.ATTACK)
        {
            multiplyDamage = Mathf.Clamp(multiplyDamage, 0.1f, 10);
        }else if (actionType == ActionType.HEAL)
        {
            addHealing = Mathf.Clamp(addHealing, -100, 100);
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

        actionName = this.name;
    }
}
