using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    CameraController cam;
    Vector3 movementVector;
    Vector3 animVector;
    float yawValue;
    float pitchValue;
    float aim;
    float aimTarget;
    bool canWalk = true;
    float waterCooldown = 0;
    
    [SerializeField]
    Animator anim;

    [SerializeField]
    Rig rig;

    [SerializeField]
    GameObject targetPoint;

    [SerializeField]
    float movementSpeed = 3;

    [SerializeField]
    float maxAimHeight = 2;

    [SerializeField]
    float minAimHeight = -1;

    [SerializeField]
    float rotationSpeed = 60;

    [SerializeField]
    float maxWaterCooldown = 10;

    [SerializeField]
    GameObject shotgun;

    [SerializeField]
    GameObject shotgunFirepoint;

    [SerializeField]
    GameObject cameraPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotgun.SetActive(false);
        GameObject camContainer = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        cam = camContainer.GetComponentInChildren<CameraController>();
        cam.Init(transform, targetPoint.transform);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (waterCooldown > 0) waterCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(canWalk)
        {
            Vector3 heading = (transform.forward * movementVector.y + transform.right * movementVector.x);
            rb.velocity = heading * movementSpeed + Vector3.up * rb.velocity.y;
            Quaternion deltaRotation = Quaternion.Euler(0, yawValue * rotationSpeed * Time.fixedDeltaTime, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
            targetPoint.transform.localPosition = new Vector3(0, pitchValue, 5);

            if (aim < aimTarget - 0.05f) aim += Time.fixedDeltaTime * 3;
            else if (aim > aimTarget + 0.05f) aim -= Time.fixedDeltaTime * 3;
            if (aim < 0.5f && shotgun.activeSelf) shotgun.SetActive(false);
            if (aim > 0.5f && !shotgun.activeSelf) shotgun.SetActive(true);
            rig.weight = aim;

            Vector3 actualVelocity = new Vector3(movementVector.x, 0, movementVector.y).normalized;
            animVector = Vector3.Lerp(animVector, actualVelocity, Time.fixedDeltaTime * 5);
            anim.SetFloat("ZVelocity", animVector.z);
            anim.SetFloat("XVelocity", animVector.x);
        }
        else
        {
            if (aim > 0) aim -= Time.fixedDeltaTime * 5;
            rig.weight = aim;
            anim.SetFloat("XVelocity", 0);
            anim.SetFloat("ZVelocity", 0);
        }
    }

    public void OnMove(InputAction.CallbackContext c)
    {
        movementVector = c.ReadValue<Vector2>();
        //movementVector = (transform.forward * moveValue.y + transform.right * moveValue.x);
        if (movementVector.magnitude > 1) movementVector = movementVector.normalized;
    }

    /// <summary>
    /// Look with the mouse
    /// </summary>
    /// <param name="c"></param>
    public void OnLookAdditive(InputAction.CallbackContext c)
    {
        Vector2 lookValue = c.ReadValue<Vector2>();
        yawValue = lookValue.x * 0.2f; //Mouse delta is too sensitive
        pitchValue = Mathf.Clamp(pitchValue + lookValue.y * Time.fixedDeltaTime, minAimHeight, maxAimHeight);
    }

    public void OnLook(InputAction.CallbackContext c)
    {
        Vector2 lookValue = c.ReadValue<Vector2>();
        yawValue = lookValue.x;

        if (lookValue.y > 0) pitchValue = lookValue.y * maxAimHeight;
        else if (lookValue.y < 0) pitchValue = lookValue.y * -minAimHeight;        
    }

    public void OnFire1(InputAction.CallbackContext c)
    {

    }

    public void OnFire2(InputAction.CallbackContext c)
    {
        if(waterCooldown <= 0)
        {
            waterCooldown = maxWaterCooldown;
            StartCoroutine(ThrowWater());
        }
    }

    public void OnAim(InputAction.CallbackContext c)
    {
        if (c.performed)
        {
            aimTarget = 1;
            cam.EngageAim();
        }
        else if (c.canceled)
        {
            aimTarget = 0;
            cam.DisengageAim();
        }
    }

    IEnumerator ThrowWater()
    {
        canWalk = false;
        anim.SetTrigger("Throw");
        yield return new WaitForSeconds(2.1f);
        canWalk = true;
    }
}
