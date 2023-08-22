using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemQuantityText;
    public Image icon;
    #region Singleton

    public static ItemDescription instance;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
    public void ShowItemDetails(Item item)
    {
        this.gameObject.SetActive(true);

        itemNameText.text = item.name;
        itemDescriptionText.text = item.description;
        itemQuantityText.text = item.quantity.ToString();
        icon.sprite = item.icon;
        gameObject.SetActive(true);
    }

    // ?n UI Panel
    public void HideItemDetails()
    {
        gameObject.SetActive(false);
    }
}
