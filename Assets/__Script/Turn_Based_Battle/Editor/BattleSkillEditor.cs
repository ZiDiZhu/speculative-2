using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleSkill))]
public class BattleSkillEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BattleSkill skill = (BattleSkill)target;

        skill.actionName = EditorGUILayout.TextField("Action Name", skill.actionName);
        skill.actionType = (ActionType)EditorGUILayout.EnumPopup("Action Type", skill.actionType);
        skill.targetType = (TargetType)EditorGUILayout.EnumPopup("Target Type", skill.targetType);
        skill.actionDescription = EditorGUILayout.TextArea(skill.actionDescription, GUILayout.MaxHeight(75));
        skill.mpCost = EditorGUILayout.IntField("MP Cost", skill.mpCost);

        switch (skill.actionType)
        {
            case ActionType.ATTACK:
                // Only show addDamage for ATTACK type
                skill.addDamage = EditorGUILayout.IntField("Add Damage", skill.addDamage);
                break;
            case ActionType.HEAL:
                // Only show addHealing for HEAL type
                skill.addHealing = EditorGUILayout.IntField("Add Healing", skill.addHealing);
                break;
            case ActionType.BUFF:
                // Handle BUFF type fields
                skill.multiplyDamage = EditorGUILayout.FloatField("Multiply Damage", skill.multiplyDamage);
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(skill);
        }
    }
}
