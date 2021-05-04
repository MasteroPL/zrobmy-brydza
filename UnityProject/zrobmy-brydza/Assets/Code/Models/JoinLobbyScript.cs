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

public class JoinLobbyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => { JoinLobby(); });
        GameObject.Find("BackButton").GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene(0);
        });
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

        Debug.Log(username + "_" + ipaddress);
        if (username == "" || ipaddress == "")
        {
            return;
        }

        // auth request
        var authData = new AuthData()
        {
            LobbyId = "DEFAULT",
            Login = username,
            LobbyPassword = ""
        };
        var clientSocket = new ClientSocket("127.0.0.1");
        var authRequest = clientSocket.SendRequest(authData.GetApiObject());

        while (authRequest.RequestState != RequestState.RESPONSE_RECEIVED)
        {
            clientSocket.UpdateCommunication();
        }

        // table info request
        var tableInfoRequestData = WrapRequestData("get-table-info", null);
        var tableInfoRequest = clientSocket.SendRequest(tableInfoRequestData.GetApiObject());

        while (tableInfoRequest.RequestState != RequestState.RESPONSE_RECEIVED)
        {
            clientSocket.UpdateCommunication();
        }

        var actionsSerializer = new ActionsSerializer(tableInfoRequest.ResponseData);
        actionsSerializer.Validate();

        var responseSerializer = new GetTableInfoSerializer(actionsSerializer.Actions[0].ActionData);
        try
        {
            responseSerializer.Validate();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }

        if (responseSerializer.Status == "OK")
        {
            UserData.TableData = responseSerializer;
            SceneManager.LoadScene(1);
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
