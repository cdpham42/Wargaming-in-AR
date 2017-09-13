using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCameraMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeed;
    private float x;
    private float y;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        rotationSpeed = 3.0f;
        movementSpeed = 5.0f;
        x = 0.0f;
        y = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        x += Input.GetAxis("Mouse X") * rotationSpeed;
        y += Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.localRotation = Quaternion.AngleAxis(x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(y, Vector3.left);
        transform.position += transform.forward * movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += transform.right * movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
    }
}
