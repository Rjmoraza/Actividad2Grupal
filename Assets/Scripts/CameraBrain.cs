using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBrain : MonoBehaviour
{
    Camera cam;
    Vector3 offset;

    [SerializeField]
    Vector3 farOffset;

    [SerializeField]
    Vector3 closeOffset;

    [SerializeField]
    Vector3 damping;

    [SerializeField]
    float cameraRadius = 0.5f;

    [SerializeField]
    Transform followTarget;

    [SerializeField]
    Transform aimTarget;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = followTarget.transform.position + offset;
        transform.LookAt(aimTarget.position);
        offset = farOffset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitCamera(Transform followTarget, Transform aimTarget)
    {
        this.followTarget = followTarget;
        this.aimTarget = aimTarget;
        offset = farOffset;
    }

    public void UpdateCamera()
    {
        Vector3 localOffset = followTarget.rotation * offset;
        Vector3 targetPos = followTarget.position + localOffset;
        Vector3 deltaPos = targetPos - transform.position;
        Vector3 dampedDelta = deltaPos + damping * Time.deltaTime;
        Vector3 realTarget = transform.position + dampedDelta;

        transform.position = realTarget;

        transform.LookAt(aimTarget);
    }

    Vector3 CalculateTargetPos(Vector3 wantedTarget, Vector3 followTarget)
    {
        Vector3 delta = wantedTarget - followTarget;
        Vector3 direction = delta.normalized;
        float distance = delta.magnitude;
        RaycastHit hit;
        if (Physics.Raycast(followTarget, direction, out hit, distance))
        {
            return hit.point - (direction * cameraRadius);
        }
        return wantedTarget;
    }

    public void EngageAim()
    {
        StartCoroutine(InterpolateOffset(offset, closeOffset));
    }

    public void DisengageAim()
    {
        StartCoroutine(InterpolateOffset(offset, farOffset));
    }

    IEnumerator InterpolateOffset(Vector3 sourceOffset, Vector3 targetOffset)
    {
        float t = 0;
        while (t < 1)
        {
            offset = Vector3.Lerp(sourceOffset, targetOffset, t);
            t += Time.deltaTime * 2;
            yield return null;
        }
        offset = targetOffset;
    }
}
