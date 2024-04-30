using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ToggleLight : MonoBehaviour
{
    public Light spotlight;
    private AudioSource source;
    public AudioClip toggleOnClip;
    public AudioClip toggleOffClip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (RaySystem.hasFlashlight)
        {
            if (Input.GetKeyDown(KeyCode.F) && !spotlight.enabled) // on
            {
                spotlight.enabled = true;
                source.Stop();
                source.PlayOneShot(toggleOnClip);
            }
            else if (Input.GetKeyDown(KeyCode.F)) // off
            {
                spotlight.enabled = false;
                source.Stop();
                source.PlayOneShot(toggleOffClip);
            }
        }
        
    }
}
