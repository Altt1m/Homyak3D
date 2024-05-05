using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class NotePickup : MonoBehaviour
{
    public Transform raypoint;
    RaycastHit hit;
    public Text info;
    public Image crosshair;

    public Image noteImage;
    public Text noteText;
    public Image clickImage;

    public string tag;

    private AudioSource source;
    public AudioClip notePickupClip;

    private GameObject noteToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        noteImage.enabled = false;
        noteText.enabled = false;
        clickImage.enabled = false;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(raypoint.position, raypoint.forward, out hit, 1.5f))
        {
            if (hit.collider.tag == "Untagged")
            {
                info.text = null;
                clickImage.enabled = false;
                crosshair.enabled = true;
            }

            if (hit.collider.tag == tag)
            {
                noteToDestroy = hit.collider.gameObject;
                if (!noteImage.enabled)
                {
                    info.text = "Note";
                    clickImage.enabled = true;
                    crosshair.enabled = false;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        NoteUp();
                    }
                }
            }

        }
        else
        {
            if (!noteImage.enabled)
            {
                info.text = null;
                clickImage.enabled = false;
                crosshair.enabled = true;
            }
        }

        if (noteImage.enabled && noteText.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                NoteDown();

                Destroy(noteToDestroy);
            }
        }
    }

    void NoteUp()
    {
        PlayerMovement.isBusy = true;
        noteImage.enabled = true;
        noteText.enabled = true;
        clickImage.enabled = false;
        info.text = null;
        source.PlayOneShot(notePickupClip);
    }

    void NoteDown()
    {
        PlayerMovement.isBusy = false;
        noteImage.enabled = false;
        noteText.enabled = false;
        crosshair.enabled = true;
    }
}
