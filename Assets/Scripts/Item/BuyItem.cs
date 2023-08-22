using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MultiplayerSingleton<BuyItem>
{
    public TMP_Text NameItem;
    public TMP_Text GoldItem;
    public Button buyItem;
    public Button buttonClose;
    public Image imageItem;
    private Shop itemInShop;
    public GameCoins gameCoins;

    public GameObject SuccessConfirm;
    public TMP_Text successText;
    public GameObject FailConfirm;
    public TMP_Text failText;

    public void BuyItemChange()
    {
        if (gameCoins.HasEnoughCoins(itemInShop.Price))
        {

            itemInShop.IsPurchased = true;


            var inventoryId = NamePrefab.Instance.inventories.inventoryId;
            Debug.Log("Inventory" + inventoryId);
            var listItems = NamePrefab.Instance.inventories.listItem;

            if (listItems.Count == 0)
            {

                var item = new Item
                {
                    icon = itemInShop.Image,
                    name = itemInShop.Name,
                    quantity = 1,
                    showInInventory = true
                };
                Inventory.Instance.Add(item);
                var itemInventory = new ItemInventoryData
                {
                    inventoryId = inventoryId,
                    itemId = itemInShop.Id,
                    quantity = 1,
                };
                Debug.Log("itemInventory" + itemInventory);
                string jsonDataItemInventory = JsonUtility.ToJson(itemInventory);

                StartCoroutine(ItemInventoryDataApi.Instance.CreateItemInventory(jsonDataItemInventory));
                var exchangeHistory = new ExchangeHistoryData
                {
                    itemId = itemInShop.Id,
                    playerId = NamePrefab.Instance.inventories.playerId,
                    quantity = itemInventory.quantity
                };
                string json = JsonUtility.ToJson(exchangeHistory);
                StartCoroutine(ExchangeHistoryAPI.Instance.PostRequest(json));
            }
            else
            {
                var itemName = listItems.FirstOrDefault(x => x.name == itemInShop.Name);
                if (itemName == null)
                {
                    var item = new Item
                    {
                        icon = itemInShop.Image,
                        name = itemInShop.Name,
                        quantity = 1,
                        showInInventory = true
                    };
                    Inventory.Instance.Add(item);
                    var itemInventory = new ItemInventoryData
                    {
                        inventoryId = inventoryId,
                        itemId = itemInShop.Id,
                        quantity = 1,
                    };
                    string jsonDataItemInventory = JsonUtility.ToJson(itemInventory);
                    StartCoroutine(ItemInventoryDataApi.Instance.CreateItemInventory(jsonDataItemInventory));
                    var exchangeHistory = new ExchangeHistoryData
                    {
                        itemId = itemInShop.Id,
                        playerId = NamePrefab.Instance.inventories.playerId,
                        quantity = itemInventory.quantity

                    };
                    string json = JsonUtility.ToJson(exchangeHistory);
                    StartCoroutine(ExchangeHistoryAPI.Instance.PostRequest(json));

                    SuccessConfirm.gameObject.SetActive(true);
                    successText.text = "Bạn đã mua thành cộng 1 " + itemInShop.Name;
                }
                else
                {
                    var oldItem = Inventory.Instance.items.FirstOrDefault(x => x.name == itemName.name);
                    Debug.Log("Old Item" + oldItem.quantity);
                    var newItem = new Item()
                    {
                        icon = oldItem.icon,
                        name = oldItem.name,
                        quantity = oldItem.quantity + 1,
                        showInInventory = true
                    };
                    Debug.Log("New Item" + newItem.quantity);

                    Inventory.Instance.UpdateItem(oldItem, newItem);
                    StartCoroutine(ItemInventoryDataApi.Instance.GetItemInventoryByItemName(itemName.name, (itemData) =>
                    {
                        itemData.quantity += 1;
                        itemData.itemId = itemName.id;
                        StartCoroutine(ItemInventoryDataApi.Instance.UpdateItemInventory(itemData));

                    }));
                    StartCoroutine(ExchangeHistoryAPI.Instance.GetExchangHistoryByItemName(itemName.name, (itemData) =>
                    {
                        itemData.quantity += 1;
                        itemData.itemId = itemName.id;
                        itemData.playerId = NamePrefab.Instance.inventories.playerId;
                        Debug.Log(itemData.id);
                        StartCoroutine(ExchangeHistoryAPI.Instance.UpdateExchange(itemData));

                    }));
                    SuccessConfirm.gameObject.SetActive(true);
                    successText.text = "Bạn đã mua thành cộng 1 " + itemInShop.Name;
                }
            }
            gameCoins.UseCoins(itemInShop.Price);


            //add avatar
        }
        else
        {
            FailConfirm.gameObject.SetActive(true);
            failText.text = "Số tiền của bạn không đủ!!";
            Debug.Log("You don't have enough coins!!");
        }
    }

    public void Buy(Shop shopItem)
    {
        itemInShop = shopItem;
        NameItem.text = shopItem.Name;
        GoldItem.text = shopItem.Price.ToString();
        imageItem.sprite = shopItem.Image;

    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void CloseConfirm()
    {
        SuccessConfirm.gameObject.SetActive(false);
        FailConfirm.gameObject.SetActive(false);
    }

}
