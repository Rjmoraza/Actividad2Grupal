using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * 2, 4, LayerMask.GetMask("Player"));
        foreach(Collider hit in hits)
        {
            if(hit.tag == "Player")
            {
                hit.GetComponent<PlayerController>().Damage(40);
            }
        }
    }
}
