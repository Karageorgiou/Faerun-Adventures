using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    private List<Item> itemList;

    public event Action<Item> ItemAdded;
    public event Action<Item> ItemRemoved;


    public Inventory() {
        itemList = new List<Item>();
        Debug.Log("Inventory initialized.");


        //--- remove this later
        if (ItemSystem.GetInstance().ReceiveItem("Gold Piece", 20, out Item goldPiece)) {
            AddItem(goldPiece);
        }
        if (ItemSystem.GetInstance().ReceiveItem("Silver Piece", 8, out Item silverPiece)) {
            AddItem(silverPiece);
        }
        if (ItemSystem.GetInstance().ReceiveItem("Copper Piece", 1, out Item copperPiece)) {
            AddItem(copperPiece);
        }
        //--------------------
    }

    public void AddItem(Item item) {
        if (item.isStackable) {
            bool itemAlreadyExists = false;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.title == item.title) {
                    inventoryItem.amount += item.amount;
                    itemAlreadyExists = true;
                }
            }
            if (!itemAlreadyExists) {
                itemList.Add(item);
            }
        }
        else { 
            itemList.Add(item); 
        } 
        ItemAdded?.Invoke(item);
    }

    public void RemoveItem(Item item) {
        itemList.Remove(item);
        ItemRemoved?.Invoke(item);
    }

    public void UseItem(Item item) {
        if (itemList.Count > 0 && itemList.Contains(item)) {
            Item inventoryItem = itemList.Find(i => i == item);
            if (inventoryItem.isConsumable) {
                inventoryItem.amount--;
            }
            //inventoryItem.UseItem();
            if (inventoryItem.amount < 1) {
                RemoveItem(inventoryItem);    
            }

        } else {
            Debug.Log("Item doen't exist in this itemList.");
        }
         
    }

    public List<Item> GetItemList() {
        return itemList;
    }
}
  