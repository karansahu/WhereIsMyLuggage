using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace Luggage.Pathfind
{
    public class AStarPathfinding : MonoBehaviour
    {
        public List<Node> FindPath(Node start, Node destination)
        {
            // Stores information about explored and unexplorered nodes
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            // Starting node has a distance of 0 from itself
            start.gCost = 0;
            start.fCost = GetHeuristic(start, destination);

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                // Get the node with the lowest fCost from the open set
                Node current = openSet.OrderBy(n => n.fCost).First();

                if (current == destination)
                {
                    // Reached the destination, reconstruct the path
                    return ReconstructPath(current);
                }

                // Move current node to closed set
                openSet.Remove(current);
                closedSet.Add(current);

                // Explore all neighbors
                foreach (Node neighbor in current.neighborNodes)
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    // Calculate tentative gCost for neighbor
                    float tentativeGCost = current.gCost + GetMovementCost(current, neighbor);

                    // Check if neighbor is not in open set or has a lower gCost through current node
                    if (!openSet.Contains(neighbor) || tentativeGCost < neighbor.gCost)
                    {
                        neighbor.parent = current;
                        neighbor.gCost = tentativeGCost;
                        neighbor.fCost = neighbor.gCost + GetHeuristic(neighbor, destination);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            // No path found
            return null;
        }

        private float GetHeuristic(Node startNode, Node endNode)
        {
            return Math.Abs(endNode.gridPosition.x - startNode.gridPosition.x) + Math.Abs(endNode.gridPosition.y - startNode.gridPosition.y);
        }

        // Function to define the movement cost between two nodes (replace with your logic)
        private float GetMovementCost(Node a, Node b)
        {
            // In a grid-based movement, this could simply be 1
            return (a.gridPosition - b.gridPosition).sqrMagnitude;

            //return 1.0f;
        }

        // Reconstructs the path by following the parent nodes
        private List<Node> ReconstructPath(Node node)
        {
            List<Node> path = new List<Node>();
            while (node != null)
            {
                path.Insert(0, node);
                node = node.parent;
            }
            return path;
        }
    }
}