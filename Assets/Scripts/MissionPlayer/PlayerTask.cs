using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour
{
    private bool isTaskCompleted = false;

    // G?i hàm này khi nhân v?t hoàn thành nhi?m v?
    public void CompleteTask()
    {
        isTaskCompleted = true;
        // Th?c hi?n các hành ??ng sau khi hoàn thành nhi?m v?
        // Ví d?: hi?n th? thông báo, t?ng ?i?m, v.v.
    }

    // G?i hàm này ?? ki?m tra tr?ng thái hoàn thành nhi?m v? c?a nhân v?t
    public bool IsTaskCompleted()
    {
        return isTaskCompleted;
    }
}
