using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public int Score;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = Score.ToString();
    }
    public void ResetScore()
    {
        Score = 0;
    }

    public void AddScore(int add)
    {
        Score = Score + add;

    }
}
