using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new node", menuName = "Map Game/Map Node")]
public class MapNodeData : ScriptableObject
{
    public string name;
    [TextArea(5,7)] public string description;

    public List<Edge> neighbors;
    public Sprite mapSprite;

    public List<MapUnit> unitsOnNode = new List<MapUnit>(); 

    public MapNodeData(string n ){
        name = n;
    }

}

[System.Serializable]
public class Edge
{
    public MapNodeData connectedNode; // Reference to the connected NodeSO
    public float weight; // if A->B is not the same as B->A, then it's an incline

    public Edge(MapNodeData node, float weight)
    {
        connectedNode = node;
        this.weight = weight;
    }
}