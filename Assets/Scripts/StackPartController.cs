using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private StackController _stackController;
    private Collider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _stackController = transform.parent.GetComponent<StackController>();
        _collider = GetComponent<Collider>();
    }

    public void Shatter()
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = false;

        Vector3 forcePoint = transform.parent.position;
        float parentXPosition = transform.parent.position.x;
        float xPosition = _meshRenderer.transform.position.x;

        Vector3 subDirection = (parentXPosition - xPosition < 0) ? Vector3.right : Vector3.left;
        Vector3 direction = (Vector3.up * 1.5f + subDirection).normalized;

        float force = UnityEngine.Random.Range(20, 35);
        float torque = UnityEngine.Random.Range(110, 180);

        _rigidbody.AddForceAtPosition(direction * force, forcePoint, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.left * torque);
        _rigidbody.velocity = Vector3.down;
    }

    public void RemoveAllChilds()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
}
