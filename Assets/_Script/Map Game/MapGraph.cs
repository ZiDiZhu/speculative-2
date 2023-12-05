using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new graph", menuName = "Map Game/Map Graph")]
public class MapGraph : ScriptableObject
{

    //TODO: Add a Generator Method to add to Context Menu that reads a .txt and parses it.
    public List<MapNodeData> nodes = new List<MapNodeData>();

    public void AddNode(MapNodeData node)
    {
        nodes.Add(node);
    }

    public void RemoveNode(MapNodeData node)
    {
        nodes.Remove(node);
    }
    public List<MapNodeData> FindPathBetweenNodes(MapNodeData startNode, MapNodeData targetNode)
    {
        Queue<MapNodeData> queue = new Queue<MapNodeData>();
        Dictionary<MapNodeData, MapNodeData> parentNodes = new Dictionary<MapNodeData, MapNodeData>();

        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            MapNodeData currentNode = queue.Dequeue();

            if (currentNode == targetNode)
            {
                return ConstructPath(parentNodes, targetNode);
            }

            foreach (Edge edge in currentNode.neighbors)
            {
                MapNodeData neighborNode = edge.connectedNode;

                if (!parentNodes.ContainsKey(neighborNode))
                {
                    queue.Enqueue(neighborNode);
                    parentNodes[neighborNode] = currentNode;
                }
            }
        }

        // If targetNode is not reachable from startNode, return an empty list
        return new List<MapNodeData>();
    }

    private List<MapNodeData> ConstructPath(Dictionary<MapNodeData, MapNodeData> parentNodes, MapNodeData targetNode)
    {
        List<MapNodeData> path = new List<MapNodeData>();
        MapNodeData currentNode = targetNode;

        while (parentNodes.ContainsKey(currentNode))
        {
            path.Insert(0, currentNode); // Insert at the beginning to maintain order
            currentNode = parentNodes[currentNode];
        }

        path.Insert(0, currentNode); // Insert the start node

        return path;
    }

}
