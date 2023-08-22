using UnityEngine;
using UnityEngine.UI;

public class PlayerRunController : MonoBehaviour
{
    [SerializeField] private CharacterMoving characterMoving;
    [SerializeField] private Button sprintButton;
    private bool isSprinting = false;

    private void Start()
    {
        // Gán sự kiện khi người chơi nhấn và nhả nút bật chế độ chạy nhanh
        sprintButton.onClick.AddListener(OnSprintButtonPressed);
        sprintButton.onClick.AddListener(OnSprintButtonReleased);
    }

    private void OnSprintButtonPressed()
    {
        isSprinting = true;
        characterMoving.SetSprinting(isSprinting);
    }

    private void OnSprintButtonReleased()
    {
        isSprinting = false;
        characterMoving.SetSprinting(isSprinting);
    }
}
