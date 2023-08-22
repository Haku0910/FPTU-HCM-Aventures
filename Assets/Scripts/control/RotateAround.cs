using Photon.Pun;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform target; // Đối tượng mà camera sẽ xoay quanh
    public float rotationSpeed = 100.0f;
    public GameObject currentCamera;
    public GameObject MainCamera;

    private bool isRotating = false;
    private float totalRotation = 0.0f;
    public ManageButton manageButton;
    public ButtonManager buttonManager;
    private PhotonView photonView;
    private void Update()
    {
        photonView = GetComponent<PhotonView>();
        if (isRotating && photonView.IsMine)
        {
            RotateCamera();
        }
    }

    public void StartCameraCinema()
    {
        // Kiểm tra nếu đã thực hiện quay camera rồi
        if (PlayerPrefs.GetInt("CameraRotated", 0) == 1)
        {
            Debug.Log("nguoi e iu 1");
            isRotating = false;
            // Vô hiệu hóa component RotateCameraAround
            if (!photonView.IsMine)
            {
                currentCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(false);
            }
            else
            {
                currentCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(true);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
        }
        else
        {
            Debug.Log("nguoi e iu 2");
            isRotating = true;
        }

    }

    private void RotateCamera()
    {
        Vector3 rotationAxis = Vector3.up;
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.RotateAround(target.position, rotationAxis, rotationAmount);
        totalRotation += rotationAmount;

        // Kiểm tra nếu đã xoay xong một vòng
        if (totalRotation >= 360.0f)
        {
            isRotating = false;

            if (!photonView.IsMine)
            {
                currentCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(false);
            }
            else
            {
                currentCamera.gameObject.SetActive(false);
                MainCamera.gameObject.SetActive(true);
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });

            PlayerPrefs.SetInt("CameraRotated", 1);

        }

    }
}
