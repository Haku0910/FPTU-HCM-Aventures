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
            // G?i th�ng tin v? tr� v� h??ng quay c?a nh�n v?t t? m�y t�nh ng??i ch?i ??n c�c m�y t�nh kh�c trong ph�ng
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Nh?n th�ng tin v? tr� v� h??ng quay c?a nh�n v?t t? c�c m�y t�nh kh�c trong ph�ng
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
