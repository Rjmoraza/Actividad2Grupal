using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimatorEvents : MonoBehaviour
{
    AudioSource source;

    [SerializeField]
    AudioClip[] creaksS;

    [SerializeField]
    AudioClip[] steps;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Step()
    {
        AudioClip step = steps[Random.Range(0, steps.Length)];
        AudioClip creak = creaksS[Random.Range(0, creaksS.Length)];

        source.PlayOneShot(step);
        source.PlayOneShot(creak);
    }
}
