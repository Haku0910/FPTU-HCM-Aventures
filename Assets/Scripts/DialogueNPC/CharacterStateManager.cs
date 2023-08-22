using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    public static int currentCharacterState = 0;

    public string currentOutfit; // Bi?n l?u tr?ng th�i trang ph?c
    public Vector3 currentPosition; // Bi?n l?u v? tr� hi?n t?i c?a nh�n v?t

    // Bi?n t?nh ?? truy c?p th�ng qua c�c script kh�c
    public static CharacterStateManager instance;

    private void Awake()
    {
        // ??m b?o ch? c� m?t instance c?a CharacterStateManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ??m b?o kh�ng b? h?y khi chuy?n scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ph??ng th?c ?? l?u tr?ng th�i v� v? tr� c?a nh�n v?t
    public void SaveCharacterState()
    {
        PlayerPrefs.SetString("Outfit", currentOutfit);
        PlayerPrefs.SetFloat("PosX", currentPosition.x);
        PlayerPrefs.SetFloat("PosY", currentPosition.y);
        PlayerPrefs.SetFloat("PosZ", currentPosition.z);
    }

    // Ph??ng th?c ?? kh�i ph?c tr?ng th�i v� v? tr� c?a nh�n v?t
    public void RestoreCharacterState()
    {
        currentOutfit = PlayerPrefs.GetString("Outfit");
        float posX = PlayerPrefs.GetFloat("PosX");
        float posY = PlayerPrefs.GetFloat("PosY");
        float posZ = PlayerPrefs.GetFloat("PosZ");
        currentPosition = new Vector3(posX, posY, posZ);
    }
}
