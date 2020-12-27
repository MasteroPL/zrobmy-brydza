using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionsManager
	{
		public const string ERROR_CODE_NOT_FOUND = "ACTION_NOT_FOUND";
		public const string ERROR_CODE_MANAGER_GENERIC = "GENERIC_MANAGER_ERROR";
		public const string ERROR_CODE_INTERNAL = "INTERNAL_ACTION_ERROR";

		private Dictionary<string, Action> ActionsDictionary = new Dictionary<string, Action>();

		public ActionsManager(Dictionary<string, Action> actionsDictionary) {
			this.ActionsDictionary = actionsDictionary;
		}
		public ActionsManager() { }

		/// <summary>
		/// Dodaje akcję do listy dostępnych akcji w tym managerze
		/// </summary>
		/// <param name="actionName">Nazwa (identyfikator) akcji</param>
		/// <param name="action">Obiekt definiujący akcję</param>
		public void AddAction(string actionName, Action action) {
			if (ActionsDictionary.ContainsKey(actionName)) {
				throw new ArgumentException("This action name is already defined");
			}

			ActionsDictionary.Add(actionName, action);
		}

		public void AddActions(Dictionary<string, Action> actions) {
			foreach(var keyValuePair in actions) {
				AddAction(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public JObject PerformActions(JObject actionsData) {
			var serializer = new ActionsSerializer(actionsData);
			var actionsMeta = serializer.GetActionsMeta();
			
			return PerformActions(actionsMeta);
		}

		public JObject PerformActions(IEnumerable<ActionMeta> actions) {
			var response = new ActionsSerializer();
			response.Actions = new ActionSerializer[actions.Count()];
			JObject jObjPtr;
			
			int index = 0;
			foreach(var action in actions) {
				try {
					jObjPtr = PerformAction(action.Name, action.Data);
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

		public JObject PerformAction(string actionName, JObject actionData) {
			if (!ActionsDictionary.ContainsKey(actionName)) {
				throw new ActionNotFoundException(actionName, "Action " + actionName + " has not been defined for this manager.");
			}

			return ActionsDictionary[actionName].Invoke(actionData);
		}
	}

	public struct ActionMeta
	{
		public string Name;
		public JObject Data;
	}
}
