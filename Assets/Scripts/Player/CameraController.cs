using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetObject; // Đối tượng chỉ định để xoay xung quanh
    public float rotationSpeed = 5f; // Tốc độ xoay của camera

    private Vector3 offset; // Khoảng cách từ camera đến đối tượng
    private bool isTouching = false;
    private Vector2 lastTouchPosition;

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Vui lòng gắn đối tượng cần xoay vào targetObject.");
            return;
        }

        // Tính khoảng cách ban đầu từ camera đến đối tượng
        offset = transform.position - targetObject.position;
    }

    private void Update()
    {
        // Xử lý xoay camera khi người chơi di chuyển ngón tay trên màn hình
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isTouching = true;
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                // Tính toán khoảng chênh lệch giữa vị trí cũ và vị trí mới của ngón tay
                float deltaX = touch.position.x - lastTouchPosition.x;

                // Tính toán góc quay mới dựa trên sự di chuyển của ngón tay và tốc độ xoay
                float angle = deltaX * rotationSpeed;

                // Xoay camera xung quanh đối tượng theo góc quay tính toán được
                Vector3 newPosition = Quaternion.Euler(0f, angle, 0f) * offset;
                transform.position = targetObject.position + newPosition;

                // Đảm bảo camera nhìn thẳng vào đối tượng
                transform.LookAt(targetObject);

                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouching = false;
            }
        }
    }
}
