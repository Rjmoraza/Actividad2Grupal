using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private enum State { 
        Roaming,
        Fleeing,
        Attacking,
        Dying
    }

    GameObject[] waypoints;
    NavMeshAgent agent;
    AudioSource source;
    bool visible = false;
    State currentState = State.Roaming;
    float hitpoints;

    [SerializeField]
    float maxHP = 100;

    [SerializeField]
    Animator anim;

    [SerializeField]
    SkinnedMeshRenderer[] renderers;

    [SerializeField]
    AudioClip scream1;

    [SerializeField]
    AudioClip scream2;

    private Coroutine currentRoutine;

    // Start is called before the first frame update
    void Start()
    {
        //waypoints = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        agent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        hitpoints = maxHP;

        currentRoutine = StartCoroutine(Roam());
        TurnInvisible();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("ZSpeed", agent.velocity.magnitude);
        print(currentState);
    }

    void SwitchState(State newState)
    {
        if(currentState != newState)
        {
            StopCoroutine(currentRoutine);
            switch (newState)
            {
                case State.Roaming:
                    currentState = newState;
                    currentRoutine = StartCoroutine(Roam());
                    break;
                case State.Fleeing:
                    currentState = newState;
                    currentRoutine = StartCoroutine(Flee());
                    break;
                case State.Attacking:
                    currentState = newState;
                    currentRoutine = StartCoroutine(Attack());
                    break;
                case State.Dying:
                    currentState = newState;
                    anim.SetTrigger("Die");
                    agent.speed = 0;
                    source.Stop();
                    TurnInvisible();
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }

    IEnumerator Roam()
    {
        while(true)
        {
            Vector3 targetPos = waypoints[Random.Range(0, waypoints.Length)].transform.position;
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPos, path);
            agent.SetPath(path);
            //agent.SetDestination(targetPos);
            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator Flee()
    {
        agent.speed = 3.5f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while(true)
        {
            if(player != null)
            {
                Vector3 direction = (transform.position - player.transform.position).normalized;
                NavMeshPath path = new NavMeshPath();
                print(agent.CalculatePath(transform.position + direction * 10, path));
                agent.SetPath(path);
                //agent.SetDestination(transform.position + direction * 10);
                print("I'm Fleeing");
            }
            yield return null;
        }
    }

    IEnumerator Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while(true)
        {
            yield return null;
            if(player != null)
            {
                Vector3 playerPos = player.transform.position;
                float distance = Vector3.Distance(transform.position, playerPos);
                if (distance > 2)
                {
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(playerPos, path);
                    agent.SetPath(path);
                    //agent.SetDestination(playerPos);
                    agent.speed = 3.5f;
                }
                else
                {
                    agent.speed = 0;
                    anim.SetTrigger("Attack");
                    yield return new WaitForSeconds(3f);
                }
            }
        }
    }

    IEnumerator RevealCoroutine()
    {
        visible = true;
        TurnVisible();
        anim.SetTrigger("Reveal");
        source.PlayOneShot(scream1);
        float baseSpeed = agent.speed;
        agent.speed = 0;
        yield return new WaitForSeconds(2.5f);
        agent.speed = baseSpeed;

        if(currentState == State.Roaming)
        {
            SwitchState(State.Fleeing);
        }
    }

    public void Damage(float dmg)
    {
        if(currentState != State.Dying)
        {
            source.PlayOneShot(scream2);
            hitpoints -= dmg;
            if (hitpoints <= 0)
            {
                SwitchState(State.Dying);
            }
            else if (hitpoints <= (maxHP * 0.7f))
            {
                SwitchState(State.Attacking);
            }
        }
    }

    public void Reveal()
    {
        if(!visible)
        {
            StartCoroutine(RevealCoroutine());
        }
    }

    public void TurnVisible()
    {
        visible = true;
        StartCoroutine(TurnVisibleCoroutine());
    }

    public void TurnInvisible()
    {
        visible = false;
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

    public bool IsVisible()
    {
        return visible;
    }

    public bool IsAlive()
    {
        return hitpoints > 0;
    }

    public void SetWaypoints(GameObject[] waypoints)
    {
        this.waypoints = waypoints;
    }
}
