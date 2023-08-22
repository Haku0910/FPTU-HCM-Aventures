using UnityEngine;
using UnityEngine.UI;

public class NPCName : MonoBehaviour
{
    public string npcName;
    public GameObject nameTextPrefab;
    private Transform nameTextTransform;
    private GameObject nameTextObject;
    private Text nameText;
    [SerializeField] private Animator animator;

    private void Start()
    {
        // T?o m?t b?n sao c?a prefab "nameTextPrefab" và gán nó vào transform cha c?a NPC
        nameTextObject = Instantiate(nameTextPrefab, transform.parent);
        nameTextTransform = nameTextObject.transform;

        // L?y thành ph?n Text t? thành ph?n con c?a nameTextObject
        nameText = nameTextObject.GetComponentInChildren<Text>();

        // ??t tên c?a NPC vào thành ph?n Text trong v?n b?n tên
        if (nameText != null)
        {
            nameText.text = npcName;
        }

        // C?p nh?t v? trí và h??ng c?a v?n b?n tên
        nameTextTransform.position = transform.position + Vector3.up * 1f;

        animator.SetBool("isTalking", true);
    }
}
