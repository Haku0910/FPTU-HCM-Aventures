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
        // Ki?m tra xem c� m?t instance c?a CharacterStateManager trong scene kh�ng
        characterStateManager = FindObjectOfType<CharacterStateManager>();

        if (characterStateManager == null)
        {
            Debug.LogError("CharacterStateManager is missing in the scene.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Ki?m tra n?u nh�n v?t va ch?m v?i m?t v?t th? n�o ?�
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            textDisplay.text = npcDialogue;
            textDisplay.gameObject.SetActive(true);
            /* // Chuy?n ??i sang scene kh�c
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
        // L?u gi? gi� tr? tr?ng th�i nh�n v?t tr??c khi chuy?n scene
        int characterState = CharacterStateManager.currentCharacterState;

        // Chuy?n scene
        SceneManager.LoadScene(sceneName);

        // G�n l?i gi� tr? tr?ng th�i nh�n v?t sau khi chuy?n scene
        CharacterStateManager.currentCharacterState = characterState;
    }
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.A))
        {
            // X? l� s? ki?n khi ng??i ch?i nh?n ph�m E khi ?ang g?n NPC
            // V� d?: M? h?i tho?i, k�ch ho?t nhi?m v?, vv.
        }
    }

}
