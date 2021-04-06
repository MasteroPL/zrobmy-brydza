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
        GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions").SetActive(false); // right corner popup 

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

        // right corner popup
        GameObject.Find("/Canvas/LoginCorner/RightCornerPopupButton").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions").SetActive(!GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions").activeSelf);
        });
        
        // down arrow for popup
        //GameObject.Find("/Canvas/LoginCorner/RightCornerLoginLabel/RightCornerLoginLabelText").GetComponent<Button>().onClick.AddListener(() => { });  // right upper corner label with user nickname : RightCornerLoginLabel
        
        // right corner popup button options
        GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions/ProfileButton").GetComponent<Button>().onClick.AddListener(() => { });  // change scene for 'profile'
        GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions/LogInOutButton").GetComponent<Button>().onClick.AddListener(() => {
            if (UserData.LoggedIn)
            {
                GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions/LogInOutButton/LogInOutButtonText").GetComponent<Text>().text = "Wyloguj";
                // TODO perform logout request
            }
            else
            {
                GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions/LogInOutButton/LogInOutButtonText").GetComponent<Text>().text = "Zaloguj";
                SceneManager.LoadScene(2);
            }
        }); // logout user

    }

    // Update is called once per frame
    void Update()
    {
        if (UserData.LoggedIn)
        {
            GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions/LogInOutButton/LogInOutButtonText").GetComponent<Text>().text = "Wyloguj";
        }
        else
        {
            GameObject.Find("/Canvas/LoginCorner/RightCornerLoginOptions/LogInOutButton/LogInOutButtonText").GetComponent<Text>().text = "Zaloguj";
        }
    }
}
