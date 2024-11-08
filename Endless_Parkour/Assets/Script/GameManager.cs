using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Color platformHeaderColor;
    public Color playerColor = Color.white;

    [Header("Score info")]
    public int coins;
    public int coinsTotal;
    public float distance;
    public int score = 0;
    public int highScore = 0;

    public Player player;

    SpriteRenderer sr;

    void Awake()
    {
        instance = this;
        sr = player.GetComponent<SpriteRenderer>();
        LoadInfo();
    }

    void Update()
    {
        if (player.transform.position.x > distance) distance = player.transform.position.x;
    }

    #region Player Data
    public void SaveColor(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);  // Ép thành số nguyên 0-255
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int colorValue = (r << 16) | (g << 8) | b;
        PlayerPrefs.SetInt("CharacterColor", colorValue);
    }

    // Hàm tải dữ liệu
    public void LoadColor()
    {
        if (PlayerPrefs.HasKey("CharacterColor"))
        {
            int colorValue = PlayerPrefs.GetInt("CharacterColor");

            int r = (colorValue >> 16) & 0xFF;
            int g = (colorValue >> 8) & 0xFF;
            int b = colorValue & 0xFF;

            sr.color = new Color(r / 255f, g / 255f, b / 255f); // Chuyển đổi thành giá trị từ 0 đến 1
            return;
        }

        sr.color = Color.white; // Giá trị mặc định nếu chưa có dữ liệu
    }


    public int LoadCoins() => PlayerPrefs.GetInt("Coins", 0);

    void SaveCoins()
    {
        coinsTotal += coins;
        PlayerPrefs.SetInt("Coins", coinsTotal);
    }

    public void LoadScore(out string _lastScore, out string _highScore)
    {
        _lastScore = "Last score: " + score.ToString();
        _highScore = "High score: " + highScore.ToString();
    }
    void LoadScore()
    {
        if (PlayerPrefs.HasKey("Score")) score = PlayerPrefs.GetInt("Score", 0);
        if (PlayerPrefs.HasKey("HighScore")) highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void SaveScore()
    {
        score = (int)(distance / 10) + coins;
        PlayerPrefs.SetInt("Score", score);
        if (score > highScore) PlayerPrefs.SetInt("HighScore", score);
    }

    #endregion

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
        SaveInfo();
    }
    public void RunBegin() => player.runBegin = true;

    void LoadInfo()
    {
        coinsTotal = LoadCoins();
        LoadScore();
        LoadColor();
    }

    void SaveInfo()
    {
        SaveCoins();
        SaveScore();
        SaveColor(sr.color);
        PlayerPrefs.Save();
    }
}
