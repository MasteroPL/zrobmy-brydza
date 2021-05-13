using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EasyHosting.Models.Serialization;
using EasyHosting.Meta;
using EasyHosting.Models.Client;
using EasyHosting.Models.Actions;
using Newtonsoft.Json.Linq;
using Assets.Code.UI;
using System;
using GetTableInfoSerializer = ServerSocket.Actions.GetTableInfo.ResponseSerializer;
using System.IO;

public class JoinLobbyScript : MonoBehaviour
{
    // okno bledu dolaczania do stolu
    [SerializeField] GameObject ErrorWindow;
    [SerializeField] Button ErrorCloseButton;

    // okno bledu formularza
    [SerializeField] GameObject ErrorFormWindow;
    [SerializeField] Button ErrorFormCloseButton;

    // Przyciski dla formularza dolaczenia 
    [SerializeField] Button JoinButton;
    [SerializeField] Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        JoinButton.onClick.AddListener(() => { JoinLobby(); });
        BackButton.onClick.AddListener(() => { SceneManager.LoadScene("MainMenuScene"); });
        ErrorCloseButton.onClick.AddListener(() => { ErrorWindow.SetActive(false); });
        ErrorFormCloseButton.onClick.AddListener(() => { ErrorFormWindow.SetActive(false); });
    }

    void OnDestroy()
    {
        JoinButton.onClick.RemoveAllListeners();
        BackButton.onClick.RemoveAllListeners();
        ErrorCloseButton.onClick.RemoveAllListeners();
        ErrorFormCloseButton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {

    }

    static ActionsSerializer WrapRequestData(string actionName, JObject data)
    {
        var result = new ActionsSerializer();
        result.Actions = new ActionSerializer[1];
        var tmp = new ActionSerializer();

        tmp.ActionName = actionName;
        tmp.ActionData = data;

        result.Actions[0] = tmp;

        return result;
    }

    public void JoinLobby()
    {
        string username = GameObject.Find("UsernameInput").GetComponent<InputField>().text;
        string ipaddress = GameObject.Find("IpAddressInput").GetComponent<InputField>().text;

        if (username == "" || ipaddress == "")
        {
            // komunikat o bledzie formularza
            ErrorFormWindow.SetActive(true);
            return;
        }

        // auth request
        var authData = new AuthData()
        {
            LobbyId = "DEFAULT",
            Login = username,
            LobbyPassword = ""
        };
        var clientSocket = new ClientSocket(ipaddress);
        string status = null;
        try
        {
            var authRequest = clientSocket.SendRequest(authData.GetApiObject());
            while (authRequest.RequestState != RequestState.RESPONSE_RECEIVED)
            {
                clientSocket.UpdateCommunication();
            }

            status = authRequest.ResponseData["status"].Value<string>();
        }
        catch(Exception ex)
        {
            // w przypadku gdy nas nie zautoryzuje to nawet nie probujemy pobierac danych stolu bo nie ma po co
            using (StreamWriter outputFile = new StreamWriter("log.txt")) {
                outputFile.WriteLine(ex.Message);
                outputFile.WriteLine(ex.StackTrace);
            }
            ErrorWindow.SetActive(true);
            return;
        }
        if (status != "OK")
        {
            using (StreamWriter outputFile = new StreamWriter("log.txt")) {
                outputFile.WriteLine("wut");
            }
            ErrorWindow.SetActive(true);
        }
        else
        {
            UserData.LoggedIn = true;
            UserData.ClientConnection = clientSocket;
            UserData.Username = username;
            SceneManager.LoadScene("Gameplay");
        }
    }

    public class AuthData : BaseSerializer
    {
        [SerializerField(apiName: "lobby-id")]
        public string LobbyId;

        [SerializerField(apiName: "login")]
        public string Login;

        [SerializerField(apiName: "lobby-password")]
        public string LobbyPassword;
    }
}
