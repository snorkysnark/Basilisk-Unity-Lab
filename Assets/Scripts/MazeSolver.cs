using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MazeSolver : Powerable
{
    [SerializeField] Maze maze = null;
    [SerializeField] PathWalker walker = null;
    [SerializeField] Vector2Int startingPoint = default;

    private PathfindingGraph graph;
    private List<Vector3> path;

    private void Start()
    {
        graph = new PathfindingGraph(maze);
    }

    private void OnDrawGizmos()
    {
        if(graph != null)
        {
            graph.DrawGizmo();
        }
        if(path != null)
        {
            Gizmos.color = Color.red;
            for(int i = 0; i < path.Count - 1; i++) Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }

    private void OnValidate()
    {
        startingPoint = maze.ClampPosition(startingPoint);
        if(!Application.isPlaying && walker != null)
        {
            walker.transform.position = maze.CellWorldPosition(startingPoint.x, startingPoint.y);
        }
    }

    public override void Power()
    {
        if(path == null) path = Solve();
        if(path != null) walker.WalkPath(path);
    }

    private List<Vector3> Solve()
    {
        List<Node> open = new List<Node>();
        Node start = graph.GetNodeByPosition(startingPoint);
        start.gCost = 0;
        start.fCost = EstimateCost(start);
        open.Add(start);

        while(open.Count > 0)
        {
            Node current = open.Min();
            if(IsGoal(current))
            {
                return ReconstructPath(current);
            }
            open.Remove(current);

            foreach(Node neighbor in current.connections)
            {
                int newGCost = current.gCost + 1;
                if(newGCost < neighbor.gCost)
                {
                    neighbor.gCost = newGCost;
                    neighbor.fCost = neighbor.fCost + EstimateCost(neighbor);
                    neighbor.cameFrom = current;
                    if(!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private bool IsGoal(Node node)
    {
        return node.x == maze.GoalPosition.x && node.y == maze.GoalPosition.y;
    }

    private List<Vector3> ReconstructPath(Node end)
    {
        List<Vector3> path = new List<Vector3>();
        Node current = end;
        while(current != null)
        {
            path.Add(graph.NodeWorldPosition(current));
            current = current.cameFrom;
        }
        path.Reverse();
        return path;
    }

    private int EstimateCost(Node node)
    {
        return Mathf.Abs(node.x - maze.GoalPosition.x) + Mathf.Abs(node.y + maze.GoalPosition.y);
    }

    private class PathfindingGraph
    {
        private Maze maze;
        private Dictionary<Vector2Int, Node> nodeByPosition = new Dictionary<Vector2Int, Node>();

        public PathfindingGraph(Maze maze)
        {
            this.maze = maze;
            for(int y = 0; y < maze.Height; y++)
            {
                for(int x = 0; x < maze.Width; x++)
                {
                    if(maze.CellHasWall(x, y)) continue;
                    Node node = GetOrCreateNodeAt(x, y);
                    TryAddConnection(node, maze, x + 1, y);
                    TryAddConnection(node, maze, x - 1, y);
                    TryAddConnection(node, maze, x, y + 1);
                    TryAddConnection(node, maze, x, y - 1);
                }
            }
        }

        public void DrawGizmo()
        {
            foreach(Node node in nodeByPosition.Values)
            {
                foreach(Node neighbor in node.connections)
                {
                    Gizmos.DrawLine(NodeWorldPosition(node), NodeWorldPosition(neighbor));
                }
            }
        }

        public Vector3 NodeWorldPosition(Node node)
        {
            return maze.CellWorldPosition(node.x, node.y);
        }

        public Node GetNodeByPosition(Vector2Int position)
        {
            Node node = null;
            nodeByPosition.TryGetValue(position, out node);
            return node;
        }
        
        private Node GetOrCreateNodeAt(int x, int y)
        {
            Vector2Int position = new Vector2Int(x, y);
            Node node;
            if(!nodeByPosition.TryGetValue(position, out node))
            {
                node = new Node(x, y);
                nodeByPosition.Add(position, node);
            }
            return node;
        }

        private void TryAddConnection(Node node, Maze maze, int x, int y)
        {
            if(!maze.IsWithinBounds(x, y) || maze.CellHasWall(x, y)) return;
            Node neighbor = GetOrCreateNodeAt(x, y);
            node.connections.Add(neighbor);
        }
    }

    private class Node : IComparable<Node>
        {
            public int gCost;
            public int fCost;
            public Node cameFrom;
            public readonly int x;
            public readonly int y;

            public List<Node> connections;

            public Node(int x, int y)
            {
                this.x = x;
                this.y = y;
                connections = new List<Node>();
                gCost = int.MaxValue;
                fCost = int.MaxValue;
            }

            public int CompareTo(Node other)
            {
                return fCost.CompareTo(other.fCost);
            }
        }
}
