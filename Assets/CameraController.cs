using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float friction = 0.025f;
    [SerializeField] int targetFrameRate = 60;
    [SerializeField] Vector2 limits, offset;

     [HideInInspector]public List<Transform> players = new List<Transform>();

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    private void LateUpdate()
    {
        if (players.Count == 0 || players[0] == null) return;

        Vector3 targetPosition = Vector3.zero;
        foreach (var p in players) targetPosition += p.position;
        targetPosition /= players.Count;

        targetPosition.z = transform.position.z;
        targetPosition += (Vector3) offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, limits.x, limits.y);
        transform.position = Vector3.Lerp(transform.position, targetPosition, friction);
    }
}
