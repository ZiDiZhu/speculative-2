using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ActionType { ATTACK, MAGIC, SPECIAL, ITEM, DEFEND, RUN }

public class CharacterAction : MonoBehaviour
{
    public string actionName;
    public TextField description;
    public int mpCost;
    public ActionType actionType;


}
