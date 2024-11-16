using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class PostprocessingManager : MonoBehaviour
{
    Vignette vignette;
    LensDistortion distorsion;
    float hitpoints;
    float maxHP;
    float highIntensity = 0.35f;
    float lowIntensity = 0.45f;
    Color highColor = Color.black;
    Color lowColor = Color.red;


    // Start is called before the first frame update
    void Start()
    {
        Volume volume = GetComponent<Volume>();
        VolumeProfile profile = volume.profile;
        profile.TryGet<Vignette>(out vignette);
        profile.TryGet<LensDistortion>(out distorsion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHit()
    {
        StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect()
    {
        float intensity = 1f;
        while(intensity > 0)
        {
            intensity -= Time.deltaTime;
            distorsion.intensity.Override(intensity);
            yield return null;
        }
    }

    public void UpdateHP(float hp, float maxHP)
    {
        this.hitpoints = hp;
        this.maxHP = maxHP;

        float t = hitpoints / maxHP;
        vignette.color.Override(Color.Lerp(lowColor, highColor, t));
        vignette.intensity.Override(Mathf.Lerp(lowIntensity, highIntensity, t));
    }
}
