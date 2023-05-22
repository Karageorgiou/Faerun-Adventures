using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainActionBar : MonoBehaviour {
    private Transform mainActionSlotTemplate;
    private Transform background;
    private Transform title;

    private UI_MainActionBarActionSlot actionSlot;


    private void Awake() {
        mainActionSlotTemplate = transform.Find("MainActionTemplate");
        if (mainActionSlotTemplate != null) {
            actionSlot = mainActionSlotTemplate.GetComponent<UI_MainActionBarActionSlot>();
        }
    }



}
