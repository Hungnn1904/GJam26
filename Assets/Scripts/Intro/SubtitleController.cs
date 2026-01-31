using TMPro;
using UnityEngine;
using System.Collections;

public class SubtitleController : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;

    [TextArea(2, 3)]
    public string[] lines;

    public float showTime = 2.5f;   // thời gian hiện chữ
    public float gapTime = 0.5f;    // khoảng nghỉ giữa các dòng

    void Start()
    {
        subtitleText.text = "";
        StartCoroutine(PlaySubtitles());
    }

    IEnumerator PlaySubtitles()
    {
        foreach (string line in lines)
        {
            subtitleText.text = line;
            yield return new WaitForSeconds(showTime);

            subtitleText.text = "";
            yield return new WaitForSeconds(gapTime);
        }
    }
}
