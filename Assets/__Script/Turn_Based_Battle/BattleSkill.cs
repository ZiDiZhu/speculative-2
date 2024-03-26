using UnityEngine;

//TODO: think about how to handle composed actions (e.g. attack + heal)

[CreateAssetMenu(fileName = "new Battle Skill", menuName = "Turn Based Battle/Action Data")]
public class BattleSkill : ScriptableObject
{

    public enum ActionType { ATTACK, HEAL, BUFF }
    public enum TargetType { SELF, SINGLE_OPPONENT, SINGLE_ALLY, ALL_PARTY, ALL_ALLY, ALL_OPPONENT }

    public string skillName;
    public ActionType actionType; //I want an editor tool that changes the value whenever this is changed
    public TargetType targetType;
    [SerializeField][TextArea] public string actionDescription;
    public int mpCost;

    public float multiplyDamage { get; set; } 
    public int addDamage { get; set; }
    public int addHealing   { get; set; }


    private void OnValidate()
    {
        skillName = this.name;
    }
}
