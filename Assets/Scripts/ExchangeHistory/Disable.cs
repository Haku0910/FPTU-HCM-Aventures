using UnityEngine;

public class Disable : MonoBehaviour
{
    public GameObject canvasGame;

    private static Disable instance;
    public static Disable Instance { get; private set; }

    private void Awake()
    {
        // Ki?m tra n?u ?� c� m?t instance t?n t?i, n?u c� th� h?y b?n th�n
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Kh?i t?o instance
        Instance = this;

        // ??m b?o r?ng instance kh�ng b? h?y khi chuy?n c?nh
        DontDestroyOnLoad(gameObject);
    }


    public void DisableGame()
    {
        canvasGame.SetActive(false);
    }

    public void EnableGame()
    {
        canvasGame.SetActive(true);
    }
}
