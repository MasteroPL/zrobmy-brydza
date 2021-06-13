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



namespace ClientSocketTesting
{
    class Program3
	{
		static void OnRequestResponseMacius(object sender, Request request)
		{
			Console.WriteLine("[Macius]>" + request.ResponseData);
		}
		static void OnSignalMacius(object sender, StandardResponseWrapperSerializer signal)
		{
			Console.WriteLine("[Macius]>" + signal.Data);
		}
		static void OnRequestResponsePawelek(object sender, Request request)
		{
			Console.WriteLine("[Pawelek]>" + request.ResponseData);
		}
		static void OnSignalPawelek(object sender, StandardResponseWrapperSerializer signal)
		{
			Console.WriteLine("[Pawelek]>" + signal.Data);
		}
		static void OnRequestResponseMarcin(object sender, Request request)
		{
			Console.WriteLine("[Marcin]>" + request.ResponseData);
		}
		static void OnSignalMarcin(object sender, StandardResponseWrapperSerializer signal)
		{
			Console.WriteLine("[Marcin]>" + signal.Data);
		}
		static void OnRequestResponseMarcin2_0(object sender, Request request)
		{
			Console.WriteLine("[Marcin2_0]>" + request.ResponseData);
		}
		static void OnSignalMarcin2_0(object sender, StandardResponseWrapperSerializer signal)
		{
			Console.WriteLine("[Marcin2_0]>" + signal.Data);
		}

		public static ActionsSerializer WrapRequestData(string actionName, JObject data)
		{
			var result = new ActionsSerializer();
			result.Actions = new ActionSerializer[1];
			var tmp = new ActionSerializer();

			tmp.ActionName = actionName;
			tmp.ActionData = data;

			result.Actions[0] = tmp;

			return result;
		}

		public static void Main3(string[] args)
		{
			ClientSocket MaciusSocket = null, PawelekSocket = null, MarcinSocket = null, Marcin2_0Socket;

			MaciusSocket = new ClientSocket("127.0.0.1");
			PawelekSocket = new ClientSocket("127.0.0.1");
			MarcinSocket = new ClientSocket("127.0.0.1");
			Marcin2_0Socket = new ClientSocket("127.0.0.1");

			MaciusSocket.RequestResponseReceived += OnRequestResponseMacius;
			MaciusSocket.SignalReceived += OnSignalMacius;
			PawelekSocket.RequestResponseReceived += OnRequestResponsePawelek;
			PawelekSocket.SignalReceived += OnSignalPawelek;
			MarcinSocket.RequestResponseReceived += OnRequestResponseMarcin;
			MarcinSocket.SignalReceived += OnSignalMarcin;
			Marcin2_0Socket.RequestResponseReceived += OnRequestResponseMarcin2_0;
			Marcin2_0Socket.SignalReceived += OnSignalMarcin2_0;

			UserAI MaciusAI = new UserAI()
			{
				ClientSocket = MaciusSocket,
				Position = PlayerTag.N,
				Username = "Macius"
			};
			UserAI PawelekAI = new UserAI()
			{
				ClientSocket = PawelekSocket,
				Position = PlayerTag.W,
				Username = "Pawelek"
			};
			UserAI MarcinAI = new UserAI() {
			    ClientSocket = MarcinSocket,
			    Position = PlayerTag.E,
			    Username = "Marcin"
			};
			UserAI Marcin2_0AI = new UserAI()
			{
				ClientSocket = Marcin2_0Socket,
				Position = PlayerTag.S,
				Username = "The_real_marcin"
			};

			MaciusAI.Init();
			PawelekAI.Init();
			MarcinAI.Init();
			Marcin2_0AI.Init();

			Marcin2_0AI.Authorize();
			MaciusAI.Authorize();
			PawelekAI.Authorize();
			MarcinAI.Authorize();

			MaciusAI.LoadGame();
			PawelekAI.LoadGame();
			MarcinAI.LoadGame();
			Marcin2_0AI.LoadGame();

			MaciusAI.Sit();
			PawelekAI.Sit();
			MarcinAI.Sit();
			Marcin2_0AI.Sit();

			MaciusAI.LoadGame();
			PawelekAI.LoadGame();
			MarcinAI.LoadGame();
			Marcin2_0AI.LoadGame();

			Console.WriteLine("> Inicjalne ładowanie zakończone");
			Thread.Sleep(500);

			Marcin2_0AI.Game.Start();

			MaciusAI.LoadGame();
			PawelekAI.LoadGame();
			MarcinAI.LoadGame();
			Marcin2_0AI.LoadGame();

			while (true)
			{
				MaciusAI.Play();
				Thread.Sleep(500);
				PawelekAI.Play();
				Thread.Sleep(500);
				MarcinAI.Play();
				Thread.Sleep(500);
				Marcin2_0AI.Play();
				Thread.Sleep(500);
			}
		}
	}
}
