using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;


namespace AstarAPI
{
    public class GrideGenerator : MonoBehaviour
    {
        public Material openNodeCol;
        public Material currentNodeCol;
        public Material closedNodeCol;

        public GameObject normalAssCube;
        public Node[] nodesInGrid;
        public Vector2 gridsize;
        public Vector3 cellSize;

        Vector3 gridOrigin;
        float offsetX;
        float offsetY;

        void Awake()
        {
            nodesInGrid = new Node[(int)(gridsize.x * gridsize.y)];
            for (int i = 0; i < gridsize.x * gridsize.y; i++)
            {
                nodesInGrid[i] = new Node();
            }

            gridOrigin = gameObject.transform.position;

            GenerateGrid();
        }

        void GenerateGrid()
        {
            for (int y = 0; y < gridsize.y; y++)
            {
                offsetY = cellSize.z * y;

                for (int x = 0; x < gridsize.x; x++)
                {
                    offsetX = cellSize.x * x;

                    Node currentNode = nodesInGrid[(int)(x + y * gridsize.x)];
                    currentNode.nodeGridPosition = new Vector2Int(x, y);
                    currentNode.nodeworldPosition = new Vector3(gridOrigin.x + offsetX, gridOrigin.y, gridOrigin.z + offsetY);
                    currentNode.ngj = Instantiate(normalAssCube, currentNode.nodeworldPosition, Quaternion.identity);

                    currentNode.ngj.transform.localScale = cellSize;
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            Vector2Int gridPos = node.nodeGridPosition;

            if (gridPos.x > 0)
            {
                neighbours.Add(GetNodeFromGridPos(new Vector2Int(gridPos.x - 1, gridPos.y)));
            }

            if (gridPos.x < gridsize.x - 1)
            {
                neighbours.Add(GetNodeFromGridPos(new Vector2Int(gridPos.x + 1, gridPos.y)));
            }

            if (gridPos.y < gridsize.y - 1)
            {
                neighbours.Add(GetNodeFromGridPos(new Vector2Int(gridPos.x, gridPos.y + 1)));
            }

            if (gridPos.y > 0)
            {
                neighbours.Add(GetNodeFromGridPos(new Vector2Int(gridPos.x, gridPos.y - 1)));
            }

            return neighbours;
        }

        public Node GetNodeFromGridPos(Vector2Int position)
        {
            int index = (int)(position.x + position.y * gridsize.x);

            return nodesInGrid[index];
        }

    }
}
