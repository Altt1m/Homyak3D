using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RaySystem : MonoBehaviour
{
    public Transform raypoint;
    public float usingdistantion = 2f;
    RaycastHit hit;

    public Text info;
    public Image crosshair;
    public Image clickImage;

    public static bool hasFlashlight = false;
    public int items = 0;
    public Text itemsText;
    private AudioSource source;
    public AudioClip pickupClip;

    private void Start()
    {
        itemsText.text = "0";
        source = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        if (Physics.Raycast(raypoint.position, raypoint.forward, out hit, usingdistantion))
        {
            if (hit.collider.tag == "Untagged")
            {
                info.text = null;
                clickImage.enabled = false;
                crosshair.enabled = true;
            }

            if (hit.collider.tag == "door")
            {
                info.text = "Examine";
                clickImage.enabled = true;
                crosshair.enabled = false;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Door door = hit.collider.GetComponent<Door>();
                    door.Using();
                }
            }

            if (hit.collider.tag == "flashlight")
            {
                info.text = "Pick up";
                clickImage.enabled = true;
                crosshair.enabled = false;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Destroy(hit.collider.gameObject);
                    source.Stop();
                    source.PlayOneShot(pickupClip);
                    hasFlashlight = true;
                }
            }

            if (hit.collider.tag == "seed")
            {
                info.text = "Pick up";
                clickImage.enabled = true;
                crosshair.enabled = false;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    items++;
                    itemsText.text = items + "";
                    Destroy(hit.collider.gameObject);
                    source.Stop();
                    source.PlayOneShot(pickupClip);
                }
            }
        }
        else
        {
            info.text = null;
            clickImage.enabled = false;
            crosshair.enabled = true;
        }
    }

}
