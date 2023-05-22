using GameUtils;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTesting : MonoBehaviour {
    private float timer;

    void Start() {

    }

    void Update() {
        if (Mouse3D.GetInstance().IsMouseOverUI())
            return;

        Vector3 mouseWorldPos = Mouse3D.GetInstance().GetMouseWorldPosition();
        float sphereRadius = 1f; 
        if (Physics.SphereCast(mouseWorldPos + new Vector3(0,2,0), sphereRadius, Vector3.down, out RaycastHit hit, Mathf.Infinity)) {
            if (CheckForPlayer(hit, out System.Func<string> playerDescription)) {
                UI_Tooltip.ShowTooltip_Static(playerDescription);
            } else if (CheckForChest(hit, out System.Func<string> chestDescription)){
                UI_Tooltip.ShowTooltip_Static(chestDescription);
            } else {
                UI_Tooltip.HideTooltip_Static();

            }
        } else {
            UI_Tooltip.HideTooltip_Static();
        }
    }

    private bool CheckForPlayer(RaycastHit hit, out System.Func<string> getTooltipTextFunc) {
        Player player = hit.collider.GetComponent<Player>();
        if (player != null) {
            getTooltipTextFunc = () => {
                return player.GetDescription();
            };
            return true;
        } else {
            getTooltipTextFunc = () => {
                return null;
            };
            return false;
        }
    }
    private bool CheckForChest(RaycastHit hit, out System.Func<string> getTooltipTextFunc) {
        Chest chest = hit.collider.GetComponent<Chest>();
        if (chest != null) {
            getTooltipTextFunc = () => {
                return chest.GetDescription();
            };
            return true;
        } else {
            getTooltipTextFunc = () => {
                return null;
            };
            return false;
        }
    }


}