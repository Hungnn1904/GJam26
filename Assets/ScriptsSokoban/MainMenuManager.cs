using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện bắt buộc để chuyển cảnh

public class MainMenuManager : MonoBehaviour
{
    [Header("Cấu hình chuyển cảnh")]
    [Tooltip("Tên của Scene tiếp theo (Ví dụ: Day0 hoặc Gameplay)")]
    public string nextSceneName;

    // Hàm dành cho nút PLAY
    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Reset lại thời gian (đề phòng trường hợp game trước bị Pause)
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Bạn chưa nhập tên Scene vào Inspector!");
        }
    }

    // Hàm dành cho nút QUIT
    public void QuitGame()
    {
        Debug.Log("Đã thoát game!"); // Chỉ hiển thị trong cửa sổ Console của Unity
        Application.Quit(); // Thoát ứng dụng (chỉ có tác dụng khi build ra file .exe hoặc .apk)
    }
}