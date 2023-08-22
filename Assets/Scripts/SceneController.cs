using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject playerPrefab;
    public Vector3 spawnLocation;
    public static SceneController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
