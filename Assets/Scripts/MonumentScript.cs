using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonumentScript : MonoBehaviour
{
    public GameObject seed;
    public Transform raypoint;
    RaycastHit hit;
    public Text info;
    public Image crosshair;
    public Image clickImage;
    public AudioClip seedPutDown;
    private AudioSource source;
    public string MonName;
    private static int seedDown = 0;
    private static bool gameFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(raypoint.position, raypoint.forward, out hit, 2f))
        {
            if (hit.collider.tag == "Untagged")
            {
                info.text = null;
                clickImage.enabled = false;
                crosshair.enabled = true;
            }

            if (hit.collider.gameObject.name == MonName && !seed.active && RaySystem.items > 0)
            {
                info.text = "Put down";
                clickImage.enabled = true;
                crosshair.enabled = false;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    source.Stop();
                    source.PlayOneShot(seedPutDown);
                    seed.SetActive(true);
                    seedDown++;
                    info.text = null;
                    clickImage.enabled = false;
                    crosshair.enabled = true;

                }
            }

        }
        else
        {
            info.text = null;
            clickImage.enabled = false;
            crosshair.enabled = true;
        }

        if (seedDown == 5 && !gameFinished)
        {
            gameFinished = true;
            SceneTransition.SwitchToScene("Win Scene"); 
        }
    }
}
