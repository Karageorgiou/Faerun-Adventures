
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_Inventory : MonoBehaviour {
    private Inventory inventory;
    private Player player;

    private Transform titleTransform;
    private TextMeshProUGUI nameTextMeshProUgui;

    private Transform itemSlotContainerTransform;
    private Transform itemSlotTemplateTransform;

    public void SetPlayer(Player player) {
        FindName();
        FindContainer();
        FindTemplate();

        this.player = player;
        SetInventory(this.player.GetInventory());


        
        nameTextMeshProUgui.SetText(this.player.name);

        
    }

    private void FindContainer() {
        itemSlotContainerTransform = transform.Find("ItemSlotContainer");
        if (itemSlotContainerTransform != null) {

        }
        else {
            Debug.LogWarning("ItemSlotContainer was not found");

        }
    }

    private void FindTemplate() {
        itemSlotTemplateTransform = itemSlotContainerTransform.Find("ItemSlotTemplate");
        if (itemSlotTemplateTransform != null) {

        }
        else {
            Debug.LogWarning("ItemSlotTemplate was not found");

        }
    }

    private void FindName() {
        titleTransform = transform.Find("Title");
        if (titleTransform != null) {

            nameTextMeshProUgui = titleTransform.GetComponentInChildren<TextMeshProUGUI>();
            if (nameTextMeshProUgui == null) {
                Debug.LogWarning("textMeshProUGUI component was not found");
            }

        } else {
            Debug.LogWarning("Title was not found");
        }

    }

    private void SetInventory(Inventory inventory) {
        this.inventory = inventory;
        inventory.ItemAdded += Inventory_ItemAdded;
        inventory.ItemRemoved += Inventory_ItemRemoved;
        itemSlotTemplateTransform.gameObject.SetActive(false);
        UpdateInventoryItems();
        
    }

    

    private void UpdateInventoryItems() {
        foreach (Transform child in itemSlotContainerTransform) {
            if (child == itemSlotTemplateTransform) {
                continue;
            }
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 80f;

        foreach (Item item in inventory.GetItemList()) {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplateTransform, itemSlotContainerTransform).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Icon").GetComponent<Image>();
            if (item.GetSprite() != null) {
                image.sprite = item.GetSprite();
            }
            else {
                Color currentColor = image.color;
                currentColor.a = 0f;
                image.color = currentColor;
            }
            
            TextMeshProUGUI amountText = itemSlotRectTransform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1) {
                amountText.SetText(item.amount.ToString());
            }
            else {
                amountText.SetText("");
            }

            x++;
            if (x > 2) {
                x = 0;
                y--;
            }
        }
    }





    private void Inventory_ItemRemoved(Item item) {
        UpdateInventoryItems();
    }

    private void Inventory_ItemAdded(Item item) {
        UpdateInventoryItems();
    }

}
 