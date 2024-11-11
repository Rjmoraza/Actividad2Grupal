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

    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(Roam());
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
}
