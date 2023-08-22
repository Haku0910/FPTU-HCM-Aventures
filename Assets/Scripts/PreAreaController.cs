using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreAreaController : MonoBehaviour
{
    public Button acceptButton;
    public Button cancelButton;
    public GameObject avatarPlayer;

    string sceneName = null;
    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Current scene:" + sceneName);
        if (other.CompareTag("Player"))
        {
            // Hiển thị nút "Đồng ý" và "Hủy"
            Debug.Log("hiển thị");
            acceptButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }
    }

    public virtual void AcceptButtonClicked()
    {
        if (sceneName.Equals("Playground"))
        {
            Debug.Log("Load Cinema scene");
            SceneManager.LoadScene("CinemaScreen");
            avatarPlayer.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Load Playgroud scene");
            SceneManager.LoadScene("Playground");
            avatarPlayer.gameObject.SetActive(true);
        }
    }

    public virtual void CancelButtonClicked()
    {
        // Ẩn nút "Đồng ý" và "Hủy"
        Debug.Log("Hủy");
        acceptButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }
}
