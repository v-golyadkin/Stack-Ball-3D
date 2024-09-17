using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _cameraFollow;
    private Transform _ball, _finish;

    private void Awake()
    {
        _ball = FindObjectOfType<Ball>().transform;
    }

    private void Update()
    {
        if (_finish == null)
            _finish = GameObject.Find("Win(Clone)").GetComponent<Transform>();

        if (transform.position.y > _ball.transform.position.y && transform.position.y > _finish.transform.position.y + 4f)
            _cameraFollow = new Vector3(transform.position.x, _ball.transform.position.y, transform.position.y);

        transform.position = new Vector3(transform.position.x, _cameraFollow.y, -5);
    }
}
