using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour
{
    private bool isTaskCompleted = false;

    // G?i h�m n�y khi nh�n v?t ho�n th�nh nhi?m v?
    public void CompleteTask()
    {
        isTaskCompleted = true;
        // Th?c hi?n c�c h�nh ??ng sau khi ho�n th�nh nhi?m v?
        // V� d?: hi?n th? th�ng b�o, t?ng ?i?m, v.v.
    }

    // G?i h�m n�y ?? ki?m tra tr?ng th�i ho�n th�nh nhi?m v? c?a nh�n v?t
    public bool IsTaskCompleted()
    {
        return isTaskCompleted;
    }
}
