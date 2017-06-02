using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Text highScoreLevel1Text;
    public Text highScoreLevel2Text;

    void Awake()
    {
        highScoreLevel1Text.text = "HighScore: " + PlayerPrefs.GetInt("HighScoreLevel1").ToString();
        highScoreLevel2Text.text = "HighScore: " + PlayerPrefs.GetInt("HighScoreLevel2").ToString();
    }

    private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }
    public void OnLevelButtonPressed()
    {
        StartCoroutine(StartGame("Poziom1"));
    }

    public void OnLevel2ButtonPressed()
    {
        StartCoroutine("Poziom2");
    }
}
