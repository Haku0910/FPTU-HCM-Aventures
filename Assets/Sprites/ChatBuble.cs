using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBuble : MonoBehaviour
{
    public static void Create(Transform parent, Vector3 localPosition, string text)
    {
        // T?o m?t b?n sao c?a prefab "pfChatBubble" t? GameAssets và gán nó vào transform "parent" ?ã cho
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent);

        // ??t v? trí c?c b? c?a h?p tho?i chat b?ng "localPosition" ?ã cho
        chatBubbleTransform.localPosition = localPosition;

        // G?i ph??ng th?c "Setup" trên ChatBubble c?a b?n sao h?p tho?i chat, truy?n vào n?i dung v?n b?n
        chatBubbleTransform.GetComponent<ChatBuble>().Setup(text);

        // Xóa b?n sao h?p tho?i chat sau 6 giây
        Destroy(chatBubbleTransform.gameObject, 2f);
    }

    private SpriteRenderer backgroundSpriteRenderer;
    private Transform backgroundCube;
    private TextMeshPro textMeshPro;
    private void Awake()
    {
        // L?y tham chi?u ??n SpriteRenderer c?a n?n t? transform con có tên "Background"
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();

        // L?y tham chi?u ??n Transform c?a kh?i n?n t? transform con có tên "BackgroundCube"
        backgroundCube = transform.Find("BackgroundCube");

        // L?y tham chi?u ??n TextMeshPro t? transform con có tên "Text"
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

    }

    private void Setup(string text)
    {
        // ??t v?n b?n cho textMeshPro
        textMeshPro.SetText(text);

        // C?p nh?t mesh c?a textMeshPro
        textMeshPro.ForceMeshUpdate();

        // L?y kích th??c c?a v?n b?n ?ã ???c hi?n th?
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        // ??nh ngh?a padding cho n?n h?p tho?i
        Vector2 padding = new Vector2(7f, 3f);

        // C?p nh?t kích th??c c?a backgroundSpriteRenderer ?? phù h?p v?i kích th??c c?a v?n b?n và padding
        backgroundSpriteRenderer.size = textSize + padding;

        // C?p nh?t kích th??c c?a backgroundCube ?? phù h?p v?i kích th??c c?a v?n b?n và padding
        backgroundCube.localScale = textSize + padding * .5f;

        // ??nh ngh?a offset ?? ?i?u ch?nh v? trí c?a n?n h?p tho?i
        Vector3 offset = new Vector3(-3f, 0f);

        // ??t v? trí local c?a backgroundSpriteRenderer d?a trên kích th??c c?a nó, offset và v? trí xem tr?c x
        backgroundSpriteRenderer.transform.localPosition =
            new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;

        // ??t v? trí local c?a backgroundCube d?a trên kích th??c c?a nó, offset và v? trí xem tr?c x và z
        backgroundCube.localPosition =
            new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f, +.1f) + offset;

        // Thêm vi?c hi?n th? v?n b?n theo t?ng ch? cái vào textMeshPro v?i t?c ?? .03f và các tùy ch?n khác
        TextWriter.AddWriter_Static(textMeshPro, text, .03f, true, true, () => { });
    }

    public void Test()
    {
        Debug.Log("vao day la chinh xaxc");
    }

}
