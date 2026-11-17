using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Running,
        Paused,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public float BaseRunSpeed = 12f;
    public float CurrentRunSpeed { get; private set; }
    public int Score { get; private set; }
    public int Coins { get; private set; }
    public int HighScore { get; private set; }

    private GameObject player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Initialize()
    {
        HighScore = PlayerPrefs.GetInt("MetroRushHighScore", 0);
        CurrentRunSpeed = BaseRunSpeed;
        CurrentState = GameState.Menu;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHud();
        }
    }

    private void Update()
    {
        if (CurrentState != GameState.Running)
        {
            return;
        }

        CurrentRunSpeed = Mathf.Lerp(CurrentRunSpeed, BaseRunSpeed + (Score * 0.08f), Time.deltaTime * 0.8f);
    }

    public void StartGame()
    {
        Score = 0;
        Coins = 0;
        CurrentRunSpeed = BaseRunSpeed;
        CurrentState = GameState.Running;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0f, 1.1f, 0f);
            var runner = player.GetComponent<RunnerController>();
            if (runner != null)
            {
                runner.ResetState();
            }
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameplay();
            UIManager.Instance.UpdateHud();
        }
    }

    public void EndGame()
    {
        if (CurrentState == GameState.GameOver)
        {
            return;
        }

        CurrentState = GameState.GameOver;
        HighScore = Mathf.Max(HighScore, Score);
        PlayerPrefs.SetInt("MetroRushHighScore", HighScore);
        PlayerPrefs.Save();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver();
        }
    }

    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Running)
        {
            return;
        }

        Score += amount;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHud();
        }
    }

    public void AddCoins(int amount)
    {
        if (CurrentState != GameState.Running)
        {
            return;
        }

        Coins += amount;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHud();
        }
    }

    public void Collect(CollectibleType type)
    {
        switch (type)
        {
            case CollectibleType.Coin:
                AddCoins(1);
                AddScore(10);
                break;
            case CollectibleType.Magnet:
                if (RunnerController.Instance != null)
                {
                    RunnerController.Instance.ApplyPowerUp(PowerUpType.Magnet);
                }
                break;
            case CollectibleType.Hoverboard:
                if (RunnerController.Instance != null)
                {
                    RunnerController.Instance.ApplyPowerUp(PowerUpType.Hoverboard);
                }
                break;
            case CollectibleType.Jetpack:
                if (RunnerController.Instance != null)
                {
                    RunnerController.Instance.ApplyPowerUp(PowerUpType.Jetpack);
                }
                break;
            case CollectibleType.Shield:
                if (RunnerController.Instance != null)
                {
                    RunnerController.Instance.ApplyPowerUp(PowerUpType.Shield);
                }
                break;
        }
    }
}
