using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitingScript : MonoBehaviourPunCallbacks
{
    public GameObject canvaswait;
    public GameObject currentCamera;
    public GameObject mainCamera;
    public GameObject fixedJoystick;
    public GameObject touchField;
    public GameObject sprintButton;
    public ButtonManager buttonManager;
    public Slider slider;
    public float totalTime = 3f;
    private float currentTime = 0f;

    public RotateAround rotateAround;

    private bool isWaiting = false;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal && photonView.IsMine)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
            canvaswait.gameObject.SetActive(true);
            slider.value = 0f;
            StartSlider();
        }
        else
        {
            // Tắt chức năng màn hình loading cho các người chơi khác
            fixedJoystick.gameObject.SetActive(false);
            touchField.gameObject.SetActive(false);
            sprintButton.gameObject.SetActive(false);

            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });

            canvaswait.gameObject.SetActive(false);
            currentCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(false);
        }
    }

    public void StartSlider()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal && !isWaiting && photonView.IsMine)
        {
            Debug.Log("start slider");
            StartCoroutine(AnimateSlider());
        }
    }

    private IEnumerator AnimateSlider()
    {
        currentTime = 0f;
        isWaiting = true;

        // Lặp trong vòng totalTime giây
        while (currentTime < totalTime)
        {
            // Tính toán giá trị mới cho slider dựa trên thời gian đã trôi qua
            float progress = currentTime / totalTime;
            slider.value = progress;

            // Chờ một frame
            yield return null;

            // Cập nhật thời gian đã trôi qua
            currentTime += Time.deltaTime;
        }

        // Đảm bảo slider có giá trị là 1 sau khi kết thúc vòng lặp
        slider.value = 1f;
        canvaswait.gameObject.SetActive(false);
        rotateAround.StartCameraCinema();
        isWaiting = false;
    }
}
