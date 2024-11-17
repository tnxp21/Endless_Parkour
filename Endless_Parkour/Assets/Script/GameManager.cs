using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Purchased color")]
    [SerializeField] Material[] skyBoxMat;

    [Header("Purchased color")]
    public Color platformHeaderColor;

    public UI_Manage uiManage;


    [Header("Score info")]
    public int coins = 0;
    public int coinsTotal;
    public float distance = 0f;
    public int score = 0;
    public int highScore = 0;

    public Player player;

    public SpriteRenderer sr;

    void Awake()
    {
        Time.timeScale = 1;
        instance = this;
        sr = player.GetComponent<SpriteRenderer>();
        SetupSkyBox(PlayerPrefs.GetInt("SkyBoxSetting", 0));
        LoadInfo();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        if (player.transform.position.x > distance) distance = player.transform.position.x;
    }

    #region Player Data
    public void SaveColor(Color color, string colorType)
    {
        int r = Mathf.RoundToInt(color.r * 255);  // Ép thành số nguyên 0-255
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int colorValue = (r << 16) | (g << 8) | b;
        PlayerPrefs.SetInt(colorType, colorValue);
        PlayerPrefs.Save();
    }

    // Hàm tải dữ liệu
    public void LoadColor()
    {
        sr.color = Color.white; // Giá trị mặc định nếu chưa có dữ liệu
        platformHeaderColor = Color.yellow;
        if (PlayerPrefs.HasKey("PlayerColor"))
        {
            int colorValue = PlayerPrefs.GetInt("PlayerColor");

            int r = (colorValue >> 16) & 0xFF;
            int g = (colorValue >> 8) & 0xFF;
            int b = colorValue & 0xFF;

            sr.color = new Color(r / 255f, g / 255f, b / 255f); // Chuyển đổi thành giá trị từ 0 đến 1
        }
        if (PlayerPrefs.HasKey("PlatformColor"))
        {
            int colorValue = PlayerPrefs.GetInt("PlatformColor");

            int r = (colorValue >> 16) & 0xFF;
            int g = (colorValue >> 8) & 0xFF;
            int b = colorValue & 0xFF;

            platformHeaderColor = new Color(r / 255f, g / 255f, b / 255f); // Chuyển đổi thành giá trị từ 0 đến 1
        }
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

    public void RunBegin() => player.runBegin = true;

    public void SetupSkyBox(int i)
    {
        if(i < skyBoxMat.Length)
        RenderSettings.skybox = skyBoxMat[i];
        else
            RenderSettings.skybox = skyBoxMat[Random.Range(0, skyBoxMat.Length)];
        PlayerPrefs.SetInt("SkyBoxSetting", i);
    }

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
        PlayerPrefs.Save();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void GameEnded()
    {
        SaveInfo();
        uiManage.OpenEndGameMenu();
    }
}
