using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageButton : MonoBehaviour
{
    public GameObject mainCamera;
    [SerializeField] private GameObject toActiveReference;

    public static void Create(Transform parent, Vector3 localPosition)
    {
        // T?o m?t b?n sao c?a prefab "pfChatBubble" t? GameAssets v� g�n n� v�o transform "parent" ?� cho
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfButtonMessage, parent);

        // ??t v? tr� c?c b? c?a h?p tho?i chat b?ng "localPosition" ?� cho
        chatBubbleTransform.localPosition = localPosition;


        // X�a b?n sao h?p tho?i chat sau 6 gi�y
        Destroy(chatBubbleTransform.gameObject, 8f);
    }
    private Button buttonUI;
    private SpriteRenderer backgroundSpriteRenderer;

    private void Awake()
    {
        Debug.Log("UIMessageButton Awake");

        buttonUI = transform.Find("ButtonMessage").GetComponent<Button>();
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        buttonUI.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        ShowToActive();
    }
    public void SetToActiveReference(GameObject toActive)
    {
        Debug.Log("vao la chinh xavc");

        toActiveReference = toActive;
    }
    public void ShowToActive()
    {
        Debug.Log("vao la chinh xav nhedsa");

    }

}
