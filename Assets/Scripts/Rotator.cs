using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 100f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, _speed * Time.deltaTime, 0));
    }
}
