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

    public static bool hasFlashlight;
    public static int items;
    private int itemsCollected;
    public Text itemsText;
    private AudioSource source;
    public AudioClip pickupClip;

    private void Start()
    {
        itemsText.text = "0/5";
        source = GetComponent<AudioSource>();
        items = 0;
        itemsCollected = 0;
        hasFlashlight = false;
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
                    items++; itemsCollected++;
                    itemsText.text = itemsCollected + "/5";
                    Destroy(hit.collider.gameObject);
                    source.Stop();
                    source.PlayOneShot(pickupClip);
                }
                if (itemsCollected == 5)
                {
                    EnemyController.seedsFound = true;
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
