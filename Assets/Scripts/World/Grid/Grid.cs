
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<GridObject> {
   
    bool showDebug = false;

    public EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

     

    private Vector3 originPosition;

    private int width;
    private int height;
    private float cellSize;

    private GridObject[,] gridArray;
    
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<GridObject>, int, int, GridObject> createGridObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new GridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
         
        
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
            Debug.Log(width.ToString() + "x" + height.ToString());
            
            Font font = Resources.Load<Font>("Fonts/Custom Sprite Font/Custom Sprite Font");
            Debug.Log("Custom font loaded: " + (font != null));

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    //debugTextArray[x, y] = GameUtils.WorldText.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, 8, Color.white, TextAnchor.MiddleCenter, font: font);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };


        }
         
         
         
    }
     
    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public void GetGridPosition(Vector3 worldPosition, out int x, out int y) {
        
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);

        if (x < 0 || x >= width || y < 0 || y >= height) {
            //Debug.LogWarning("Requested grid position (" + worldPosition + ") is outside of the grid.");
        }

        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);
    }
   


    public void SetGridObject(int x, int y, GridObject value) {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) {
                OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            }
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, GridObject value) {
        GetGridPosition(worldPosition, out int x, out int y);
        SetGridObject(x, y, value);
    }

    public GridObject GetGridObject(int x, int y, out bool exists) {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            exists = true;
            return gridArray[x, y];
        } else {
            exists = false;
            return default(GridObject);
        }
    }

    public GridObject GetGridObject(Vector3 worldPosition, out bool exists) {
        GetGridPosition(worldPosition, out int x, out int y);
        return GetGridObject(x, y, out exists);
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public GridObject[,] GetGridArray() {
        return gridArray;
    }

    public bool GridObjectExists(int x , int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return true;
        } else {
            return false;
        }
    }


}
