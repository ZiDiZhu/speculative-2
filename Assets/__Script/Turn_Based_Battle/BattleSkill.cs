using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: think about how to handle composed actions (e.g. attack + heal)
public enum ActionType { ATTACK, HEAL, BUFF }
public enum TargetType { SELF, SINGLE_OPPONENT, SINGLE_ALLY, ALL_PARTY, ALL_ALLY, ALL_OPPONENT }

[CreateAssetMenu(fileName = "new Battle Skill", menuName = "Turn Based Battle/Action Data")]
public class BattleSkill : ScriptableObject
{
    public string actionName;
    public ActionType actionType; //I want an editor tool that changes the value whenever this is changed
    public TargetType targetType;
    [SerializeField][TextArea] public string actionDescription;
    public int mpCost;

    public float multiplyDamage; 
    public int addDamage; 
    public int addHealing; 


    private void OnValidate()
    {
        if (actionType == ActionType.ATTACK)
        {
            //TODO: only show addDamage
        }else if (actionType == ActionType.HEAL)
        {
            //TODO: only show addHealing
        }
        else if (actionType == ActionType.BUFF)
        {
            
        }

        actionName = this.name;
    }
}
