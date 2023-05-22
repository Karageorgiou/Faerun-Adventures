using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MoveActionBar : MonoBehaviour {
    private Transform moveActionSlotTemplate;
    private Transform background;
    private Transform title;

    private UI_MoveActionBarActionSlot actionSlot;

    private void Awake() {
        moveActionSlotTemplate = transform.Find("MoveActionTemplate");
        if (moveActionSlotTemplate != null) {
            actionSlot = moveActionSlotTemplate.GetComponent<UI_MoveActionBarActionSlot>();
        }
    }
}
