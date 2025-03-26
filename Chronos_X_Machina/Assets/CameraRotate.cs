using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float returnSpeed = 5f;
    public float minRotation = 90f;
    public float maxRotation = 270f;
    private Quaternion originalRotation;
    private bool isDragging = false;
    private float currentRotation = 180f;
    private Vector3 lastMousePosition;

    void Start()
    {
        originalRotation = Quaternion.Euler(0, currentRotation, 0);
        transform.rotation = originalRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start dragging
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // Stop dragging
        {
            isDragging = false;
            StartCoroutine(RotateBack());
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float newRotation = currentRotation + (mouseDelta.x * rotationSpeed * Time.deltaTime);
            currentRotation = Mathf.Clamp(newRotation, minRotation, maxRotation);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
            lastMousePosition = Input.mousePosition;
        }
    }

    System.Collections.IEnumerator RotateBack()
    {
        while (Mathf.Abs(currentRotation - 180f) > 0.1f)
        {
            currentRotation = Mathf.Lerp(currentRotation, 180f, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
            yield return null;
        }
        currentRotation = 180f;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }
}
