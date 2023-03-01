using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    private void Update()
    {
        if (transform.eulerAngles.y != 0) transform.eulerAngles = Vector3.zero;
    }
}
