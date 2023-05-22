
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {
    public static Pathfinding Instance;

    private LayerMask unwalkableMask;
    private LayerMask groundMask;
    private LayerMask playerMask;
    private const int MOVE_DIAGONAL_COST = 14;
    private const int MOVE_STRAIGHT_COST = 10;




    private Grid<PathNode> grid;
    private List<PathNode> openList;    // Queued up for searching
    private List<PathNode> closedList; // Already searched
    private Vector3 originPosition;

    public static Pathfinding GetInstance() {
        return Instance;
    }

    public Pathfinding(int width, int height,Vector3 originPosition , LayerMask unwalkableMask, LayerMask groundMask, LayerMask playerMask) {
        Instance = this;
        this.unwalkableMask = unwalkableMask;
        this.groundMask = groundMask;
        this.playerMask = playerMask;
        this.originPosition = originPosition;
        grid = new Grid<PathNode>(width, height, 1.5f, originPosition, (Grid<PathNode> grid, int x, int y) => new PathNode(grid, x, y));
        foreach (PathNode pathNode in grid.GetGridArray()) {
            pathNode.CheckForWalkable(unwalkableMask);
            pathNode.CheckForGround(groundMask);
            pathNode.CheckForPlayer(playerMask);
        }
    }

    private PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y, out bool exists);
    }

    private List<PathNode> GetNeighbours(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();
       
        if (currentNode.x - 1 >= 0) {
            // left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            if (currentNode.y - 1 >= 0) {
                // left down
                PathNode leftDownNeigbour = GetNode(currentNode.x - 1, currentNode.y - 1);
                neighbourList.Add(leftDownNeigbour);
            }
            if (currentNode.y + 1 < grid.GetHeight()) {
                // left up
                PathNode leftUpNeigbour = GetNode(currentNode.x - 1, currentNode.y + 1);
                neighbourList.Add(leftUpNeigbour);
            }
        }
        
        if (currentNode.x + 1 < grid.GetWidth()) {
            // right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            if (currentNode.y - 1 >= 0) {
                // right down
                PathNode rightDownNeighbour = GetNode(currentNode.x + 1, currentNode.y - 1);
                neighbourList.Add(rightDownNeighbour);
            }
            if (currentNode.y + 1 < grid.GetHeight()) {
                // right up
                PathNode rightUpNeighbour = GetNode(currentNode.x + 1, currentNode.y + 1);
                neighbourList.Add(rightUpNeighbour);
            }
        } 
        // up 
        if (currentNode.y + 1 < grid.GetHeight()) {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }
        // down 
        if (currentNode.y - 1 >= 0) {
            neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode node = endNode;
        while (node.previousNode != null) {
            path.Add(node.previousNode);
            node = node.previousNode;
            node.isPath = true;
        }
        path.Reverse();
        return path;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private int CalculateDistance(PathNode a, PathNode b) {
        if (a == null || b == null) {
            return 0;
        }
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }     

    // Pulbic Methods

    public Grid<PathNode> GetGrid() {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetGridPosition(startWorldPosition, out int startX, out int startY);
        grid.GetGridPosition(endWorldPosition, out int endX, out int endY);
        List<PathNode> path = FindPath(startX, startY, endX, endY, out bool hasPath);

        if (hasPath) {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                Vector3 origin = pathNode.GetWorldPositionNodeCenterPathfinding();
                Vector3 destination = new Vector3(0.5f, 0, 0.5f) * grid.GetCellSize() * 0.5f;
                vectorPath.Add(origin + destination);
            }

            return vectorPath;
        } else {
            return null;
        }
    }

    public List<Vector3> FindPathForAttack(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetGridPosition(startWorldPosition, out int startX, out int startY);
        grid.GetGridPosition(endWorldPosition, out int endX, out int endY);
        List<PathNode> path = FindPathForAttack(startX, startY, endX, endY, out bool hasPath);

        if (hasPath) {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                Vector3 origin = pathNode.GetWorldPositionNodeCenterPathfinding();
                Vector3 destination = new Vector3(0.5f, 0, 0.5f) * grid.GetCellSize() * 0.5f;
                vectorPath.Add(origin + destination);
            }

            return vectorPath;
        } else {
            return null;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY, out bool hasPath) {
        PathNode startNode = grid.GetGridObject(startX, startY, out bool startExists);
        PathNode endNode = grid.GetGridObject(endX, endY, out bool endExists);
        openList = new List<PathNode> {
            startNode
        };
        closedList = new List<PathNode>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y, out bool exists);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateF();
                pathNode.previousNode = null;
                //pathNode.isPlayer = false;
                pathNode.isPath = false;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateF();


        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);
            

            if (currentNode.Equals(endNode)) {
                // Final Node
                hasPath = true;
                endNode.isPath = true;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbours(currentNode)) {
                neighbourNode.CheckForPlayer(playerMask);
                if (closedList.Contains(neighbourNode)) {
                    continue;
                }
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }
                if (!neighbourNode.isGround) {
                    closedList.Add(neighbourNode);
                    continue;
                }
                if (neighbourNode.isPlayer) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tempGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
                if (tempGCost < neighbourNode.gCost) {

                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tempGCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                    neighbourNode.CalculateF();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //Debug.Log("Can't find path");
        hasPath = false;
        return null;
    }


      public List<PathNode> FindPathForAttack(int startX, int startY, int endX, int endY, out bool hasPath) {
        PathNode startNode = grid.GetGridObject(startX, startY, out bool startExists);
        PathNode endNode = grid.GetGridObject(endX, endY, out bool endExists);
        openList = new List<PathNode> {
            startNode
        };
        closedList = new List<PathNode>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y, out bool exists);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateF();
                pathNode.previousNode = null;
                //pathNode.isPlayer = false;
                pathNode.isPath = false;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateF();


        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);
            

            if (currentNode.Equals(endNode)) {
                // Final Node
                hasPath = true;
                endNode.isPath = true;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbours(currentNode)) {
                neighbourNode.CheckForPlayer(playerMask);
                if (closedList.Contains(neighbourNode)) {
                    continue;
                }
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }
                if (!neighbourNode.isGround) {
                    closedList.Add(neighbourNode);
                    continue;
                }
                /*if (neighbourNode.isPlayer) {
                    closedList.Add(neighbourNode);
                    continue;
                }*/

                int tempGCost = currentNode.gCost + CalculateDistance(currentNode, neighbourNode);
                if (tempGCost < neighbourNode.gCost) {

                    neighbourNode.previousNode = currentNode;
                    neighbourNode.gCost = tempGCost;
                    neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                    neighbourNode.CalculateF();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //Debug.Log("Can't find path");
        hasPath = false;
        return null;
    }





}
