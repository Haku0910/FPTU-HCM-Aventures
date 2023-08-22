using TMPro;
using UnityEngine;

public class AddChatUI : MonoBehaviour
{

    #region Singlton:AddChatUI

    public static AddChatUI Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    #endregion

    public TMP_Text name;
    public TMP_Text mess;
    // Start is called before the first frame update


}
