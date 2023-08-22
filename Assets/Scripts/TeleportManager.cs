using Photon.Pun;
using UnityEngine;

public class TeleportManager : MonoBehaviourPun
{
    private Vector3 targetPosition;

    public void TeleportPlayer(Vector3 newPosition)
    {
        // Tính toán vị trí mới mà bạn muốn chuyển người chơi đến
        targetPosition = newPosition;

        // Gửi thông tin vị trí mới cho tất cả các máy khách khác
        photonView.RPC("TeleportRPC", RpcTarget.OthersBuffered, targetPosition);
    }

    [PunRPC]
    private void TeleportRPC(Vector3 newPosition)
    {
        // Nhận thông tin vị trí mới và cập nhật vị trí của người chơi trên máy khách
        if (!photonView.IsMine)
        {
            transform.position = newPosition;
        }
    }
}
