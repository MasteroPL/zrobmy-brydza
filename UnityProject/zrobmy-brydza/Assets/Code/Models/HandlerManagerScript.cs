using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Code.UI;

public class HandlerManagerScript : MonoBehaviour
{
    [SerializeField] Button PlayWithAIButton;
    [SerializeField] Button PlayViaServerButton;
    [SerializeField] Button QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        PlayWithAIButton.onClick.AddListener(() => { SceneManager.LoadScene("Gameplay"); });
        PlayViaServerButton.onClick.AddListener(() => { SceneManager.LoadScene("JoinLobbyView"); });
        QuitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        PlayWithAIButton.onClick.RemoveAllListeners();
        PlayViaServerButton.onClick.RemoveAllListeners();
        QuitButton.onClick.RemoveAllListeners();
    }
}
