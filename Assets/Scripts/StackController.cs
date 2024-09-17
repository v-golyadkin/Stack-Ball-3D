using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private StackPartController[] _stackPartControllers = null;

    public void ShatterAllParts()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
            FindObjectOfType<Ball>().IncreaseBrokenStacks();
        }

        foreach(StackPartController controller in _stackPartControllers)
        {
            controller.Shatter();
        }

        StartCoroutine(RemoveParts());
    }

    IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
