using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

    

    public enum ItemType {
        None,
        Coin,
        Key,
    }

    public string title;
    public ItemType itemType;
    public int amount;
    public float cost;
    public float weight;

    public bool isConsumable;
    public bool isStackable;

    public Sprite sprite;

    public Item(string title, ItemType itemType, int amount, float cost, float weight, bool isConsumable, bool isStackable, Sprite sprite) {
        this.title = title;
        this.itemType = itemType;
        this.amount = amount;
        this.cost = cost;
        this.weight = weight;
        this.isConsumable = isConsumable;
        this.isStackable = isStackable;
        this.sprite = sprite;
    }

    public Sprite GetSprite() {
        return sprite;
    }

} 
  