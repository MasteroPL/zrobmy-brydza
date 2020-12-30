using EasyHosting.Models.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Actions
{
	/// <summary>
	/// Klasa do zarządzania dostępnymi akcjami
	/// </summary>
	public class ActionsManager
	{
		public const string ERROR_CODE_NOT_FOUND = "ACTION_NOT_FOUND";
		public const string ERROR_CODE_MANAGER_GENERIC = "GENERIC_MANAGER_ERROR";
		public const string ERROR_CODE_INTERNAL = "INTERNAL_ACTION_ERROR";

		private Dictionary<string, BaseAction> ActionsDictionary = new Dictionary<string, BaseAction>();

		/// <summary>
		/// Konstruktor z inicjalną definicją słownika akcji
		/// </summary>
		/// <param name="actionsDictionary">Inicjalny słownik akcji</param>
		public ActionsManager(Dictionary<string, BaseAction> actionsDictionary) {
			this.ActionsDictionary = actionsDictionary;
		}
		/// <summary>
		/// Domyślny konstruktor, pozostawia słownik akcji pusty, do manualnego uzupełnienie
		/// </summary>
		public ActionsManager() { }

		/// <summary>
		/// Dodaje akcję do listy dostępnych akcji w tym managerze
		/// </summary>
		/// <param name="actionName">Nazwa (identyfikator) akcji</param>
		/// <param name="action">Obiekt definiujący akcję</param>
		public void AddAction(string actionName, BaseAction action) {
			if (ActionsDictionary.ContainsKey(actionName)) {
				throw new ArgumentException("This action name is already defined");
			}

			ActionsDictionary.Add(actionName, action);
		}

		/// <summary>
		/// Dodaje wiele akcji
		/// </summary>
		/// <param name="actions">Słownik akcji do dodania</param>
		public void AddActions(Dictionary<string, BaseAction> actions) {
			foreach(var keyValuePair in actions) {
				AddAction(keyValuePair.Key, keyValuePair.Value);
			}
		}

		/// <summary>
		/// Wykonuje akcje zdefiniowane w źródłowym JObject.
		/// </summary>
		/// <param name="actionsData">
		/// Definicja akcji. Struktura:
		/// {
		///		"actions": [
		///			{
		///				"name": "action-name-1",
		///				"data": { ... }
		///			},
		///			{
		///				"name": "action-name-2",
		///				"data": { ... }
		///			},
		///			...
		///		]
		/// }
		/// </param>
		/// <returns>
		/// Wyniki każdej akcji w kolejności takiej, w jakiej zdefiniowane były akcje w źródłowym JObject.
		/// Struktura:
		/// {
		///		"actions": [
		///			{
		///				"name": "action-name-1",
		///				"data": (response)
		///			},
		///			{
		///				"name": "action-name-2",
		///				"data": (response)
		///			},
		///			...
		///		]
		/// }
		/// </returns>
		public JObject PerformActions(ClientConnection conn, JObject actionsData) {
			var serializer = new ActionsSerializer(actionsData);
			serializer.Validate();
			var actionsMeta = serializer.GetActionsMeta();
			
			return PerformActions(conn, actionsMeta);
		}

		public JObject PerformActions(ClientConnection conn, IEnumerable<ActionMeta> actions) {
			var response = new ActionsSerializer();
			response.Actions = new ActionSerializer[actions.Count()];
			JObject jObjPtr;
			
			int index = 0;
			foreach(var action in actions) {
				try {
					jObjPtr = PerformAction(conn, action.Name, action.Data);
				} catch(ActionNotFoundException e) {
					jObjPtr = new JObject();
					jObjPtr.Add("error_code", ERROR_CODE_NOT_FOUND);
					jObjPtr.Add("error_message", e.Message);
				} catch(ActionManagerException e) {
					jObjPtr = new JObject();
					jObjPtr.Add("error_code", ERROR_CODE_MANAGER_GENERIC);
					jObjPtr.Add("error_message", e.Message);
				} catch(Exception e) {
					jObjPtr = new JObject();
					jObjPtr.Add("error_code", ERROR_CODE_INTERNAL);
					jObjPtr.Add("error_message", e.Message);
				}
				response.Actions[index] = new ActionSerializer() {
					ActionName = action.Name,
					ActionData = jObjPtr
				};

				index++;
			}

			return response.GetApiObject();
		}

		/// <summary>
		/// Wykonuje pojedynczą akcję
		/// </summary>
		/// <param name="actionName">Nazwa akcji</param>
		/// <param name="actionData">Dane akcji</param>
		/// <returns>Bezpośrednia odpowiedź z wywołania akcji</returns>
		public JObject PerformAction(ClientConnection conn, string actionName, JObject actionData) {
			if (!ActionsDictionary.ContainsKey(actionName)) {
				throw new ActionNotFoundException(actionName, "Action " + actionName + " has not been defined for this manager.");
			}

			return ActionsDictionary[actionName].Invoke(conn, actionData);
		}
		/// <summary>
		/// Wykonuje pojedynczą akcję
		/// </summary>
		/// <param name="actionData">
		/// Dane pojedycznej akcji
		/// Struktura:
		/// {
		///		"name": "action-name-1",
		///		"data": { ... }
		///	}
		/// </param>
		/// <returns>Bezpośrednia odpowiedź z wywołania akcji</returns>
		public JObject PerformAction(ClientConnection conn, JObject actionData) {
			var serializer = new ActionSerializer(actionData);
			serializer.Validate();
			return PerformAction(conn, serializer.ActionName, serializer.ActionData);
        }
	}

	public struct ActionMeta
	{
		public string Name;
		public JObject Data;
	}
}
