using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Code.UI;
using System.Text.RegularExpressions;
using System.Text;

public class LoginFormHandler : MonoBehaviour
{
    private Regex PasswordRegexValidator;

    // Start is called before the first frame update
    void Start()
    {
        PasswordRegexValidator = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");

        GameObject.Find("LoginButton").GetComponent<Button>().onClick.AddListener(() => { LoginClickHandler(); });
        GameObject.Find("ComeBackButton").GetComponent<Button>().onClick.AddListener(() => { ComeBackClickHandler(); });

        GameObject.Find("PasswordInput").GetComponent<InputField>().onValueChanged.AddListener((value) => { PasswordValidation(value); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoginClickHandler()
    {
        Debug.Log("Login requests has to be sent and handled");
        UserData.LoggedIn = true;

        Debug.Log("Login: " + GameObject.Find("LoginInput").GetComponent<InputField>().text);
        Debug.Log("Haslo: " + GameObject.Find("PasswordInput").GetComponent<InputField>().text);

        SceneManager.LoadScene(0);
    }

    void ComeBackClickHandler()
    {
        SceneManager.LoadScene(0);
    }

    void PasswordValidation(string PasswordInputValue)
    {
        if (!PasswordRegexValidator.IsMatch(PasswordInputValue) || PasswordInputValue.Length >= 32)
        {
            GameObject.Find("PasswordInput/Text").GetComponent<Text>().color = Color.red;
        }
        else
        {
            GameObject.Find("PasswordInput/Text").GetComponent<Text>().color = Color.black;
        }
    }
}
