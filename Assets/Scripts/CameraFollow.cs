using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float _horizontalSpeed = 5.0f;
    [SerializeField] private Transform _target;

    private readonly string STR_MOUSE_X = "Mouse X";
    
    private void Update()
    {
        transform.position = _target.transform.position;
        RotateCameraTheMouse();
    }

    private void RotateCameraTheMouse()
    {
        float h = _horizontalSpeed * Input.GetAxis(STR_MOUSE_X);
        
        transform.Rotate(0, h, 0);
    }
}
