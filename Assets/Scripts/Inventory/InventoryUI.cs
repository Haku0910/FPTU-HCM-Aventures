using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/* This object manages the inventory UI. */

public class InventoryUI : MultiplayerSingleton<InventoryUI>
{



    GameObject g;
    [SerializeField] private Transform itemsParent;   // The parent object of all the items
    [SerializeField] private Image uiLoadingCircleImage;
    [SerializeField] private Animator loadingCircleAnimator;
    [SerializeField] private GameObject buttonBag;
    [SerializeField] private GameObject inventoryUI;  // The entire UI
    Inventory inventory;    // Our current inventory
    [SerializeField] private GameObject slotPrefab; // Tham chi?u t?i Prefab Slot
    List<Item> items;
    private List<Sprite> loadedSprites = new List<Sprite>();
    Button buyBtn;
    private List<ItemInventoryData> itemInventoryDatas;
    private ItemDescription itemDetailPanel; // Reference to the ItemDetailPanel prefab

    private bool isDataLoaded = false; // Bi?n ki?m tra xem d? li?u ?� ???c t?i xong hay ch?a

    private int IS_ROTATING_ANIM_PARAM;

    private ItemData ItemData;
    private void Start()
    {
        IS_ROTATING_ANIM_PARAM = Animator.StringToHash("IsRotating");
        SetLoadingCircleAnimation(true);
        StartCoroutine(GetDataInventoryAPI());
    }
    private IEnumerator GetDataInventoryAPI()
    {
        yield return StartCoroutine(GetlistInventoryByPlayerName(OnGetItemInventoryList));
        yield return StartCoroutine(UpdateInventoryFromAPI());

    }
    public IEnumerator GetlistInventoryByPlayerName(Action<List<ItemInventoryData>> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/ItemInventorys/iteminventory/{PhotonNetwork.LocalPlayer.NickName}";

        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                // Parse JSON response to extract "data" array
                Wrapper<GetItemInventoryByPlayer> wrapper = JsonUtility.FromJson<Wrapper<GetItemInventoryByPlayer>>(response);
                // Call the callback function with the majorName
                Debug.Log("data " + wrapper.data.listItem);
                callback?.Invoke(wrapper.data.listItemInventory);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }

    private void OnGetItemInventoryList(List<ItemInventoryData> listitemInventoryDatas)
    {
        itemInventoryDatas = listitemInventoryDatas;
    }


    public void SetActive()
    {
        // Show the bag button and hide the inventory UI
        uiLoadingCircleImage.gameObject.SetActive(true);
        SetLoadingCircleAnimation(true);
        // Call UpdateInventoryFromAPI to update the inventory data
        StartCoroutine(UpdateInventoryFromAPI());

        /*  // Get the Inventory instance
          inventory = Inventory.instance;

          // Add a callback to UpdateUI whenever the inventory is changed
          inventory.onItemChangedCallback += UpdateUI;*/
    }

    private void SetLoadingCircleAnimation(bool animate)
    {
        loadingCircleAnimator.SetBool(IS_ROTATING_ANIM_PARAM, animate);
    }

    public IEnumerator UpdateInventoryFromAPI()
    {
        // Get the item data from the API
        var itemsApi = itemInventoryDatas;

        if (itemsApi != null)
        {
            // If the item data is not null, start loading sprites from URLs
            for (int i = 0; i < itemsApi.Count; i++)
            {
                Debug.Log("itemInventory.itemId: " + itemsApi[i].itemId);
                yield return StartCoroutine(ItemApi.Instance.CheckItemById(itemsApi[i].itemId, OnGetItem));
                Debug.Log(ItemData.imageUrl);
                Debug.Log("Description " + ItemData.description);

                yield return StartCoroutine(LoadSpriteFromURL(ItemData.imageUrl, (sprite) =>
                {
                    // Add the loaded sprite to the temporary list of loadedSprites
                    loadedSprites.Add(sprite);

                    // Check if all sprites have been loaded from the API
                    if (loadedSprites.Count <= itemsApi.Count)
                    {


                        Item newItem = new Item
                        {
                            description = ItemData.description,
                            name = ItemData.name,
                            icon = loadedSprites[i],
                            quantity = itemsApi[i].quantity,
                            // Add other item data that you want to copy from the ItemInventory
                        };
                        Inventory.Instance.Add(newItem);

                    }
                }));
            }
            // Call UpdateUI after the current frame to avoid lag
            StartCoroutine(UpdateUIAfterFrame());
            uiLoadingCircleImage.gameObject.SetActive(false);
            SetLoadingCircleAnimation(false);
        }
        else
        {
            Debug.Log("items is null");
            SetLoadingCircleAnimation(isDataLoaded);
        }
    }
    private void OnGetItem(ItemData itemData)
    {
        ItemData = itemData;
    }
    private IEnumerator UpdateUIAfterFrame()
    {
        // Wait for the end of the current frame before updating the UI
        yield return new WaitForEndOfFrame();

        // Update the UI after adding items to the inventory
        UpdateUI();

        // Mark data as loaded
        isDataLoaded = true;

        // Update the animation of the loading circle
        SetLoadingCircleAnimation(!isDataLoaded);
    }

    private IEnumerator LoadSpriteFromURL(string url, Action<Sprite> callback)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load image from URL: " + www.error);
            }
            else
            {
                // Tạo texture từ dữ liệu đã tải về
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Tạo sprite từ texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                Debug.Log(sprite);
                // Gọi lại hàm callback và truyền sprite đã tạo
                callback.Invoke(sprite);
            }
        }
    }
    public void UpdateUI()
    {
        // Get all the inventory slots in the UI
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Tạo mới các slot dựa trên số lượng item trong inventory
        for (int i = 0; i < Inventory.Instance.items.Count; i++)
        {
            // Instantiate prefab và gán nó vào parent (itemsParent)
            GameObject newSlotGO = Instantiate(slotPrefab, itemsParent);

            // Lấy component InventorySlot từ GameObject mới tạo
            InventorySlot newSlot = newSlotGO.GetComponent<InventorySlot>();

            // Gán thông tin của item từ inventory vào slot
            newSlot.AddItem(Inventory.Instance.items[i]);

            buyBtn = newSlotGO.transform.GetComponent<Button>();

            buyBtn.AddEventListener(i, OnShopItemBtnClicked);

        }

        // Data is loaded, update the animation of the loading circle
        SetLoadingCircleAnimation(!isDataLoaded);
    }

    void OnShopItemBtnClicked(int itemIndex)
    {
        ItemDescription.instance.ShowItemDetails(Inventory.Instance.items[itemIndex]);
    }
}