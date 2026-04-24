using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Lives")]
    public Image[] lifeImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Game Over")]
    public GameObject gameOverPanel;

    [Header("Victory")]
    public GameObject victoryPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        scoreText.text = "Score: " + PlayerStats.Score;
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void UpdateTextScore(int amount)
    {
        PlayerStats.Score += amount;
        scoreText.text = "Score: " + PlayerStats.Score;
    }

    public void UpdateLives(int currentHealth)
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].sprite = i < currentHealth ? fullHeart : emptyHeart;
        }
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
        MusicManager.instance?.PlayVictoryMusic();
    }

    public void ReturnToMenu()
    {
        PlayerStats.Score = 0;
        SceneManager.LoadScene(0);
    }
}
