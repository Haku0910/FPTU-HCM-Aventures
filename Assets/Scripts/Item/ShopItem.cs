using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, Interactable
{
    #region Singlton:Shop

    public static ShopItem Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        ShopItemsList = new List<Shop>(); // Kh?i t?o danh s�ch ShopItemsList
        StartCoroutine(FetchDataAndShowItems());

    }

    #endregion


    private Player player1;
    private List<Shop> ShopItemsList;
    [SerializeField] Animator NoCoinsAnim;


    [SerializeField] GameObject ItemTemplate;
    GameObject g;
    [SerializeField] Transform ShopScrollView;
    [SerializeField] GameObject ShopPanel;
    [SerializeField] GameObject buyItem;
    Button buyBtn;

    [SerializeField] private string prompt;


    public string InteractionPromp => prompt;

    void Start()
    {
    }
    //get Item Inventory 


    private IEnumerator FetchDataAndShowItems()
    {

        // ??i cho d? li?u t? API ???c l?y
        yield return StartCoroutine(GetDataFromAPI());

        // Sau khi c� d? li?u, ??t bi?n flag l� true v� hi?n th? c�c m?c
        yield return StartCoroutine(GetItemFromApi());
    }
    private IEnumerator GetDataFromAPI()
    {
        // G?i h�m ?? th?c hi?n y�u c?u API v� l?y danh s�ch c�c m?c
        yield return StartCoroutine(ItemApi.Instance.GetItemsFromAPI());
    }
    private IEnumerator GetItemFromApi()
    {
        List<ItemData> itemdatas = ItemApi.Instance.items;
        Debug.Log("Count" + itemdatas.Count);
        if (ShopItemsList.Count != itemdatas.Count)
        {
            ShopItemsList.Clear();
            for (int i = 0; i < itemdatas.Count; i++)
            {
                ShopItemsList.Add(new Shop());
            }
        }
        for (int i = 0; i < itemdatas.Count; i++)
        {
            ShopItemsList[i].Id = itemdatas[i].id;
            ShopItemsList[i].Price = itemdatas[i].price;
            ShopItemsList[i].Name = itemdatas[i].name;
            ShopItemsList[i].Quanity = itemdatas[i].quantity;
            yield return StartCoroutine(LoadImageFromURL(ShopItemsList[i], itemdatas[i].imageUrl));
        }
        ShowAllItems();

    }
    private void ShowAllItems()
    {
        int count = ItemApi.Instance.items.Count;
        Debug.Log("Count" + count);

        for (int i = 0; i < count; i++)
        {
            g = Instantiate(ItemTemplate, ShopScrollView);
            g.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = ShopItemsList[i].Image;
            g.transform.GetChild(1).GetComponent<TMP_Text>().text = ShopItemsList[i].Name;
            buyBtn = g.transform.GetComponent<Button>();
            g.transform.GetChild(4).GetComponent<TMP_Text>().text = ShopItemsList[i].Price.ToString();
            /*if (ShopItemsList[i].IsPurchased == true)
            {
                DisableBuyButton();
            }*/
            buyBtn.AddEventListener(i, OnShopItemBtnClicked);
        }
    }
    private IEnumerator LoadImageFromURL(Shop shopItem, string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                shopItem.Image = sprite; // C?p nh?t h�nh ?nh cho m?c
            }
            else
            {

                Debug.Log("L?i khi t?i h�nh ?nh t? URL: " + webRequest.error);
            }
        }
    }

    void OnShopItemBtnClicked(int itemIndex)
    {
        buyItem.SetActive(true);
        BuyItem.Instance.Buy(ShopItemsList[itemIndex]);
    }


    private void OnPlayer(Player player)
    {
        player1 = player;
    }


    /*---------------------Open & Close shop--------------------------*/
    public void OpenShop()
    {
        ShopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        ShopPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }

    public bool Interact(Interactor interactor)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
        OpenShop();
        return true;
    }
}
