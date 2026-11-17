using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Text scoreText;
    private Text coinsText;
    private Text highScoreText;
    private GameObject gameOverPanel;
    private GameObject playButton;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateDefaultLayout()
    {
        var scoreGO = new GameObject("ScoreText");
        scoreGO.transform.SetParent(transform, false);
        scoreText = scoreGO.AddComponent<Text>();
        scoreText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        scoreText.fontSize = 42;
        scoreText.alignment = TextAnchor.UpperLeft;
        scoreText.rectTransform.anchorMin = new Vector2(0f, 1f);
        scoreText.rectTransform.anchorMax = new Vector2(0f, 1f);
        scoreText.rectTransform.anchoredPosition = new Vector2(40f, -30f);
        scoreText.color = Color.white;

        var coinGO = new GameObject("CoinsText");
        coinGO.transform.SetParent(transform, false);
        coinsText = coinGO.AddComponent<Text>();
        coinsText.font = scoreText.font;
        coinsText.fontSize = 36;
        coinsText.alignment = TextAnchor.UpperRight;
        coinsText.rectTransform.anchorMin = new Vector2(1f, 1f);
        coinsText.rectTransform.anchorMax = new Vector2(1f, 1f);
        coinsText.rectTransform.anchoredPosition = new Vector2(-40f, -30f);
        coinsText.color = Color.yellow;

        var hsGO = new GameObject("HighScoreText");
        hsGO.transform.SetParent(transform, false);
        highScoreText = hsGO.AddComponent<Text>();
        highScoreText.font = scoreText.font;
        highScoreText.fontSize = 28;
        highScoreText.alignment = TextAnchor.UpperCenter;
        highScoreText.rectTransform.anchorMin = new Vector2(0.5f, 1f);
        highScoreText.rectTransform.anchorMax = new Vector2(0.5f, 1f);
        highScoreText.rectTransform.anchoredPosition = new Vector2(0f, -30f);
        highScoreText.color = new Color(1f, 0.75f, 0.75f);

        gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(transform, false);
        var panelImage = gameOverPanel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.5f);
        var panelRT = gameOverPanel.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0f, 0f);
        panelRT.anchorMax = new Vector2(1f, 1f);
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;
        gameOverPanel.SetActive(false);

        var gameOverText = new GameObject("GameOverText");
        gameOverText.transform.SetParent(gameOverPanel.transform, false);
        var goText = gameOverText.AddComponent<Text>();
        goText.font = scoreText.font;
        goText.fontSize = 54;
        goText.alignment = TextAnchor.MiddleCenter;
        goText.text = "Metro Rush India\nGame Over";
        goText.color = Color.white;
        var goTextRT = goText.rectTransform;
        goTextRT.anchorMin = new Vector2(0.5f, 0.6f);
        goTextRT.anchorMax = new Vector2(0.5f, 0.6f);
        goTextRT.sizeDelta = new Vector2(520f, 160f);

        playButton = new GameObject("PlayButton");
        playButton.transform.SetParent(gameOverPanel.transform, false);
        var btnImage = playButton.AddComponent<Image>();
        btnImage.color = new Color(0.3f, 0.8f, 0.4f, 1f);
        var btnRT = playButton.GetComponent<RectTransform>();
        btnRT.anchorMin = new Vector2(0.5f, 0.3f);
        btnRT.anchorMax = new Vector2(0.5f, 0.3f);
        btnRT.sizeDelta = new Vector2(210f, 60f);

        var btnText = new GameObject("ButtonText");
        btnText.transform.SetParent(playButton.transform, false);
        var btnTextComp = btnText.AddComponent<Text>();
        btnTextComp.font = scoreText.font;
        btnTextComp.fontSize = 26;
        btnTextComp.alignment = TextAnchor.MiddleCenter;
        btnTextComp.text = "Restart";
        btnTextComp.color = Color.white;
        var btnTextRT = btnTextComp.rectTransform;
        btnTextRT.anchorMin = Vector2.zero;
        btnTextRT.anchorMax = Vector2.one;
        btnTextRT.sizeDelta = Vector2.zero;

        var btn = playButton.AddComponent<Button>();
        btn.onClick.AddListener(() => GameManager.Instance.StartGame());

        UpdateHud();
        ShowGameplay();
    }

    public void UpdateHud()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.Score;
        }

        if (coinsText != null)
        {
            coinsText.text = "Coins: " + GameManager.Instance.Coins;
        }

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + GameManager.Instance.HighScore;
        }
    }

    public void ShowGameplay()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        UpdateHud();
    }
}
