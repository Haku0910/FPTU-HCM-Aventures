using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour
{

    public Image icon;
    public TMP_Text quantityText;
    public GameObject textImage;
    Item item;  // Current item in the slot

    // Add item to the slot
    public void AddItem(Item newItem)
    {
        Debug.Log("New Item " + item);
        item = newItem;
        textImage.SetActive(true);
        quantityText.text = item.quantity.ToString();
        icon.sprite = item.icon;
        icon.enabled = true;
    }
    public void OnItemClick()
    {
        if (item != null)
        {
            ItemDescription.instance.ShowItemDetails(item);
        }
    }
    // Clear the slot
    public void ClearSlot()
    {
        item = null;
        quantityText.text = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // If the remove button is pressed, this function will be called.
    public void RemoveItemFromInventory()
    {
        Inventory.Instance.Remove(item);
    }

    // Use the item
    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

}
