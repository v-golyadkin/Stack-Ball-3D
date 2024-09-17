using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _model;
    
    private GameObject[] _modelPrefab = new GameObject[4];

    [SerializeField] private GameObject _winPrefab;

    private GameObject _temp1, _temp2;

    public int level = 1, addOn = 7;
    float i = 0;

    [SerializeField] private Material _plateMaterial, _baseMaterial;
    [SerializeField] private MeshRenderer _ballMesh;

    private void Awake()
    {
        GenerateLevelColor();

        level = PlayerPrefs.GetInt("Level", 1);

        if (level > 9)
            addOn = 0;

        float random = UnityEngine.Random.value;
        ModelSelection();

        for (i = 0; i > -level - addOn; i -= 0.5f)
        {
            if (level <= 20)
                _temp1 = Instantiate(_modelPrefab[UnityEngine.Random.Range(0, 2)]);
            if (level > 20 && level <= 50)
                _temp1 = Instantiate(_modelPrefab[UnityEngine.Random.Range(1, 3)]);
            if (level > 50 && level <= 100)
                _temp1 = Instantiate(_modelPrefab[UnityEngine.Random.Range(2, 4)]);
            if (level > 100)
                _temp1 = Instantiate(_modelPrefab[UnityEngine.Random.Range(3, 4)]);

            _temp1.transform.position = new Vector3(0, i - 0.01f, 0);
            _temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

            if (Mathf.Abs(i) >= level * 0.3f && Mathf.Abs(i) <= level * 0.6f)
            {
                _temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                _temp1.transform.eulerAngles += Vector3.up * 180;
            }
            else if (Mathf.Abs(i) >= level * 0.8f)
            {
                _temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

                if (random > 0.75f)
                    _temp1.transform.eulerAngles += Vector3.up * 180;
            }

            _temp1.transform.parent = FindObjectOfType<Rotator>().transform;
        }

        _temp2 = Instantiate(_winPrefab);
        _temp2.transform.position = new Vector3(0, i - 0.01f, 0);
    }

    private void GenerateLevelColor()
    {
        _plateMaterial.color = UnityEngine.Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
        _baseMaterial.color = _plateMaterial.color + Color.gray;
        _ballMesh.material.color = _plateMaterial.color;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
           GenerateLevelColor();
        }        
    }

    private void ModelSelection()
    {
        int randomNomedl = UnityEngine.Random.Range(0, 5);

        switch(randomNomedl)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                    _modelPrefab[i] = _model[i];
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                    _modelPrefab[i] = _model[i + 4];
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                    _modelPrefab[i] = _model[i + 8];
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                    _modelPrefab[i] = _model[i + 12];
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                    _modelPrefab[i] = _model[i + 16];
                break;
        }
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        SceneManager.LoadScene(0);
    }
}
