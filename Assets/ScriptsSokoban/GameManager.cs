using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string nextSceneName; 
    public LayerMask goalLayer;
    public TextMeshProUGUI timerText;
    public float timeLimit = 60f;

    private float currentTime;
    private bool isWon = false;

    void Awake() { instance = this; }

    void Start() { currentTime = timeLimit; }

    void Update()
    {
        if (isWon) return;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (timerText) timerText.text = "Time: " + Mathf.Ceil(currentTime) + "s";
        }
    }

 public void CheckWinCondition()
{
    // Tìm tất cả hòm trong scene (kể cả hòm đang ẩn để tránh lỗi)
    BoxLogic[] allBoxes = Object.FindObjectsByType<BoxLogic>(FindObjectsSortMode.None);
    int remainingBoxes = 0;

    foreach (BoxLogic box in allBoxes)
    {
        if (box.gameObject.activeSelf)
        {
            // Nếu hòm đang active mà chạm đúng đích -> Ẩn đi
            if (box.IsInCorrectGoal(goalLayer))
            {
                box.gameObject.SetActive(false); 
                Debug.Log($"Hòm {box.boxID} đã biến mất!");
            }
            else
            {
                // Nếu hòm chưa vào đích hoặc vào sai đích thì vẫn tính là còn hòm
                remainingBoxes++;
            }
        }
    }

    // Nếu không còn hòm nào active -> Thắng
    if (remainingBoxes == 0 && !isWon)
    {
        isWon = true;
        Debug.Log("Màn chơi hoàn tất!");
        Invoke("LoadNextScene", 1.0f); // Đợi 1s cho người chơi kịp nhìn
    }
}

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName);
        else Debug.Log("Hoàn thành tất cả các màn!");
    }
}