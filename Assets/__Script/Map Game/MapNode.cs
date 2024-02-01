using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapNode : MonoBehaviour
{
    public MapNodeData nodeData;
    
    public TMP_Text nameText;
    public Image image;
    public Btn selectButton;

    public List<MapUnit> unitsOnNode = new List<MapUnit>();

    public void OnUnitEnter(MapUnit unit)
    {
        unitsOnNode.Add(unit);
    }

    public void OnUnitExit(MapUnit unit)
    {
        unitsOnNode.Remove(unit);
    }

}
