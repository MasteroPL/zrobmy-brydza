using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Code.UI;

public class HandlerManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // main buttons panel
        GameObject.Find("/Canvas/ButtonsPanel/PlayWithAIButton").GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene(1); // Gameplay scene
        });
        GameObject.Find("/Canvas/ButtonsPanel/PlayViaServerButton").GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene(3); // Tables list scene
        });
        GameObject.Find("/Canvas/ButtonsPanel/QuitButton").GetComponent<Button>().onClick.AddListener(() => {
            Application.Quit();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
