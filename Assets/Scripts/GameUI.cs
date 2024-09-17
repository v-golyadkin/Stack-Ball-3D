using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _homeUI, _inGameUI, _finishUI, _gameOverUI;
    [SerializeField] private GameObject _allButtons;

    private bool _buttons;

    [Header("PreGame")]
    [SerializeField] private Button _soundButton;
    [SerializeField] private Sprite _soundOnSprite, _soundOffSprite;

    [Header("InGame")]
    [SerializeField] private Image _levelSlider;
    [SerializeField] private Image _currentLevelImage, _nextLevelImage;
    [SerializeField] private Text _currentLevelText, _nextLevelText;

    [Header("Finish")]
    [SerializeField] private Text _finishText;

    [Header("GameOver")]
    [SerializeField] private Text _gameOverScoreText;
    [SerializeField] private Text _gameOverBestScoreText;

    private Material _ballMaterial;
    private Ball _ball;

    private void Awake()
    {
        _ballMaterial = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        _ball = FindObjectOfType<Ball>();

        _levelSlider.transform.parent.GetComponent<Image>().color = _ballMaterial.color + Color.gray;
        _levelSlider.color = _ballMaterial.color;

        _currentLevelImage.transform.parent.GetComponent<Image>().color = _ballMaterial.color + Color.gray;
        _currentLevelImage.color = _ballMaterial.color;

        _nextLevelImage.transform.parent.GetComponent<Image>().color = _ballMaterial.color + Color.gray;
        _nextLevelImage.color = _ballMaterial.color;

        _soundButton.onClick.AddListener(() => SoundManager.instance.SoundOnOff());
    }

    private void Start()
    {
        _currentLevelText.text = FindObjectOfType<LevelSpawner>().level.ToString();
        _nextLevelText.text = FindObjectOfType<LevelSpawner>().level + 1 + "";
    }

    private void Update()
    {
        if (_ball.ballState == Ball.BallState.Prepare)
        {
            if(SoundManager.instance.sound && _soundButton.GetComponent<Image>().sprite != _soundOnSprite)
                _soundButton.GetComponent<Image>().sprite = _soundOnSprite;
            else if (!SoundManager.instance.sound && _soundButton.GetComponent<Image>().sprite != _soundOffSprite)
                _soundButton.GetComponent<Image>().sprite = _soundOffSprite;
        }

        if(Input.GetMouseButtonDown(0) && !IgnoreUI() &&_ball.ballState == Ball.BallState.Prepare)
        {
            _ball.ballState = Ball.BallState.Playing;
            _homeUI.SetActive(false);
            _inGameUI.SetActive(true);
            _finishUI.SetActive(false);
            _gameOverUI.SetActive(false);
        }

        if(_ball.ballState == Ball.BallState.Finish)
        {
            _homeUI.SetActive(false);
            _inGameUI.SetActive(false);
            _finishUI.SetActive(true);
            _gameOverUI.SetActive(false);

            _finishText.text = $"Level {FindObjectOfType<LevelSpawner>().level}";
        }

        if (_ball.ballState == Ball.BallState.Died)
        {
            _homeUI.SetActive(false);
            _inGameUI.SetActive(false);
            _finishUI.SetActive(false);
            _gameOverUI.SetActive(true);

            _gameOverScoreText.text = ScoreManager.instance.score.ToString();
            _gameOverBestScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();

            if (Input.GetMouseButtonDown(0))
            {
                ScoreManager.instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }
    }

    private bool IgnoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
        for(int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycastResultsList.RemoveAt(i);
                i--;
            }
        }

        return raycastResultsList.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        _levelSlider.fillAmount = fillAmount;
    }

    public void Settings()
    {
        _buttons = !_buttons;
        _allButtons.SetActive(_buttons);
    }
}
