using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;


namespace AstarAPI
{
    public class Pathfinder : MonoBehaviour
    {
        public GrideGenerator grid;

        public Node startNode;
        public Node goalNode;

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node getOptimalNode()
        {
            Node optimalNode = null;

            foreach (var node in openList)
            {
                if (optimalNode == null || optimalNode.fCost > node.fCost)
                {
                    optimalNode = node;
                }
            }
            return optimalNode;
        }

        float GetHCost(Node nodeA, Node nodeB)
        {
            // Calculate the Manhattan distance between two nodes
            int xDistance = Mathf.Abs((int)nodeA.nodeGridPosition.x - (int)nodeB.nodeGridPosition.x);
            int yDistance = Mathf.Abs((int)nodeA.nodeGridPosition.y - (int)nodeB.nodeGridPosition.y);
            return xDistance + yDistance;
        }

        float GetGCost(Node node, Node parent)
        {
            // Calculate the distance between the current node and the parent node
            float distanceFromParent = Vector3.Distance(node.nodeworldPosition, parent.nodeworldPosition);

            // Return the total g cost of the current node
            // The g cost of the parent node is added to the distance from the parent node to the current node
            return parent.gCost + distanceFromParent;
        }

        public List<Node> FindPath(Vector2Int startPos, Vector2Int endPos)
        {
            startNode = grid.GetNodeFromGridPos(startPos);
            goalNode = grid.GetNodeFromGridPos(endPos);

            openList.Clear();
            closedList.Clear();

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                colornodes();
                Node currentNode = getOptimalNode();

                if (currentNode == goalNode)
                {
                    // Path has been found
                    List<Node> path = new List<Node>();
                    Node node = goalNode;

                    while (node != startNode)
                    {
                        path.Add(node);
                        node = node.parent;
                    }

                    path.Reverse();
                    return path;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbouringNode in grid.GetNeighbours(currentNode))
                {
                    if (closedList.Contains(neighbouringNode)) continue;

                    float tentativeGCost = GetGCost(neighbouringNode, currentNode);

                    if (!openList.Contains(neighbouringNode) || tentativeGCost < neighbouringNode.gCost)
                    {
                        neighbouringNode.gCost = tentativeGCost;
                        neighbouringNode.hCost = GetHCost(neighbouringNode, goalNode);
                        neighbouringNode.parent = currentNode;

                        if (!openList.Contains(neighbouringNode))
                        {
                            openList.Add(neighbouringNode);
                        }
                    }
                }
            }
            // Path not found
            return null;
        }

        void colornodes()
        {
            foreach (var node in openList)
            {
                node.ngj.GetComponent<MeshRenderer>().material = grid.openNodeCol;
            }

            foreach (var node in closedList)
            {
                node.ngj.GetComponent<MeshRenderer>().material = grid.closedNodeCol;
            }
        }
    }
}
