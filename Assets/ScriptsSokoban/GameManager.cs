using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cấu hình màn chơi")]
    public int totalTargets; 
    public LayerMask goalLayer; 
    public float timeLimit = 30f;

    [Header("Âm thanh")]
    public AudioSource audioSource; // Kéo AudioSource vào đây
    public AudioClip tingSound;     // Kéo file nhạc Ting Ting vào đây

    [Header("Giao diện UI")]
    public TextMeshProUGUI timerText;
    public GameObject winPanel; 
    public TextMeshProUGUI rewardText;

    private float currentTime;
    private bool isWon = false;
    private int lastBoxCount = 0; // Để theo dõi số hòm vừa vào đích

    void Awake() { instance = this; }

    void Start()
    {
        currentTime = timeLimit;
        if(winPanel != null) winPanel.SetActive(false);
        // Nếu chưa có AudioSource thì tự thêm
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (isWon) return;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Thời gian: " + Mathf.Ceil(currentTime).ToString() + "s";
            if (currentTime <= 10) timerText.color = Color.red;
        }
    }

    public void CheckWinCondition()
    {
        int currentCount = 0;
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject box in boxes)
        {
            Collider2D hitGoal = Physics2D.OverlapCircle(box.transform.position, 0.1f, goalLayer);
            if (hitGoal != null) currentCount++;
        }

        // PHẦN QUAN TRỌNG: Nếu số hòm ở đích tăng lên thì phát tiếng Ting
        if (currentCount > lastBoxCount)
        {
            audioSource.PlayOneShot(tingSound);
        }
        lastBoxCount = currentCount;

        if (currentCount >= totalTargets && !isWon)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        isWon = true;
        if(winPanel != null) winPanel.SetActive(true);
        int gold = (currentTime > 0) ? 3 : 2;
        rewardText.text = "BẠN ĐÃ XONG VIỆC!\nNhận được: " + gold + " xu vàng";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}