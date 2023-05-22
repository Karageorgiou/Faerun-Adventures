using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Player player;


    private void Awake() {
        player = GetComponent<Player>();
    }

    public string GetTooltipText() {
        if (player != null) {
            return player.GetDescription();
        }
        else {
            return "Player is null";
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("POINTER ENTER");
        /*System.Func<string> getTooltipTextFunc = () => {
            return GetTooltipText();
        };
        UI_Tooltip.ShowTooltip_Static(getTooltipTextFunc);*/
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("POINTER EXIT");

        /*UI_Tooltip.HideTooltip_Static();*/
    }
}