using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour {
    private static ItemSystem Instance;

    [SerializeField] private ItemDataSO baseItemData;

    private ItemDataSO baseDataCopy;

    private void Awake() {
        Instance = this;
        baseDataCopy = new ItemDataSO(baseItemData);
    }

    public static ItemSystem GetInstance() {
        return Instance;
    }


    public bool ReceiveItem(string requestedItemTitle, int requestedAmount, out Item outItem) {
        foreach (Item inventoryItem in baseDataCopy.items) {
            if (inventoryItem.title == requestedItemTitle) {
                outItem = new Item(inventoryItem.title, inventoryItem.itemType, requestedAmount, inventoryItem.cost,
                    inventoryItem.weight, inventoryItem.isConsumable, inventoryItem.isStackable, inventoryItem.sprite);
                Debug.Log("Found " + requestedItemTitle);
                return true;
            }
        }
        Debug.Log("Can't find " + requestedItemTitle);
        outItem = null;
        return false;
    }
}
