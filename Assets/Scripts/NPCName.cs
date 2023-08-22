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
        // T?o m?t b?n sao c?a prefab "nameTextPrefab" v� g�n n� v�o transform cha c?a NPC
        nameTextObject = Instantiate(nameTextPrefab, transform.parent);
        nameTextTransform = nameTextObject.transform;

        // L?y th�nh ph?n Text t? th�nh ph?n con c?a nameTextObject
        nameText = nameTextObject.GetComponentInChildren<Text>();

        // ??t t�n c?a NPC v�o th�nh ph?n Text trong v?n b?n t�n
        if (nameText != null)
        {
            nameText.text = npcName;
        }

        // C?p nh?t v? tr� v� h??ng c?a v?n b?n t�n
        nameTextTransform.position = transform.position + Vector3.up * 1f;

        animator.SetBool("isTalking", true);
    }
}
