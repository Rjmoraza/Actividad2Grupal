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

    [SerializeField]
    GameObject waterVFX;

    [SerializeField]
    GameObject waterSocket;

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

    public void ThrowWater()
    {
        StartCoroutine(SpawnWater());
    }

    private IEnumerator SpawnWater()
    {
        GameObject water = Instantiate(waterVFX, waterSocket.transform.position, waterSocket.transform.rotation);
        yield return new WaitForSeconds(5);
        Destroy(water);
    }
}
