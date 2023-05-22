using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using System;

public class GridVisual : MonoBehaviour {
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask playerMask;
    [SerializeField] private bool realtimeUpdates = false;

    private Grid<PathNode> grid;
    private Mesh mesh;

   

    private void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(Grid<PathNode> grid) {
        this.grid = grid;
        UpdateGridVisual();

        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
        GridCombatSystem.GetInstance().OnReachedTargetPosition += Grid_OnReachedTargetPosition;
        GridCombatSystem.GetInstance().OnPlayerSelected += GridVisual_OnPlayerSelected;

        GridCombatSystem.GetInstance().OnBusyStateTriggered += GridVisual_OnBusyStateTriggered;

        GridCombatSystem.GetInstance().OnCancelActionTriggered += GridVisual_OnCancelActionTriggered;
        GridCombatSystem.GetInstance().OnMoveActionTriggered += GridVisual_OnMoveActionTriggered;
        GridCombatSystem.GetInstance().OnMainActionTriggered += GridVisual_OnMainActionTriggered;
    }

    private void GridVisual_OnBusyStateTriggered() {
        realtimeUpdates = false;
    }

    private void GridVisual_OnMainActionTriggered(Player obj) {
        realtimeUpdates = true;
    }

    private void GridVisual_OnCancelActionTriggered(Player player) {
        realtimeUpdates = false;
    }

    private void GridVisual_OnMoveActionTriggered(Player player) {
       realtimeUpdates = true;
    }

    private void FixedUpdate() {
        if (realtimeUpdates) {
            UpdateGridVisual();
        }
    }

    private void OnDestroy() {
        grid.OnGridObjectChanged -= Grid_OnGridObjectChanged;
        GridCombatSystem.GetInstance().OnReachedTargetPosition -= Grid_OnReachedTargetPosition;
        GridCombatSystem.GetInstance().OnPlayerSelected -= GridVisual_OnPlayerSelected;

        GridCombatSystem.GetInstance().OnBusyStateTriggered -= GridVisual_OnBusyStateTriggered;

        GridCombatSystem.GetInstance().OnCancelActionTriggered -= GridVisual_OnCancelActionTriggered;
        GridCombatSystem.GetInstance().OnMoveActionTriggered -= GridVisual_OnMoveActionTriggered;
        GridCombatSystem.GetInstance().OnMainActionTriggered -= GridVisual_OnMainActionTriggered;
    }

    

    private void GridVisual_OnPlayerSelected() {
        UpdateGridVisual();
    }

    private void Grid_OnReachedTargetPosition(object sender, GridCombatSystem.OnReachedTargetPositionEventArgs e) {
        
        UpdateGridVisual();
    }

    

    private void Grid_OnGridObjectChanged(object sender, Grid<PathNode>.OnGridObjectChangedEventArgs e) {

        Debug.Log("grid object changed:" + e.x + " , " + e.y);
        UpdateGridVisual();
    }

    private void UpdateGridVisual() {

        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        MeshTools.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out vertices, out uv, out triangles);
         
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 0, 1) * grid.GetCellSize();

                PathNode pathNode = grid.GetGridObject(x, y, out bool exists);


                Vector2 gridObjectUV = Vector2.zero;


                if (!pathNode.isGround) {
                    quadSize = Vector3.zero;
                }
                if (!pathNode.isWalkable) {
                    gridObjectUV = new Vector2(0.1f, 1);
                } else { //it is walkable
                    if (pathNode.isPath) {
                        //gridObjectUV = Vector2.one;
                    }
                    if (pathNode.isPlayer) {
                        gridObjectUV = new Vector2(0.4f, 1);
                    }
                }

                if (pathNode.isInRange) {
                    gridObjectUV = Vector3.one;
                }

                if (pathNode.isPossibleMovePath) {
                    gridObjectUV = new Vector2(0.4f, 1);
                }


                MeshTools.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, grid.GetCellSize() * 0.45f, gridObjectUV, gridObjectUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}
