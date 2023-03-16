using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] GameObject crosshair;
    [SerializeField] float aimSensetivity = 1;
    [SerializeField] Vector2 crossHairBounds = new Vector2(8.5f, 4.5f);
    Vector2 aimInput;

    public void GetAimInput(InputAction.CallbackContext ctx) => aimInput = ctx.ReadValue<Vector2>();

    private void Start()
    {
        crosshair.transform.parent = Camera.main.transform;
    }

    private void Update()
    {
        var pos = new Vector3(aimInput.x, aimInput.y, 0) * aimSensetivity;
        pos += crosshair.transform.localPosition;
        pos.x = Mathf.Clamp(pos.x, -crossHairBounds.x, crossHairBounds.x);
        pos.y = Mathf.Clamp(pos.y, -crossHairBounds.y, crossHairBounds.y);
        crosshair.transform.localPosition = pos;
    }
}
