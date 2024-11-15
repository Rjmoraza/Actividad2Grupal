using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    GameObject[] waypoints;
    UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    Animator anim;

    [SerializeField]
    SkinnedMeshRenderer[] renderers;

    private Coroutine currentState;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        agent = GetComponent<NavMeshAgent>();

        currentState = StartCoroutine(Roam());
        TurnInvisible();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("ZSpeed", agent.velocity.magnitude);
    }

    IEnumerator Roam()
    {
        while(true)
        {
            Vector3 targetPos = waypoints[Random.Range(0, waypoints.Length)].transform.position;
            agent.SetDestination(targetPos);
            yield return new WaitForSeconds(10);
        }
    }

    public void TurnVisible()
    {
        StartCoroutine(TurnVisibleCoroutine());
    }

    public void TurnInvisible()
    {
        StartCoroutine(TurnInvisibleCoroutine());
    }

    private IEnumerator TurnVisibleCoroutine()
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            foreach(SkinnedMeshRenderer mr in renderers)
            {
                mr.material.SetFloat("_Alpha", alpha);
            }
            yield return null;
        }
    }

    private IEnumerator TurnInvisibleCoroutine()
    {
        float alpha = 1;
        while(alpha > 0)
        {
            alpha -= Time.deltaTime;
            foreach (SkinnedMeshRenderer mr in renderers)
            {
                mr.material.SetFloat("_Alpha", alpha);
            }
            yield return null;
        }
    }
}
