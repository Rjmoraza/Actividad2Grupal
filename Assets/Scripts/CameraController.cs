using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    CinemachineVirtualCamera cvc;
    Cinemachine3rdPersonFollow cpf;
    CinemachineBrain cb;
    Camera cam;

    float side, targetSide;
    float distance, targetDistance;
    float epsilon = 0.02f;

    [SerializeField]
    float minSide = 0.7f;

    [SerializeField]
    float maxSide = 1;

    [SerializeField]
    float minDistance = 0.5f;

    [SerializeField]
    float maxDistance = 1.10f;

    [SerializeField]
    float adjustmentMultiplier = 2f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;

            cam = Camera.main;
            cvc = GetComponent<CinemachineVirtualCamera>();
            cpf = cvc.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            cb = cam.GetComponent<CinemachineBrain>();

            side = targetSide = maxSide;
            distance = targetDistance = maxDistance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (distance < targetDistance - epsilon) distance += Time.deltaTime * adjustmentMultiplier;
        else if (distance > targetDistance + epsilon) distance -= Time.deltaTime * adjustmentMultiplier;

        if (side < targetSide - epsilon) side += Time.deltaTime * adjustmentMultiplier;
        else if (side > targetSide + epsilon) side -= Time.deltaTime * adjustmentMultiplier;

        cpf.CameraSide = side;
        cpf.CameraDistance = distance;
    }

    public void EngageAim()
    {
        targetDistance = minDistance;
        targetSide = minSide;
    }

    public void DisengageAim()
    {
        targetDistance = maxDistance;
        targetSide = maxSide;
    }
}
