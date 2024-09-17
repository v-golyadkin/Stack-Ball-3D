using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private Text _scoreText;
    [HideInInspector] public int score = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        AddScore(0);
    }

    private void Update()
    {
        if(_scoreText == null)
        {
            _scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            _scoreText.text = score.ToString();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", score);

        _scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
