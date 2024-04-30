using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSounds : MonoBehaviour
{
    //звук шагов при движении игрока

    private AudioSource source;
    public AudioClip[] clips;
    private Material material;

    private bool ismoving = false;
    private bool isrunning = false;
    private int currentmaterial = 0;
    public float timebetweensounds = 0.8f;


    private void Start()
    {
        InvokeRepeating("MoveSound", 0, timebetweensounds);
        InvokeRepeating("RunSound", 0, timebetweensounds /2f);
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
            {
                ismoving = false;
                isrunning = true;
            }
            else
            {
                //идет
                ismoving = true;
                isrunning = false;
            }

        }
        else
        {
            //стоит
            ismoving = false;
            isrunning = false;
        }

    }

    private void MoveSound()
    {
        if (ismoving && !PlayerMovement.isBusy)
        {
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }

    private void RunSound()
    {
        if (isrunning && !PlayerMovement.isBusy)
        {
            source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }

}
