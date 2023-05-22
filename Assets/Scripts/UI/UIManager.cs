using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour {
    private static UIManager Instance;

    [SerializeField] private Transform inventoryTemplate;
    private int playerCount = 0;

    private void Awake() {
        if (inventoryTemplate != null) {
            inventoryTemplate.gameObject.SetActive(false);
        }
    }


    private void Start() {
        EntityHandler.GetInstance().OnPlayerEntityAdded += UIManager_OnPlayerEntityAdded;
    }

    private void UIManager_OnPlayerEntityAdded(Player player) {
        GenerateInventoryVisual(player);
    }

    public static UIManager GetInstance() {
        return Instance;
    }



    public void GenerateInventoryVisual(Player player) {
        float startPosition = Screen.width;


        Transform inventoryTransform = Instantiate(inventoryTemplate, transform);
        

        UI_Inventory inventoryUI = inventoryTransform.GetComponent<UI_Inventory>();
        inventoryUI.SetPlayer(player);

        RectTransform playerPortraitRectTransform = inventoryTransform.GetComponent<RectTransform>();
        //playerPortraitRectTransform.anchoredPosition = new Vector2(0f, startPosition + (-150f * i));
        playerPortraitRectTransform.anchoredPosition = new Vector2(startPosition + (-240f * playerCount), 0f);
        inventoryTransform.gameObject.SetActive(true);
        playerCount++;


    }


}
