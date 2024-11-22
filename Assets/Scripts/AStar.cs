using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public int X;
    public int Y;
    public int G; // 이동 비용
    public int H; // 휴리스틱(목적지까지 예상 비용)
    public int F => G + H; // 총 비용
    public bool IsWall;
    public Node Parent;

    public Node(bool isWall, int x, int y)
    {
        IsWall = isWall;
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Node other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
}

public enum DirectionType
{
    Up,
    Down,
    Left,
    Right
}

public class AStar
{
    private static readonly Vector2Int[] DIRECTIONS =
    {
        new Vector2Int(0, 1),  // 위
        new Vector2Int(0, -1), // 아래
        new Vector2Int(-1, 0),  // 왼쪽
        new Vector2Int(1, 0)  // 오른쪽
    };

    private bool[,] tiles;
    private List<Node> openList = new List<Node>();
    private HashSet<Node> closedList = new HashSet<Node>();

    public void SetTiles(bool[,] tiles)
    {
        this.tiles = tiles;
    }

    public List<Node> Find(Vector2Int start, Vector2Int end)
    {
        Node startNode = new Node(false, start.x, start.y);
        Node endNode = new Node(false, end.x, end.y);

        openList.Clear();
        closedList.Clear();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetNodeWithLowestF(openList);

            if (currentNode.X == endNode.X && currentNode.Y == endNode.Y)
            {
                return RetracePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor) || neighbor.IsWall)
                {
                    continue;
                }

                int newG = currentNode.G + 1;

                if (!openList.Contains(neighbor) || newG < neighbor.G)
                {
                    neighbor.G = newG;
                    neighbor.H = Mathf.Abs(neighbor.X - endNode.X) + Mathf.Abs(neighbor.Y - endNode.Y);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return new List<Node>();
    }

    private Node GetNodeWithLowestF(List<Node> openList)
    {
        Node lowestFNode = openList[0];
        foreach (Node node in openList)
        {
            if (node.F < lowestFNode.F)
            {
                lowestFNode = node;
            }
        }
        return lowestFNode;
    }

    private List<Node> GetNeighbors(Node currentNode)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int dir in DIRECTIONS)
        {
            int x = currentNode.X + dir.x;
            int y = currentNode.Y + dir.y;

            // 경계 밖이면 무시
            if (x < 0 || y < 0 || x >= tiles.GetLength(0) || y >= tiles.GetLength(1))
            {
                continue;
            }

            neighbors.Add(new Node(tiles[x, y], x, y));
        }

        return neighbors;
    }

    private List<Node> RetracePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // 부모 노드를 따라 경로를 역추적
        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        // 경로를 시작점에서 끝점 순으로 뒤집기
        path.Reverse();
        return path;
    }

    public List<Vector2Int> GetRouteMovementValue(Vector2Int start, Vector2Int end)
    {
        List<Node> route = Find(start, end);
        Debug.Log($"루트의 길이는 {route.Count}");
        string routeString = "";
        for (int index = 0; index < route.Count; index++)
        {
            routeString += $"{route[index].X}, {route[index].Y} => ";
        }
        Debug.Log(routeString);
        List<Vector2Int> routeMovement = new List<Vector2Int>();
        for (int index = 1; index < route.Count; index++)
        {
            int x = route[index].X - route[index - 1].X;
            int y = route[index].Y - route[index - 1].Y;
            routeMovement.Add(new Vector2Int(x, -y));
        }
        return routeMovement;
    }
}