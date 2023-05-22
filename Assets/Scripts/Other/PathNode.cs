using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode {

    
    public bool isWalkable;
    public bool isGround;
    public bool isPath;
    public bool isPlayer;

    public bool isInRange;

    public bool isPossibleMovePath;


    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode previousNode;
    public Player playerOnNode;

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = true;
        this.isGround = false;
        this.isPath = false;
        this.isPlayer = false;
        this.isInRange = false;
        this.isPossibleMovePath = false;
    }

     public override string ToString() {
        return x + "," + y ;
    }

    public void CalculateF() {
        fCost = gCost + hCost;
    }

    public Vector3 GetWorldPosition() {
        return grid.GetWorldPosition(x, y);
    }

    public Vector3 GetWorldPositionNodeCenter() {
        return new Vector3(GetWorldPosition().x, 0, GetWorldPosition().z) + new Vector3(0.5f, 0, 0.5f) * grid.GetCellSize();
    }

    public Vector3 GetWorldPositionNodeCenterPathfinding() {
        return new Vector3(GetWorldPosition().x, 0, GetWorldPosition().z) + new Vector3(0.5f, 0, 0.5f) * grid.GetCellSize() * 0.5f;
    }

    public void CheckForWalkable(LayerMask unwalkableMask) {
        if(!Physics.CheckSphere(GetWorldPositionNodeCenter(), .75f,unwalkableMask)) {
            isWalkable = true;
        } else {
            isWalkable = false;
        }
    }

    public void CheckForGround(LayerMask groundMask) {
        float groundCheckDistance = 1f;
        if (Physics.Raycast(GetWorldPositionNodeCenter(), Vector3.down, groundCheckDistance, groundMask)) {
            isGround = true;
        } else {
            isGround= false;
        }
    }
     
    public void CheckForPlayer(LayerMask playerMask) {
        float playerCheckDistance = 1f;
        float sphereCastRadius = (grid.GetCellSize() * 0.5f) - 0.1f;
        Vector3 sphereCastDirection = Vector3.up;

        /*if (Physics.SphereCast(GetWorldPositionNodeCenter(), sphereCastRadius, sphereCastDirection,
                out RaycastHit hitInfo, playerCheckDistance, playerMask)) {
            playerOnNode = hitInfo.transform.GetComponent<Player>();
            isPlayer = true;
            Debug.Log(playerOnNode.name);
        }
        else {
            isPlayer = false;
        }*/

        if (Physics.Raycast(GetWorldPositionNodeCenter(), Vector3.up, out RaycastHit hitInfo, playerCheckDistance, playerMask)) {
            playerOnNode = hitInfo.transform.GetComponent<Player>();
            isPlayer = true;
            //Debug.Log(playerOnNode.name);
        } else {
            isPlayer = false;
        }


        /*if (Physics.CheckSphere(GetWorldPositionNodeCenter(), ((grid.GetCellSize() * .5f) - 0.2f), playerMask)) {
            isPlayer = true;
        } else {
            isPlayer = false;
        }*/


    }

    public bool IsAccessible(LayerMask unwalkableMask, LayerMask groundMask, LayerMask playerMask) {
        CheckForWalkable(unwalkableMask);
        CheckForGround(groundMask);
        CheckForPlayer(playerMask);
        return !isPlayer && isWalkable && isGround;
    }
    
    public bool IsAccessibleNoPlayers(LayerMask unwalkableMask, LayerMask groundMask) {
        CheckForWalkable(unwalkableMask);
        CheckForGround(groundMask);
        return isWalkable && isGround;
    }

    public Player GetPlayer() {
        if (isPlayer) {
            return playerOnNode;
        }
        else {
            return null;
        }
    }

}
