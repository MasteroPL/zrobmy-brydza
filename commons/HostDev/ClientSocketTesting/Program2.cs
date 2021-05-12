using EasyHosting.Models.Actions;
using EasyHosting.Models.Client;
using EasyHosting.Models.Client.Serializers;
using GameManagerLib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSocketTesting {
	public class Program2 {
		static void OnRequestResponseMacius(object sender, Request request) {
			Console.WriteLine("[Macius]>" + request.ResponseData);
		}
		static void OnSignalMacius(object sender, StandardResponseWrapperSerializer signal) {
			Console.WriteLine("[Macius]>" + signal.Data);
		}
		static void OnRequestResponsePawelek(object sender, Request request) {
			Console.WriteLine("[Pawelek]>" + request.ResponseData);
		}
		static void OnSignalPawelek(object sender, StandardResponseWrapperSerializer signal) {
			Console.WriteLine("[Pawelek]>" + signal.Data);
		}
		static void OnRequestResponseMarcin(object sender, Request request) {
			Console.WriteLine("[Marcin]>" + request.ResponseData);
		}
		static void OnSignalMarcin(object sender, StandardResponseWrapperSerializer signal) {
			Console.WriteLine("[Marcin]>" + signal.Data);
		}

		public static ActionsSerializer WrapRequestData(string actionName, JObject data) {
			var result = new ActionsSerializer();
			result.Actions = new ActionSerializer[1];
			var tmp = new ActionSerializer();

			tmp.ActionName = actionName;
			tmp.ActionData = data;

			result.Actions[0] = tmp;

			return result;
		}

		public static void Main2(string[] args) {
			ClientSocket MaciusSocket = null, PawelekSocket = null, MarcinSocket = null;

			MaciusSocket = new ClientSocket("127.0.0.1");
			PawelekSocket = new ClientSocket("127.0.0.1");
			MarcinSocket = new ClientSocket("127.0.0.1");

			MaciusSocket.RequestResponseReceived += OnRequestResponseMacius;
			MaciusSocket.SignalReceived += OnSignalMacius;
			PawelekSocket.RequestResponseReceived += OnRequestResponsePawelek;
			PawelekSocket.SignalReceived += OnSignalPawelek;
			MarcinSocket.RequestResponseReceived += OnRequestResponseMarcin;
			MarcinSocket.SignalReceived += OnSignalMarcin;

			DumbAI MaciusAI = new DumbAI() {
				ClientSocket = MaciusSocket,
				Position = PlayerTag.N,
				Username = "Macius"
			};
			DumbAI PawelekAI = new DumbAI() {
				ClientSocket = PawelekSocket,
				Position = PlayerTag.W,
				Username = "Pawelek"
			};
			DumbAI MarcinAI = new DumbAI() {
				ClientSocket = MarcinSocket,
				Position = PlayerTag.E,
				Username = "Marcin"
			};

			MaciusAI.Init();
			PawelekAI.Init();
			MarcinAI.Init();

			MaciusAI.Authorize();
			PawelekAI.Authorize();
			MarcinAI.Authorize();

			MaciusAI.LoadGame();
			PawelekAI.LoadGame();
			MarcinAI.LoadGame();

			MaciusAI.Sit();
			PawelekAI.Sit();
			MarcinAI.Sit();

			Console.WriteLine("> Inicjalne ładowanie zakończone");

            while (true) {
				MaciusAI.Play();
				Thread.Sleep(500);
				PawelekAI.Play();
				Thread.Sleep(500);
				MarcinAI.Play();
				Thread.Sleep(500);
			}
		}
	}
}
