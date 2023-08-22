using Photon.Pun;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rb;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // G?i thông tin v? trí và h??ng quay c?a nhân v?t t? máy tính ng??i ch?i ??n các máy tính khác trong phòng
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Nh?n thông tin v? trí và h??ng quay c?a nhân v?t t? các máy tính khác trong phòng
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
