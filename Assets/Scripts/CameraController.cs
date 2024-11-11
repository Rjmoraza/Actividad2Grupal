using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
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

    public void Init(Transform player, Transform target)
    {
        cam = Camera.main;
        cvc = GetComponent<CinemachineVirtualCamera>();
        cpf = cvc.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        cb = cam.GetComponent<CinemachineBrain>();

        cvc.Follow = player;
        cvc.LookAt = target;

        side = targetSide = maxSide;
        distance = targetDistance = maxDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
