using Photon.Pun;
using UnityEngine;

public class MultiplayerSingleton<T> : MonoBehaviourPunCallbacks where T : Component
{
    private static T[] instances; // M?ng l?u các instance riêng bi?t cho t?ng ng??i ch?i

    public static T Instance
    {
        get
        {
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            if (instances == null)
            {
                instances = new T[PhotonNetwork.CurrentRoom.MaxPlayers];
            }

            if (instances[playerIndex] == null)
            {
                instances[playerIndex] = FindObjectOfType<T>();
                if (instances[playerIndex] == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instances[playerIndex] = obj.AddComponent<T>();
                }
            }
            return instances[playerIndex];
        }
    }

    protected virtual void Awake()
    {
        if (instances == null)
        {
            instances = new T[PhotonNetwork.CurrentRoom.MaxPlayers];
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (instances[playerIndex] == null)
        {
            instances[playerIndex] = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (instances != null && playerIndex >= 0 && playerIndex < instances.Length && instances[playerIndex] != null)
        {
            Destroy(instances[playerIndex].gameObject);
            instances[playerIndex] = null;
        }
    }
}
