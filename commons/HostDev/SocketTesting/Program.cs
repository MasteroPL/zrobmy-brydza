using EasyHosting.Models.Actions;
using EasyHosting.Models.Server;
using EasyHosting.Models.Server.Serializers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTesting
{
	class Program
	{
		public static void Debug() {
			JObject obj = JObject.Parse("{ \"actions\": [ { \"name\": \"test\", \"data\": null } ] }");
			var serializer = new ActionsSerializer(obj);

			serializer.Validate();
			var response = serializer.GetApiObject();
			Console.WriteLine(response.ToString());
		}

		static void Main(string[] args) {
			var actMng = new ActionsManager(new Dictionary<string, BaseAction>() {
				// Definicja listy akcji
				{ "sample-action", new SampleAction() }
			});

            var actionsData = "{ \"actions\": [" +
                "{ " +
                    "\"name\": \"sample-action\", " +
                    "\"data\": {" +
                        "\"my_name\": \"John\"" +
                    "}" +
                "}," +
                "{ " +
                    "\"name\": \"sample-action\", " +
                    "\"data\": {" +
                        "\"my_name\": \"Jake\"" +
                    "}" +
                "}," +
                "{ " +
                    "\"name\": \"sample-action\", " +
                    "\"data\": {" +
                        "\"my_name\": \"Justin\"" +
                    "}" +
                "}" +
            "]}";
            Console.WriteLine(actionsData);
            var jobj = JObject.Parse(actionsData);
            //JObject resp = actMng.PerformActions(jobj);
            //Console.WriteLine("Odpowiedź od akcji: \n" + resp.ToString());


            //var act = new SampleAction();
            //JObject resp = act.Invoke(JObject.Parse("{ \"my_name\": \"John\" }"));
            //Console.WriteLine("Odpowiedź od akcji: \n" + resp.ToString());

            //Debug();
            return;

			var serverSocket = new BridgeServerSocket();
			serverSocket.Start();
		}
	}
}
