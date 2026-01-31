using UnityEngine;
using UnityEngine.UI; // Nếu bạn dùng Legacy Text
using TMPro; // Nếu bạn dùng TextMeshPro (Khuyên dùng)

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public TextMeshProUGUI timerText; 
    public bool timerIsRunning = false;

    void Start() { timerIsRunning = true; }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                // Có thể thêm xử lý khi hết thời gian mà chưa xong
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("Thời gian: {0:00}s", seconds);
        
        // Đổi màu đỏ khi dưới 10s để tạo áp lực
        if (timeRemaining < 10) timerText.color = Color.red;
    }
}