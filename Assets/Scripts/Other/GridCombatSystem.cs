using GameUtils;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridCombatSystem : MonoBehaviour {
    private static GridCombatSystem Instance;

    [SerializeField] GridVisual gridVisual;

    [Header("Grid Properties")]
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private Vector3 originPosition = new Vector3(0, 0, 0);
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;
    //[SerializeField] private bool dynamicGridVisuals = false;

    [Header("Active Player")]
    [SerializeField] private Player player;


    private Pathfinding pathfinding;
    private Grid<PathNode> grid;

    private const int DIAGONAL_MOVE_COST_5 = 5;
    private const int DIAGONAL_MOVE_COST_10 = 10;
    private const int STRAIGHT_MOVE_COST = 5;

    private int moveCost;

    public enum CombatState {
        InCombat,
        OutOfCombat
    }
    private CombatState combatState;
    public enum State {
        Normal,
        Busy,
    }
    private State state;

    private ActionType turnState;

    public EventHandler<OnReachedTargetPositionEventArgs> OnReachedTargetPosition;
    public class OnReachedTargetPositionEventArgs : EventArgs {
        public Vector3 worldPosition;
        public int range;
    }

    public event Action OnPlayerSelected;


    public event Action OnBusyStateTriggered;
    public event Action<Player> OnCancelActionTriggered;
    
    public event Action<Player> OnMoveActionTriggered;
    public event Action<Player> OnMainActionTriggered;
    public event Action<Player> OnBonusActionTriggered;
    public event Action<Player> OnReactionTriggered;

    public static GridCombatSystem GetInstance() {
        return Instance;
    }

    private void Awake() {
        Instance = this;
        //state = State.Normal;
        SetState(State.Normal);
        combatState = CombatState.OutOfCombat;
    }

    private void Start() {
        pathfinding = new Pathfinding(width, height, originPosition, unwalkableMask, groundMask, playerMask);
        grid = pathfinding.GetGrid();
        gridVisual.SetGrid(grid);

        PlayerManager.GetInstance().OnPlayerSelected += GridCombatSystem_OnPlayerSelected;

        CombatTracker.GetInstance().OnCombatStarted += GridCombatSystem_OnCombatStarted;
        CombatTracker.GetInstance().OnRoundStarted += GridCombatSystem_OnRoundStarted;
        CombatTracker.GetInstance().OnTurnStarted += GridCombatSystem_OnTurnStarted;

    }

    private void OnDestroy() {

        PlayerManager.GetInstance().OnPlayerSelected -= GridCombatSystem_OnPlayerSelected;

        CombatTracker.GetInstance().OnCombatStarted -= GridCombatSystem_OnCombatStarted;
        CombatTracker.GetInstance().OnRoundStarted -= GridCombatSystem_OnRoundStarted;
        CombatTracker.GetInstance().OnTurnStarted -= GridCombatSystem_OnTurnStarted;

    }

    void Update() {
        //Debug.Log(state.ToString());
        switch (combatState) {
            case CombatState.OutOfCombat:
                if (Input.GetKeyDown(KeyCode.F)) {
                    EntityHandler.GetInstance().FindAndAddPlayerEntities();
                }
                if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
                    CombatTracker.GetInstance().AddPlayerToCombat(player.GetComponent<Player>());
                }
                if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
                    CombatTracker.GetInstance().RemovePlayerFromCombat(player.GetComponent<Player>());
                }
                if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    if (CombatTracker.GetInstance().StartCombat()) {
                        //Debug.Log("Combat started");
                    }
                }
                break;
            case CombatState.InCombat:
                if (Input.GetKeyDown(KeyCode.Backspace)) { // END COMBAT
                    CombatTracker.GetInstance().EndCombat();
                    combatState = CombatState.OutOfCombat;
                }

                
                switch (state) {
                    case State.Normal:
                        if (Input.GetKeyDown(KeyCode.Alpha0)) { // CANCEL ACTION
                            CancelAction();
                        }

                        if (Input.GetKeyDown(KeyCode.Return)) { // END TURN
                            SetTurnState(ActionType.None);
                            CombatTracker.GetInstance().EndTurn();
                        }



                        switch (turnState) {
                            case ActionType.MoveAction:
                                if (!Mouse3D.GetInstance().IsMouseOverUI()) { // SHOW POSSIBLE MOVES
                                    CalculatePathToMouse(player.GetWorldPosition(), Mouse3D.GetInstance().GetMouseWorldPosition());
                                }
                                if (Input.GetMouseButtonDown(0) && !Mouse3D.GetInstance().IsMouseOverUI()) { // COMFIRM MOVE
                                    Vector3 mouseWorldPosition = Mouse3D.GetInstance().GetMouseWorldPosition();
                                    grid.GetGridPosition(mouseWorldPosition, out int mouseX, out int mouseY);
                                    if (grid.GetGridObject(mouseX, mouseY, out bool targetExists).isInRange) {
                                        //state = State.Busy;
                                        SetState(State.Busy);
                                        player.MoveTo(mouseWorldPosition, () => {
                                            player.SetRemainingGameMoveRange(player.GetRemainingGameMoveRange() - moveCost);
                                            //state = State.Normal;
                                            SetState(State.Normal);
                                            ClearSecondaryFlags();
                                            SetTurnState(ActionType.None);
                                            OnReachedTargetPosition(this, new OnReachedTargetPositionEventArgs { worldPosition = mouseWorldPosition, range = player.GetGridMoveRange() });
                                        });
                                    }
                                }
                                break;
                            case ActionType.MainAction:
                                if (!Mouse3D.GetInstance().IsMouseOverUI()) { // SHOW POSSIBLE ATTACK TARGETS
                                    CalculatePathToMouseForAttack(player.GetWorldPosition(), Mouse3D.GetInstance().GetMouseWorldPosition());
                                    
                                }
                                if (Input.GetMouseButtonDown(0) && !Mouse3D.GetInstance().IsMouseOverUI()) { // COMFIRM ATTACK
                                    Vector3 mouseWorldPosition = Mouse3D.GetInstance().GetMouseWorldPosition();
                                    grid.GetGridPosition(mouseWorldPosition, out int mouseX, out int mouseY);
                                    PathNode targetNode = grid.GetGridObject(mouseX, mouseY, out bool targetExists);
                                    PathNode playerNode = grid.GetGridObject(player.GetWorldPosition(), out bool playerNodeExists);
                                    //Debug.Log("node: " + targetNode.x + " , " + targetNode.y + " isPlayer: " + targetNode.isPlayer);
                                    if ( /*targetNode.isInRange &&*/ targetNode.isPlayer) {
                                        if (CalculateGridDistance(playerNode, targetNode) <= 1) {
                                            moveCost -= STRAIGHT_MOVE_COST;
                                        }
                                        SetState(State.Busy);
                                        player.Attack(targetNode.GetPlayer(), () => {
                                            player.SetRemainingGameMoveRange(player.GetRemainingGameMoveRange() - moveCost);
                                            //state = State.Normal;
                                            SetState(State.Normal);
                                            ClearSecondaryFlags();
                                            SetTurnState(ActionType.None);
                                            OnReachedTargetPosition(this, new OnReachedTargetPositionEventArgs { worldPosition = mouseWorldPosition, range = player.GetGridMoveRange() });
                                        });
                                    }
                                }
                                break;
                            case ActionType.BonusAction:
                                break;
                            case ActionType.Reaction:
                                break;
                            case ActionType.None:
                                break;
                        }
                        break;
                    case State.Busy:
                        break;
                }
                break;
        }





    }


    // EVENT SUBSCRIBERS
    private void GridCombatSystem_OnRoundStarted(int roundIndex) {
        Debug.Log("Round: " + roundIndex);
    }

    private void GridCombatSystem_OnCombatStarted(List<Player> playerList) {
        foreach (Player player in playerList) {
            Debug.Log(player.name + " initiative: " + player.GetCombatInitiative());
        }

        combatState = CombatState.InCombat;
    }

    private void GridCombatSystem_OnPlayerSelected(Player player) {
        if (combatState == CombatState.OutOfCombat) {
            this.player = player;
            UpdateGridForPlayer(player.GetWorldPosition(), player.GetGridMoveRange(), player.GetGameMoveRange());
            OnPlayerSelected?.Invoke();
        }
    }

    private void GridCombatSystem_OnTurnStarted(int turnIndex) {
        this.player = CombatTracker.GetInstance().GetCurrentPlayer().GetComponent<Player>();
        PlayerManager.GetInstance().SelectPlayer(player);
        Debug.Log("Turn: " + turnIndex);
        Debug.Log("Player: " + player);
        player.SetRemainingGameMoveRange(player.GetGameMoveRange());

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                PathNode pathNode = grid.GetGridObject(x, y, out bool exists);
                pathNode.isPath = false;
                pathNode.isPossibleMovePath = false;
                pathNode.isInRange = false;
            }
        }
        

        OnReachedTargetPosition(this, null);
    }


    // OTHER METHODS
    public Vector3 CalculateDirectionVector(PathNode fromNode, PathNode toNode) { //todo: have to fix this
        Vector3 direction = toNode.GetWorldPosition() - fromNode.GetWorldPosition();
        direction.Normalize(); 
        return direction;
    }

    private int CalculateGridDistance(PathNode a, PathNode b) {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        return Mathf.Max(dx, dy);
    }

    private void ClearSecondaryFlags() {
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.GetGridObject(x, y, out bool exists1).isInRange = false;
                grid.GetGridObject(x, y, out bool exists2).isPath = false;
                grid.GetGridObject(x, y, out bool exists3).isPossibleMovePath = false;
            }
        }
    }

    private void UpdateGridForPlayer(Vector3 worldPosition, int gridRange, int gameRange) {
        grid.GetGridPosition(worldPosition, out int playerX, out int playerY);
        gridRange += 1;

        ClearSecondaryFlags();
         
        int diagonalMoveCount;
        int moveCost;

        Dictionary<PathNode, List<PathNode>> pathListsDictionary = new Dictionary<PathNode, List<PathNode>>();

        for (int x = playerX - gridRange; x <= playerX + gridRange; x++) {
            for (int y = playerY - gridRange; y <= playerY + gridRange; y++) { // i was here
                 
                 
                diagonalMoveCount = 0;
                moveCost = 0;
                if (grid.GridObjectExists(x, y)) {
                    if (grid.GetGridObject(x, y, out bool exists1).IsAccessibleNoPlayers(unwalkableMask, groundMask)) {
                        List<PathNode> path = Pathfinding.GetInstance().FindPath(playerX, playerY, x, y, out bool hasPath);
                        if (hasPath) {
                            
                            // Path exists
                            for (int i = 0; i < path.Count - 1; i++) {
                                PathNode node = path[i];
                                PathNode nextNode = path[i + 1];
                                if (Mathf.Abs(node.x - nextNode.x) != 0 && MathF.Abs(node.y - nextNode.y) != 0) {
                                    // Moving horizontally & vertically
                                    if (diagonalMoveCount % 2 == 0) {
                                        moveCost += DIAGONAL_MOVE_COST_5;
                                    } else {
                                        moveCost += DIAGONAL_MOVE_COST_10;
                                    }
                                    diagonalMoveCount++;
                                } else if (Mathf.Abs(node.x - nextNode.x) != 0 && MathF.Abs(node.y - nextNode.y) == 0) {
                                    // Moving only horizontally
                                    moveCost += STRAIGHT_MOVE_COST;
                                } else if (Mathf.Abs(node.x - nextNode.x) == 0 && MathF.Abs(node.y - nextNode.y) != 0) {
                                    // Moving only vertically
                                    moveCost += STRAIGHT_MOVE_COST;
                                } else {
                                    // Not moving
                                }

                                if (moveCost <= gameRange) {
                                    PathNode pathNode = grid.GetGridObject(x, y, out bool exists);
                                    pathNode.isInRange = true;
                                } else {
                                    PathNode pathNode = grid.GetGridObject(x, y, out bool exists);
                                    pathNode.isInRange = false;
                                    break;
                                }
                            }

                            if (grid.GetGridObject(x, y, out bool exists2).isInRange) {
                                pathListsDictionary.Add(grid.GetGridObject(x, y, out bool exists3), path);
                            }
                        } else {
                            // No path to position
                        }
                    } else {
                        // Position not Accessible
                    }
                } else {
                    // Position  outside of grid
                }

            }
        }

    }


    private PathNode previousMouseNode;
    private void CalculatePathToMouse(Vector3 playerWorldPosition, Vector3 mouseWorldPosition) {
        PathNode playerNode = grid.GetGridObject(playerWorldPosition, out bool playerNodeExists);
        PathNode mouseNode = grid.GetGridObject(mouseWorldPosition, out bool mouseNodeExists);

        Debug.Log("RUN");
        if (mouseNodeExists && playerNodeExists) {
            if (previousMouseNode != null) {
                if (mouseNode == previousMouseNode) {
                    Debug.Log("SAME NODE");

                    return;
                }
            }

            int diagonalMoveCount = 0;
            moveCost = 0;
            int playerX = playerNode.x;
            int playerY = playerNode.y;
            int mouseX = mouseNode.x;
            int mouseY = mouseNode.y;
            int gridRange = player.GetGridMoveRange();

            for (int x = playerX - gridRange; x <= playerX + gridRange; x++) {
                for (int y = playerY - gridRange; y <= playerY + gridRange; y++) {
                    if (grid.GridObjectExists(x, y)) {
                        grid.GetGridObject(x, y, out bool exists)
                            .isPossibleMovePath = false;
                    }
                }
            }

            List<PathNode> currentPath = pathfinding.FindPath(playerX, playerY, mouseX, mouseY, out bool pathExists);
            if (pathExists) {
                for (int i = 0; i < currentPath.Count - 1; i++) {
                    PathNode node = currentPath[i];
                    PathNode nextNode = currentPath[i + 1];
                    if (Mathf.Abs(node.x - nextNode.x) != 0 && MathF.Abs(node.y - nextNode.y) != 0) {
                        // Moving horizontally & vertically
                        if (diagonalMoveCount % 2 == 0) {
                            moveCost += DIAGONAL_MOVE_COST_5;
                        }
                        else {
                            moveCost += DIAGONAL_MOVE_COST_10;
                        }

                        diagonalMoveCount++;
                    }
                    else if (Mathf.Abs(node.x - nextNode.x) != 0 && MathF.Abs(node.y - nextNode.y) == 0) {
                        // Moving only horizontally
                        moveCost += STRAIGHT_MOVE_COST;
                    }
                    else if (Mathf.Abs(node.x - nextNode.x) == 0 && MathF.Abs(node.y - nextNode.y) != 0) {
                        // Moving only vertically
                        moveCost += STRAIGHT_MOVE_COST;
                    }
                    else {
                        
                    }

                    if (moveCost <= player.GetRemainingGameMoveRange()) {
                        mouseNode.isInRange = true;
                        mouseNode.isPossibleMovePath = true;
                        node.isPossibleMovePath = true;
                    }
                    else {
                        mouseNode.isInRange = false;
                        mouseNode.isPossibleMovePath = false;
                        node.isPossibleMovePath = false;
                    }
                }
            }

            previousMouseNode = mouseNode;

        }
    }

    private void CalculatePathToMouseForAttack(Vector3 playerWorldPosition, Vector3 mouseWorldPosition) {
        PathNode playerNode = grid.GetGridObject(playerWorldPosition, out bool playerNodeExists);
        PathNode mouseNode = grid.GetGridObject(mouseWorldPosition, out bool mouseNodeExists);
        if (mouseNodeExists && playerNodeExists) {
            int diagonalMoveCount = 0;
            moveCost = 0;
            int playerX = playerNode.x;
            int playerY = playerNode.y;
            int mouseX = mouseNode.x;
            int mouseY = mouseNode.y;
            int gridRange = player.GetGridMoveRange();

            for (int x = playerX - gridRange; x <= playerX + gridRange; x++) {
                for (int y = playerY - gridRange; y <= playerY + gridRange; y++) {
                    if (grid.GridObjectExists(x, y)) {
                        grid.GetGridObject(x, y, out bool exists)
                            .isPossibleMovePath = false;
                    }
                }
            }

            List<PathNode> currentPath = pathfinding.FindPathForAttack(playerX, playerY, mouseX, mouseY, out bool pathExists);
            if (pathExists) {
                for (int i = 0; i < currentPath.Count - 1; i++) {
                    PathNode node = currentPath[i];
                    PathNode nextNode = currentPath[i + 1];
                    if (Mathf.Abs(node.x - nextNode.x) != 0 && MathF.Abs(node.y - nextNode.y) != 0) {
                        // Moving horizontally & vertically
                        if (diagonalMoveCount % 2 == 0) {
                            moveCost += DIAGONAL_MOVE_COST_5;
                        }
                        else {
                            moveCost += DIAGONAL_MOVE_COST_10;
                        }

                        diagonalMoveCount++;
                    }
                    else if (Mathf.Abs(node.x - nextNode.x) != 0 && MathF.Abs(node.y - nextNode.y) == 0) {
                        // Moving only horizontally
                        moveCost += STRAIGHT_MOVE_COST;
                    }
                    else if (Mathf.Abs(node.x - nextNode.x) == 0 && MathF.Abs(node.y - nextNode.y) != 0) {
                        // Moving only vertically
                        moveCost += STRAIGHT_MOVE_COST;
                    }
                    else {
                        // Not moving
                    }

                    if (moveCost <= player.GetRemainingGameMoveRange()) {
                        PathNode pathNode = grid.GetGridObject(mouseX, mouseY, out bool exists);
                        pathNode.isInRange = true;
                        pathNode.isPossibleMovePath = true;
                        node.isPossibleMovePath = true;
                    }
                    else {
                        PathNode pathNode = grid.GetGridObject(mouseX, mouseY, out bool exists);
                        pathNode.isInRange = false;
                        pathNode.isPossibleMovePath = false;
                        node.isPossibleMovePath = false;
                    }
                }
            }
        }
    }



    // PUBLIC METHODS 
    public void CancelAction() {
        ClearSecondaryFlags();
        OnReachedTargetPosition(this, null);
        SetTurnState(ActionType.None);
    }

    public void SetTurnState(ActionType turnState) {
        this.turnState = turnState;

        if (turnState == ActionType.None) {
            OnCancelActionTriggered?.Invoke(player);
        }

        if (turnState == ActionType.MoveAction) {
            UpdateGridForPlayer(player.GetWorldPosition(),player.GetGridMoveRange(),player.GetRemainingGameMoveRange());
            OnMoveActionTriggered?.Invoke(player);
        }

        if (turnState == ActionType.MainAction) {
            UpdateGridForPlayer(player.GetWorldPosition(),player.GetGridMoveRange(),player.GetRemainingGameMoveRange());
            OnMainActionTriggered?.Invoke(player);
        }
    }

    public void SetState(State state) {
        this.state = state;
    }

    public State GetState() {
        return this.state;
    }

    public Pathfinding GetPathfinding() {
        return pathfinding;
    }

    public Grid<PathNode> GetGrid() {
        return grid;
    }

    public CombatState GetCombatState() {
        return combatState;
    }




}