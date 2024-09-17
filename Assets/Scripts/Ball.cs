using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _currentTime;

    private bool _smash, _invincible;

    private int _currentBrokenStacks, _totalStacks;

    [SerializeField] private GameObject _invincibleObject;
    [SerializeField] private Image _invincibleFill;
    [SerializeField] private GameObject _fireEffect, _winEffect, _splashEffect;

    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public BallState ballState = BallState.Prepare;

    [SerializeField] private AudioClip _bounceOffClip, _deadClip, _winClip, _destoryClip, _iDestroyClip;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _currentBrokenStacks = 0;
    }

    private void Start()
    {
        _totalStacks = FindObjectsOfType<StackController>().Length;
    }

    private void Update()
    {
        if(ballState == BallState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _smash = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _smash = false;
            }

            if (_invincible)
            {
                _currentTime -= Time.deltaTime * 0.35f;

                if(!_fireEffect.activeInHierarchy)
                    _fireEffect.SetActive(true);
            }
            else
            {
                if(_fireEffect.activeInHierarchy)
                    _fireEffect.SetActive(false);

                if (_smash)
                    _currentTime += Time.deltaTime * 0.8f;
                else
                    _currentTime -= Time.deltaTime * 0.5f;
            }

            if (_currentTime >= 0.3f || _invincibleFill.color == Color.red)
                _invincibleObject.SetActive(true);
            else
                _invincibleObject.SetActive(false);

            if (_currentTime >= 1)
            {
                _currentTime = 1;
                _invincible = true;
                _invincibleFill.color = Color.red;
            }
            else if (_currentTime <= 0)
            {
                _currentTime = 0;
                _invincible = false;
                _invincibleFill.color = Color.white;
            }

            if (_invincibleObject.activeInHierarchy)
                _invincibleFill.fillAmount = _currentTime / 1;
        }

        //if(ballState == BallState.Prepare)
        //{
        //    if(Input.GetMouseButtonDown(0))
        //        ballState = BallState.Playing;
        //}

        if(ballState == BallState.Finish) 
        {
            if (Input.GetMouseButtonDown(0))
                FindObjectOfType<LevelSpawner>().NextLevel();
        }
    }

    private void FixedUpdate()
    {
        if(ballState == BallState.Playing)
        {
            if (Input.GetMouseButton(0))
            {
                _smash = true;
                _rigidbody.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if (_rigidbody.velocity.y > 5)
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 5, _rigidbody.velocity.z);
    }

    public void IncreaseBrokenStacks()
    {
        _currentBrokenStacks++;

        if (!_invincible)
        {
            ScoreManager.instance.AddScore(1);
            SoundManager.instance.PlaySoundFX(_destoryClip, 0.5f);
        }
        else
        {
            ScoreManager.instance.AddScore(2);
            SoundManager.instance.PlaySoundFX(_iDestroyClip, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision target)
    {
        if(!_smash)
        {
            _rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);

            if(target.gameObject.tag != "Finish")
            {
                GameObject splash = Instantiate(_splashEffect);
                splash.transform.SetParent(target.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 0);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.22f, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            SoundManager.instance.PlaySoundFX(_bounceOffClip, 0.5f);
        }
        else
        {
            if (_invincible)
            {
                if (target.gameObject.tag == "enemy" || target.gameObject.tag == "plane")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if(target.gameObject.tag == "enemy")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if (target.gameObject.tag == "plane")
                {
                    _rigidbody.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ballState = BallState.Died;
                    SoundManager.instance.PlaySoundFX(_deadClip, 0.5f);
                }
            }         
        }

        FindObjectOfType<GameUI>().LevelSliderFill(_currentBrokenStacks / (float)_totalStacks);

        if(target.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
            SoundManager.instance.PlaySoundFX(_winClip, 0.7f);

            GameObject win = Instantiate(_winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localScale = Vector3.up * 1.5f;
            win.transform.eulerAngles = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision target)
    {
        if(!_smash || target.gameObject.tag == "Finish")
        {
            _rigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }
}
