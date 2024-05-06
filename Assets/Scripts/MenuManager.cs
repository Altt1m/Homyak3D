using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static bool isLoading;
    // Start is called before the first frame update
    void Start()
    {
        isLoading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Escape From Homyak" && !isLoading)
        {
            isLoading = true;
            SceneTransition.SwitchToScene("Main Menu");
        }
    }
}
