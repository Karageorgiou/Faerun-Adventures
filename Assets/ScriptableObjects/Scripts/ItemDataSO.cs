using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/Item Data")]
public class ItemDataSO : ScriptableObject {
    public List<Item> items;

    private void Reset() {
        items = new List<Item>();
    }

    public ItemDataSO(ItemDataSO itemDataSO) {
        this.items = itemDataSO.items;
    }
}
