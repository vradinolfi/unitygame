using UnityEngine;

public class DragToRotate : MonoBehaviour
{
    const float Sensitivity = .4f;

    Vector3 _recentMousePosition;
    Vector3 _mouseOffset;
    Vector3 _rotation = Vector3.zero;
    bool _isRotating;

    void Update()
    {
        if (!_isRotating) return;

        _mouseOffset = Input.mousePosition - _recentMousePosition;

        _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * Sensitivity;

        transform.Rotate(_rotation);

        _recentMousePosition = Input.mousePosition;
    }

    void OnMouseDown()
    {
        _isRotating = true;
        _recentMousePosition = Input.mousePosition;
    }

    void OnMouseUp()
    {
        _isRotating = false;
    }

}