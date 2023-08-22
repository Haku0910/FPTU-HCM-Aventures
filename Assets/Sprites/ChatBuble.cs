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
        // T?o m?t b?n sao c?a prefab "pfChatBubble" t? GameAssets v� g�n n� v�o transform "parent" ?� cho
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, parent);

        // ??t v? tr� c?c b? c?a h?p tho?i chat b?ng "localPosition" ?� cho
        chatBubbleTransform.localPosition = localPosition;

        // G?i ph??ng th?c "Setup" tr�n ChatBubble c?a b?n sao h?p tho?i chat, truy?n v�o n?i dung v?n b?n
        chatBubbleTransform.GetComponent<ChatBuble>().Setup(text);

        // X�a b?n sao h?p tho?i chat sau 6 gi�y
        Destroy(chatBubbleTransform.gameObject, 2f);
    }

    private SpriteRenderer backgroundSpriteRenderer;
    private Transform backgroundCube;
    private TextMeshPro textMeshPro;
    private void Awake()
    {
        // L?y tham chi?u ??n SpriteRenderer c?a n?n t? transform con c� t�n "Background"
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();

        // L?y tham chi?u ??n Transform c?a kh?i n?n t? transform con c� t�n "BackgroundCube"
        backgroundCube = transform.Find("BackgroundCube");

        // L?y tham chi?u ??n TextMeshPro t? transform con c� t�n "Text"
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

    }

    private void Setup(string text)
    {
        // ??t v?n b?n cho textMeshPro
        textMeshPro.SetText(text);

        // C?p nh?t mesh c?a textMeshPro
        textMeshPro.ForceMeshUpdate();

        // L?y k�ch th??c c?a v?n b?n ?� ???c hi?n th?
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        // ??nh ngh?a padding cho n?n h?p tho?i
        Vector2 padding = new Vector2(7f, 3f);

        // C?p nh?t k�ch th??c c?a backgroundSpriteRenderer ?? ph� h?p v?i k�ch th??c c?a v?n b?n v� padding
        backgroundSpriteRenderer.size = textSize + padding;

        // C?p nh?t k�ch th??c c?a backgroundCube ?? ph� h?p v?i k�ch th??c c?a v?n b?n v� padding
        backgroundCube.localScale = textSize + padding * .5f;

        // ??nh ngh?a offset ?? ?i?u ch?nh v? tr� c?a n?n h?p tho?i
        Vector3 offset = new Vector3(-3f, 0f);

        // ??t v? tr� local c?a backgroundSpriteRenderer d?a tr�n k�ch th??c c?a n�, offset v� v? tr� xem tr?c x
        backgroundSpriteRenderer.transform.localPosition =
            new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;

        // ??t v? tr� local c?a backgroundCube d?a tr�n k�ch th??c c?a n�, offset v� v? tr� xem tr?c x v� z
        backgroundCube.localPosition =
            new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f, +.1f) + offset;

        // Th�m vi?c hi?n th? v?n b?n theo t?ng ch? c�i v�o textMeshPro v?i t?c ?? .03f v� c�c t�y ch?n kh�c
        TextWriter.AddWriter_Static(textMeshPro, text, .03f, true, true, () => { });
    }

    public void Test()
    {
        Debug.Log("vao day la chinh xaxc");
    }

}
