using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image crosshair;
    public GameObject stamina;
    public Image seed;
    public Text seedInfo;


    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.isBusy = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerMovement.isBusy = false;
            seed.enabled = true;
            seedInfo.enabled = true;
            stamina.SetActive(true);
            crosshair.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
