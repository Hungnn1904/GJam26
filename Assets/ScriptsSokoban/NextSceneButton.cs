using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Direct Load")]
    public string directSceneName; // đi thẳng tới scene nếu có tên

    [Header("Hub Settings")]
    public string hubSceneName = "MorningScene";
    public string[] sokobanScenes =
    {
        "Sokoban2",
        "Sokoban3",
        "Sokoban4"
    };

    public void Next()
    {
        // ===== ƯU TIÊN LOAD THEO TÊN =====
        if (!string.IsNullOrEmpty(directSceneName))
        {
            SceneManager.LoadScene(directSceneName);
            return;
        }

        string current = SceneManager.GetActiveScene().name;

        // ===== HUB =====
        if (current == hubSceneName)
        {
            int step = PlayerPrefs.GetInt("HubStep", 0);

            string target = sokobanScenes[step];

            step++;
            if (step >= sokobanScenes.Length)
                step = 0;

            PlayerPrefs.SetInt("HubStep", step);

            SceneManager.LoadScene(target);
            return;
        }

        // ===== TỪ SOKOBAN VỀ HUB =====
        foreach (string s in sokobanScenes)
        {
            if (current == s)
            {
                SceneManager.LoadScene(hubSceneName);
                return;
            }
        }

        // ===== STORY NEXT =====
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }
}
