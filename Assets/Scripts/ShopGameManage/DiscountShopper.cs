using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DiscountShopper : MonoBehaviour, Interactable
{
    public List<ItemGameShop> itemsGame = new List<ItemGameShop>(); // Danh sách vật phẩm trong cửa hàng
    List<ItemGameShop> inventory = new List<ItemGameShop>();
    public Dictionary<string, int> boughtItems = new Dictionary<string, int>(); // Vật phẩm đã mua và số lần mua
    public int money;
    public TMP_Text textMoney;
    public GameObject ToActive;

    private List<ItemGameShop> items = new List<ItemGameShop>();
    public GameObject itemPrefab;
    GameObject g;
    public GameObject scrollView;
    Button buyButton;

    public GameObject inventoryItemPrefab;
    public GameObject scrollViewInventory;
    GameObject b;

    public GameObject Checkout;
    public GameObject ConfirmPannel;
    public TMP_Text NotificationText;
    double totalPrice;
    public string prompt;

    int price1, price2, price3, price4;

    public string InteractionPromp => prompt;
    public class ItemGameShop
    {
        public string name;
        public string imageLink;
        public int price;
        public bool discounted;
        public int quantity;
    }

    void Start()
    {
        money = Random.Range(50, 100) * 100;
        price1 = Random.Range(1, 21) * 100;
        price2 = Random.Range(1, 21) * 100;
        price3 = Random.Range(1, 21) * 100;
        price4 = Random.Range(1, 21) * 100;
        textMoney.text = money.ToString();
        AddItemInList();
    }
    public void AddItemInList()
    {
        CreateList();
        LoadList(items);
    }
    public void LoadList(List<ItemGameShop> item)
    {
        if (item != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Debug.Log("STT: " + i);
                g = Instantiate(itemPrefab, scrollView.transform);
                StartCoroutine(LoadImageFromURL(items[i].imageLink, g.transform.GetChild(0).GetChild(1).GetComponent<Image>()));
                g.transform.GetChild(1).GetComponent<TMP_Text>().text = items[i].name;
                g.transform.GetChild(4).GetComponent<TMP_Text>().text = items[i].price.ToString();
                buyButton = g.transform.GetComponent<Button>();
                buyButton.AddEventListener(i, LoadItem);
            }
        }
    }

    public void CreateList()
    {
        items = new List<ItemGameShop> {

            new ItemGameShop
            {
                name = "Mirinda",
                imageLink = "https://i.imgur.com/PJtoJOt.png",
                price = price1,
            },
            new ItemGameShop
            {
                name = "Sting",
                imageLink = "https://i.imgur.com/rijm7gp.png",
                price = price2,
            },
            new ItemGameShop
            {
                name = "Coca",
                imageLink = "https://i.imgur.com/YVIRoyG.png",
                price = price3,
            },
            new ItemGameShop
            {
                name = "Pepsi",
                imageLink = "https://i.imgur.com/WO9zTH3.png",
                price = price4,
            }
        };
    }

    IEnumerator LoadImageFromURL(string url, Image targetImage)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Kiểm tra nếu đối tượng Image không bị hủy bỏ
                if (targetImage != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    targetImage.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Image is null or destroyed, cannot assign sprite.");
                }
            }
            else
            {
                Debug.LogError("Error loading image: " + www.error);
            }
        }
    }


    public void LoadItem(int itemIndex)
    {
        AddItemToInventory(itemIndex);

    }

    public void AddItemToInventory(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < items.Count)
        {
            ItemGameShop itemToAdd = items[itemIndex];
            Debug.Log("Item choose: " + itemToAdd.name);
            bool foundInInventory = false;

            // Tìm kiếm vật phẩm trong danh sách inventory
            foreach (ItemGameShop inventoryItem in inventory)
            {
                if (inventoryItem.name == itemToAdd.name)
                {
                    // Nếu vật phẩm đã có trong inventory, tăng quantity lên thêm 1
                    Debug.Log("+1");
                    inventoryItem.quantity++;
                    foundInInventory = true;
                    break;
                }
            }

            // Nếu vật phẩm chưa có trong inventory, thêm vào danh sách
            if (!foundInInventory)
            {
                Debug.Log("Add new");
                itemToAdd.quantity = 1;
                inventory.Add(itemToAdd);
            }

            // Cập nhật hiển thị inventory sau khi thêm vật phẩm
            RefreshInventoryDisplay();
        }
        else
        {
            Debug.Log("Index vật phẩm không hợp lệ.");
        }
    }

    // Hàm cập nhật hiển thị inventory
    private void RefreshInventoryDisplay()
    {
        ClearScrollViewChildren();

        for (int i = 0; i < inventory.Count; i++)
        {
            b = Instantiate(inventoryItemPrefab, scrollViewInventory.transform);

            StartCoroutine(LoadImageFromURL(inventory[i].imageLink, b.transform.GetChild(0).GetChild(1).GetComponent<Image>()));
            b.transform.GetChild(1).GetComponent<TMP_Text>().text = inventory[i].quantity.ToString();
        }
    }

    public void ClearScrollViewChildren()
    {
        Transform contentTransform = scrollViewInventory.transform;

        // Duyệt qua tất cả các child và xóa chúng
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CalculateTotalPrice()
    {
        totalPrice = 0;
        double itemPrice = 0;
        double totalItemPrice = 0;

        foreach (ItemGameShop item in inventory)
        {

            // Áp dụng công thức tính giá tiền dựa trên số lượng
            for (int i = 0; i < item.quantity; i++)
            {
                itemPrice = item.price / Mathf.Pow(2, i);
                totalItemPrice = totalItemPrice + itemPrice;
            }

            totalPrice += itemPrice;
        }
        Debug.Log("Total Price: " + totalItemPrice);
        CheckWinCondition(totalItemPrice);
    }

    public bool CheckWinCondition(double point)
    {
        if (point > money)
        {
            ConfirmPannel.gameObject.SetActive(true);
            NotificationText.text = "Bạn đã mua vượt quá " + (point - money) + " so với số tiền quy định";
            return false;
        }
        else if ((money - point) > (money * 0.25))
        {
            ConfirmPannel.gameObject.SetActive(true);
            NotificationText.text = "Bạn chưa sử dụng hơn 75% số tiền quy định";
            return false;
        }
        else
        {
            ConfirmPannel.gameObject.SetActive(true);
            NotificationText.text = "Chúc mừng bạn đã vượt qua mini game, đã sử dụng " + point + "/" + money;
            return true;
        }
    }
    public void OpenGame()
    {
        ToActive.gameObject.SetActive(true);
    }
    public void CloseGame()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
        ToActive.gameObject.SetActive(false);
    }

    public void TryAgain()
    {
        ConfirmPannel.gameObject.SetActive(false);
        inventory.Clear();
        ClearScrollViewChildren();
    }

    public bool Interact(Interactor interactor)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
        OpenGame();
        return true;
    }
}
