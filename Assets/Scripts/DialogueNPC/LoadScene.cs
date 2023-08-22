using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{

    private CharacterStateManager characterStateManager;
    public Text textDisplay;
    public string npcDialogue;

    private bool playerInRange = false;
    private void Start()
    {
        // Ki?m tra xem có m?t instance c?a CharacterStateManager trong scene không
        characterStateManager = FindObjectOfType<CharacterStateManager>();

        if (characterStateManager == null)
        {
            Debug.LogError("CharacterStateManager is missing in the scene.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Ki?m tra n?u nhân v?t va ch?m v?i m?t v?t th? nào ?ó
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            textDisplay.text = npcDialogue;
            textDisplay.gameObject.SetActive(true);
            /* // Chuy?n ??i sang scene khác
             SwitchScene("morgue");*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            textDisplay.gameObject.SetActive(false);
        }
    }
    // Ph??ng th?c ?? chuy?n ??i scene
    public void SwitchScene(string sceneName)
    {
        // L?u gi? giá tr? tr?ng thái nhân v?t tr??c khi chuy?n scene
        int characterState = CharacterStateManager.currentCharacterState;

        // Chuy?n scene
        SceneManager.LoadScene(sceneName);

        // Gán l?i giá tr? tr?ng thái nhân v?t sau khi chuy?n scene
        CharacterStateManager.currentCharacterState = characterState;
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.A))
        {
            // X? lý s? ki?n khi ng??i ch?i nh?n phím E khi ?ang g?n NPC
            // Ví d?: M? h?i tho?i, kích ho?t nhi?m v?, vv.
        }
    }

}
