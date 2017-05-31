using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public enum GameState{
        GS_PAUSEMENU,
        GS_GAME,
        GS_LEVELCOMPLETED,
        GS_GAME_OVER,
    }
    public GameObject Player;

    public GameState currentGameState = GameState.GS_PAUSEMENU;
    public static GameManager instance;
    public Canvas menuCanvas;
    public Canvas inGameCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas gameOverCanvas;

    private int maxKeyNum = 3;
    public bool keysCompleted = false;

    public Text coinsText;
    public Text enemiesText;
    public Text timerText;
    public Text enemiesCompletedText;
    public Text coinsCompletedText;
    public Text numberOfHeartsCompleted;
    private int coins = 0;
    private int keys = 0;
    private int hearts = 3;
    private int enemies = 0;
    private float timer;
    private float startTime;
    private bool finished = false;
    public Image[] keysTab;
    public Image[] heartTab;

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        inGameCanvas.enabled = (currentGameState == GameState.GS_GAME);
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
        gameOverCanvas.enabled = (currentGameState == GameState.GS_GAME_OVER);

        if (newGameState == GameState.GS_GAME)
        {
            inGameCanvas.enabled = true;
        }else if(newGameState == GameState.GS_PAUSEMENU)
        {
            inGameCanvas.enabled = false;
        }
        inGameCanvas.gameObject.SetActive((currentGameState == GameState.GS_GAME));
        pauseMenuCanvas.gameObject.SetActive((currentGameState == GameState.GS_PAUSEMENU));
    }
    public void InGame()
    {
        Player.SetActive(true);
        SetGameState(GameState.GS_GAME);
    }

    public void GameOver()
    {
        Player.SetActive(false);
        SetGameState(GameState.GS_GAME_OVER);
    }

    public void PauseMenu()
    {
        Player.SetActive(false);
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void LevelCompleted()
    {
        Player.SetActive(false);
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    private void Start()
    {
        startTime = Time.time;
    }
    void Awake()
    {
        instance = this;
        InGame();
        coinsText.text = coins.ToString();

        for (int i = 0; i < keysTab.Length; i++)
            keysTab[i].color = Color.grey;
        for (int i = 0; i < heartTab.Length; i++)
            heartTab[i].color = Color.red;
    }
	
	void Update () { 
        if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GS_PAUSEMENU)
        {
            InGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GS_GAME)
        {
            PauseMenu();
        }
        enemiesCompletedText.text = enemiesText.text;
        coinsCompletedText.text = coinsText.text;
        numberOfHeartsCompleted.text = hearts.ToString();
        if (hearts == 0)
            GameOver();

        if (finished)
            return;
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString("f2");
        string seconds = (t % 60).ToString();
        timer += Time.deltaTime;
        timerText.text = minutes + ":" + seconds;

        if (timer > LevelGenerator.instance.maxGameTime && !LevelGenerator.instance.shouldFinish)
            LevelGenerator.instance.Finish();
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene("Poziom2");
    }

    public void Finnish()
    {
        finished = true;
        timerText.color = Color.green;
    }

    public void AddKeys(int keyNumber)
    {
        keysTab[keys].color = Color.red;
        keys += 1;
        if (keys == maxKeyNum)
            keysCompleted = true;
    }
    public void AddCoins(int coinNumber)
    {
        coins += coinNumber;
        coinsText.text = coins.ToString();
    }
    public void AddEnemies(int enemiesNumber)
    {
        enemies += 1;
        enemiesText.text = enemies.ToString();
    }
    public void AddHearts(int heartNumber)
    {
        hearts += 1;
        if (hearts <= heartTab.Length)
        {
            heartTab[heartTab.Length - hearts].color = Color.cyan;
        }
    }
    public void DelHearts(int heartNumber)
    {
        heartTab[heartTab.Length - hearts].color = Color.clear;
        hearts -= 1;
    }
}
