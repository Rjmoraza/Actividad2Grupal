using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 movementVector;
    float yawValue;
    float pitchValue;
    bool isAiming = false;
    

    [SerializeField]
    Animator anim;

    [SerializeField]
    GameObject targetPoint;

    [SerializeField]
    float movementSpeed = 3;

    [SerializeField]
    float rotationSpeed = 60;

    [SerializeField]
    float maxCameraAngle = 45;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        print(isAiming);
    }

    private void FixedUpdate()
    {
        Vector3 heading = (transform.forward * movementVector.y + transform.right * movementVector.x);
        rb.velocity = heading * movementSpeed + Vector3.up * rb.velocity.y;
        Quaternion deltaRotation = Quaternion.Euler(0, yawValue * rotationSpeed * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
        targetPoint.transform.localPosition = new Vector3(0, pitchValue, 5);
    }

    public void OnMove(InputAction.CallbackContext c)
    {
        movementVector = c.ReadValue<Vector2>();
        //movementVector = (transform.forward * moveValue.y + transform.right * moveValue.x);
        if (movementVector.magnitude > 1) movementVector = movementVector.normalized;
    }

    public void OnLookAdditive(InputAction.CallbackContext c)
    {
        Vector2 lookValue = c.ReadValue<Vector2>();
        yawValue = lookValue.x;
        pitchValue = Mathf.Clamp(pitchValue + lookValue.y, -1, 1);
    }

    public void OnLook(InputAction.CallbackContext c)
    {
        Vector2 lookValue = c.ReadValue<Vector2>();
        yawValue = lookValue.x;
        pitchValue = lookValue.y;       
    }

    public void OnFire1(InputAction.CallbackContext c)
    {

    }

    public void OnFire2(InputAction.CallbackContext c)
    {

    }

    public void OnAim(InputAction.CallbackContext c)
    {
        if (c.performed)
        {
            isAiming = true;
            CameraController.instance.EngageAim();
        }
        else if (c.canceled)
        {
            isAiming = false;
            CameraController.instance.DisengageAim();
        }
    }
}
