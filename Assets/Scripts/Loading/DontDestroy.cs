using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Đặt biến static để lưu trữ instance của đối tượng này
    [SerializeField] private static DontDestroy instance;
    [SerializeField] private GameObject gameObject;

    void Start()
    {
        // Kiểm tra xem đã tồn tại một instance của đối tượng chưa
        if (instance == null)
        {
            // Nếu chưa, đặt instance là đối tượng hiện tại
            instance = this;

            // Sử dụng hàm DontDestroyOnLoad để đảm bảo đối tượng này không bị xóa khi chuyển scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã có một instance khác tồn tại, hủy đối tượng hiện tại
            Destroy(gameObject);
        }
    }
}
