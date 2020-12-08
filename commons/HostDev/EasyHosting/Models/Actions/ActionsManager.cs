using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionsManager<TSessionData>
	{
		public const string ERROR_CODE_NOT_FOUND = "ACTION_NOT_FOUND";
		public const string ERROR_CODE_MANAGER_GENERIC = "GENERIC_MANAGER_ERROR";
		public const string ERROR_CODE_INTERNAL = "INTERNAL_ACTION_ERROR";

		private Dictionary<string, Action<TSessionData>> ActionsDictionary = new Dictionary<string, Action<TSessionData>>();

		public ActionsManager(Dictionary<string, Action<TSessionData>> actionsDictionary) {
			this.ActionsDictionary = actionsDictionary;
		}
		public ActionsManager() { }

		/// <summary>
		/// Dodaje akcję do listy dostępnych akcji w tym managerze
		/// </summary>
		/// <param name="actionName">Nazwa (identyfikator) akcji</param>
		/// <param name="action">Obiekt definiujący akcję</param>
		public void AddAction(string actionName, Action<TSessionData> action) {
			if (ActionsDictionary.ContainsKey(actionName)) {
				throw new ArgumentException("This action name is already defined");
			}

			ActionsDictionary.Add(actionName, action);
		}

		public void AddActions(Dictionary<string, Action<TSessionData>> actions) {
			foreach(var keyValuePair in actions) {
				AddAction(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public JObject PerformActions(TSessionData sessionData, JObject actionsData) {
			var serializer = new ActionsSerializer(actionsData);
			var actionsMeta = serializer.GetActionsMeta();
			
			return PerformActions(sessionData, actionsMeta);
		}

		public JObject PerformActions(TSessionData sessionData, IEnumerable<ActionMeta> actions) {
			var response = new ActionsSerializer();
			response.Actions = new ActionSerializer[actions.Count()];
			JObject jObjPtr;
			
			int index = 0;
			foreach(var action in actions) {
				try {
					jObjPtr = PerformAction(sessionData, action.Name, action.Data);
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

		public JObject PerformAction(TSessionData sessionData, string actionName, JObject actionData) {
			if (!ActionsDictionary.ContainsKey(actionName)) {
				throw new ActionNotFoundException(actionName, "Action " + actionName + " has not been defined for this manager.");
			}

			return ActionsDictionary[actionName].Invoke(sessionData, actionData);
		}
	}

	public struct ActionMeta
	{
		public string Name;
		public JObject Data;
	}
}
