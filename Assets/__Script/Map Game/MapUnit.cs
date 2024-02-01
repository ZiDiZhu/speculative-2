using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit : MonoBehaviour
{
    public MapNode currentNode;
    public MapGraph map; // Reference to the map

    public void MoveToNode(MapNode node)
    {
        if (currentNode != null)
        {
            currentNode.OnUnitExit(this);
        }
        currentNode = node;
        currentNode.OnUnitEnter(this);
    }
    public List<MapNodeData> FindPathToNode(MapNodeData targetNode)
    {
        if (map == null || currentNode == null || targetNode == null)
        {
            Debug.LogError("Map or nodes are not properly set.");
            return null;
        }

        // Use the map's FindPathBetweenNodes method to find the path
        List<MapNodeData> path = map.data.FindPathBetweenNodes(currentNode.nodeData, targetNode);

        return path;
    }
}
