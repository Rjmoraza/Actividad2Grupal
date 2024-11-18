using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    CameraBrain camBrain;
    PostprocessingManager postprocessing;
    Vector3 movementVector;
    Vector3 animVector;
    float yawValue;
    float pitchValue;
    float aim;
    float aimTarget;
    bool canWalk = true;
    bool canAim = true;
    public bool isPaused = false;
    float waterCooldown = 0;
    float hitpoints;

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
    float maxHP = 100;

    [SerializeField]
    GameObject shotgun;

    [SerializeField]
    GameObject shotgunFirepoint;

    [SerializeField]
    GameObject explosionVFX;

    [SerializeField]
    GameObject cameraPrefab;

    [SerializeField]
    PauseMenu pauseMenuManager;



    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        shotgun.SetActive(false);
        GameObject camContainer = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
        camBrain = camContainer.GetComponent<CameraBrain>();
        camBrain.InitCamera(transform, targetPoint.transform);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        hitpoints = maxHP;
        postprocessing = GameObject.Find("PostProcessing")?.GetComponent<PostprocessingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waterCooldown > 0) waterCooldown -= Time.deltaTime;

        if (canWalk)
        {
            Vector3 heading = (transform.forward * movementVector.y + transform.right * movementVector.x);
            rb.velocity = heading * movementSpeed + Vector3.up * rb.velocity.y;
            transform.Rotate(0, yawValue * rotationSpeed * Time.deltaTime, 0);
            targetPoint.transform.localPosition = new Vector3(0, pitchValue, 5);

            Vector3 actualVelocity = new Vector3(movementVector.x, 0, movementVector.y).normalized;
            animVector = Vector3.Lerp(animVector, actualVelocity, Time.deltaTime * 5);
            anim.SetFloat("ZVelocity", animVector.z);
            anim.SetFloat("XVelocity", animVector.x);
        }
        else
        {
            anim.SetFloat("XVelocity", 0);
            anim.SetFloat("ZVelocity", 0);
        }

        if(canAim)
        {
            if (aim < aimTarget - 0.05f) aim += Time.deltaTime * 3;
            else if (aim > aimTarget + 0.05f) aim -= Time.deltaTime * 3;
            if (aim < 0.5f && shotgun.activeSelf) shotgun.SetActive(false);
            if (aim > 0.5f && !shotgun.activeSelf) shotgun.SetActive(true);
            rig.weight = aim;
        }
        else
        {
            if (aim > 0) aim -= Time.deltaTime * 5;
            rig.weight = aim;
        }
        camBrain.UpdateCamera();

        // Automatically recover one HP per second
        if(hitpoints > 0 && hitpoints < maxHP)
        {
            hitpoints += Time.deltaTime;
        }
        if(postprocessing) postprocessing.UpdateHP(hitpoints, maxHP);
    }

    public void OnMove(InputAction.CallbackContext c)
    {
        if (isPaused) return;
        movementVector = c.ReadValue<Vector2>();
        //movementVector = (transform.forward * moveValue.y + transform.right * moveValue.x);
        if (movementVector.magnitude > 1) movementVector = movementVector.normalized;
    }

    /// <summary>
    /// Look with the mouse
    /// The vertical axis of the mouse is added to the current look angle
    /// </summary>
    /// <param name="c">Information of the Input received in Vector2 format</param>
    public void OnLookAdditive(InputAction.CallbackContext c)
    {
        if (isPaused) return;
        Vector2 lookValue = c.ReadValue<Vector2>();
        yawValue = lookValue.x * 0.2f; //Mouse delta is too sensitive
        pitchValue = Mathf.Clamp(pitchValue + lookValue.y * Time.fixedDeltaTime, minAimHeight, maxAimHeight);
    }

    /// <summary>
    /// Look with Gamepad
    /// The vertical axis is taken as is (non-additive)
    /// </summary>
    /// <param name="c">Information from Input received in Vector2 format</param>
    public void OnLook(InputAction.CallbackContext c)
    {
        if (isPaused) return;
        Vector2 lookValue = c.ReadValue<Vector2>();
        yawValue = lookValue.x;

        if (lookValue.y > 0) pitchValue = lookValue.y * maxAimHeight;
        else if (lookValue.y < 0) pitchValue = lookValue.y * -minAimHeight;
    }

    public void OnFire1(InputAction.CallbackContext c)
    {
        if (isPaused) return;
        StartCoroutine(FireShotgun());
    }

    public void OnFire2(InputAction.CallbackContext c)
    {
        if (isPaused) return;
        if (waterCooldown <= 0)
        {
            waterCooldown = maxWaterCooldown;
            StartCoroutine(ThrowWater());
        }
    }

    public void OnAim(InputAction.CallbackContext c)
    {
        if (isPaused) return;
        if (c.performed)
        {
            aimTarget = 1;
            camBrain.EngageAim();
        }
        else if (c.canceled)
        {
            aimTarget = 0;
            camBrain.DisengageAim();
        }
    }

    public void OnPause(InputAction.CallbackContext c)
    {

        if (c.performed)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                pauseMenuManager.PauseGame();
            }
            else
            {
                pauseMenuManager.ResumeGame();
            }
            Debug.Log("Pause" + isPaused);
        }
    }

    public void Damage(float dmg)
    {
        if(hitpoints > 0)
        {
            if (postprocessing) postprocessing.ShowHit();
            hitpoints -= dmg;
            if (hitpoints > 0)
            {
                StartCoroutine(DamageRoutine());
            }
            else
            {
                canAim = false;
                canWalk = false;
                anim.SetTrigger("Die");
            }
        }
    }

    IEnumerator DamageRoutine()
    {
        canWalk = false;
        canAim = false;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(1.3f);
        canWalk = true;
        canAim = true;
    }

    IEnumerator ThrowWater()
    {
        canWalk = false;
        canAim = false;
        anim.SetTrigger("Throw");

        yield return new WaitForSeconds(1);

        RaycastHit hit;
        for(float angle = -1; angle <= 1; angle += 0.1f)
        {
            if (Physics.Raycast(transform.position, transform.forward + transform.right * angle, out hit, 10))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<EnemyController>().Reveal();
                }
            }
        }
        
        yield return new WaitForSeconds(1.6f);
        canWalk = true;
        canAim = true;
    }

    IEnumerator FireShotgun()
    {
        if(aim >= 0.9f && canWalk)
        {
            canWalk = false;
            anim.SetTrigger("Shoot");
            yield return new WaitForSeconds(0.1f);
            GameObject explosion = Instantiate(explosionVFX, shotgunFirepoint.transform.position, Quaternion.identity);

            RaycastHit hit;
            for (float angle = -0.5f; angle <= 0.5f; angle += 0.1f)
            {
                if (Physics.Raycast(transform.position, transform.forward + transform.right * angle, out hit, 10))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        hit.collider.GetComponent<EnemyController>().Damage(10);
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
            canWalk = true;
            yield return new WaitForSeconds(5);
            Destroy(explosion);
        }
        yield return null;
    }

    public bool IsAlive()
    {
        return hitpoints > 0;
    }
}
