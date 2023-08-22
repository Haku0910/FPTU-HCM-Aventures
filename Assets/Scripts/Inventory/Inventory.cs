using System.Collections.Generic;
using UnityEngine;

public class Inventory : MultiplayerSingleton<Inventory>
{
  

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;


    // Our current list of items in the inventory
    public List<Item> items = new List<Item>();

    // Add a new item if enough room
    public void Add(Item item)
    {
        if (item.showInInventory)
        {
            items.Add(item);
            Debug.Log("Item" + item);
            Debug.Log("items" + items.Count);
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }
    public void UpdateItem(Item oldItem, Item newItem)
    {
        if (oldItem.showInInventory && newItem.showInInventory)
        {
            int index = items.IndexOf(oldItem);
            if (index != -1)
            {
                items[index] = newItem;
                Debug.Log("Item updated: " + newItem.name);
                if (onItemChangedCallback != null)
                    onItemChangedCallback.Invoke();
            }
            else
            {
                Debug.LogWarning("Item not found in inventory: " + oldItem.name);
            }
        }
    }
    //Update

    // Remove an item
    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
