using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cấu hình màn chơi")]
    public string nextSceneName; 
    public LayerMask goalLayer;
    public float timeLimit = 30f; 

    [Header("Giao diện (UI)")]
    public GameObject rulesPanel;    // Đối tượng cha của Rules
    public GameObject endsPanel;     // Đối tượng cha của Ends
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI rewardText; // Kéo Text nằm trong Ends vào đây

    private float currentTime;
    private bool isWon = false;
    private bool isGameStarted = false; // Kiểm tra đã đóng Rules chưa

    void Awake() { instance = this; }

    void Start() 
    { 
        currentTime = timeLimit;
        isWon = false;
        isGameStarted = false;

        // Ép trạng thái ban đầu
        if (rulesPanel != null) rulesPanel.SetActive(true);
        if (endsPanel != null) endsPanel.SetActive(false);
        
        // Dừng game để người chơi đọc luật
        Time.timeScale = 0; 
    }

    void Update()
    {
        if (!isGameStarted || isWon) return;

        currentTime -= Time.deltaTime;
        UpdateTimerUI();
    }

    public void CloseRules() // HÀM NÀY GÁN VÀO BUTTON "TIẾP TỤC"
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false);
            isGameStarted = true;
            Time.timeScale = 1; // Chạy game
        }
    }

    void UpdateTimerUI()
    {
        if (currentTime > 0)
        {
            timerText.text = "Time: " + Mathf.Ceil(currentTime) + "s";
            timerText.color = (currentTime <= 10) ? Color.red : Color.white;
        }
        else
        {
            timerText.text = "Quá giờ! (+1 vàng)";
            timerText.color = Color.gray;
        }
    }

    public void CheckWinCondition()
    {
        BoxLogic[] allBoxes = Object.FindObjectsByType<BoxLogic>(FindObjectsSortMode.None);
        int activeBoxes = 0;

        foreach (BoxLogic box in allBoxes)
        {
            if (box.gameObject.activeSelf)
            {
                if (box.IsInCorrectGoal(goalLayer)) box.gameObject.SetActive(false);
                else activeBoxes++;
            }
        }

        if (activeBoxes == 0 && !isWon)
        {
            isWon = true;
            ShowEndScreen();
        }
    }

    void ShowEndScreen()
    {
        int totalGold = 1; // Mặc định 1 vàng hoàn thành
        if (currentTime >= 10) totalGold = 3; // +2 thưởng
        else if (currentTime > 0) totalGold = 2; // +1 thưởng

        if (endsPanel != null)
        {
            endsPanel.SetActive(true);
            if (rewardText != null) 
                rewardText.text = "Bạn nhận được " + totalGold + " vàng!";
        }
        
        // Tùy chọn: Chờ 3 giây rồi sang màn
        Invoke("LoadNextScene", 3f);
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName);
    }
}