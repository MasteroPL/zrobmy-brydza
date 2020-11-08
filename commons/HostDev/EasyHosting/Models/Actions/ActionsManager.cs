using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public class ActionsManager
	{
		private Dictionary<string, IAction> ActionsDictionary = new Dictionary<string, IAction>();

		public JObject PerformActions(JObject actionsData) {
			return null;
		}

		public JObject PerformActions(IEnumerable<ActionMeta> actions) {
			JObject[] responses = new JObject[actions.Count()];
			int index = 0;
			foreach(var action in actions) {
				responses[index] = PerformAction(action.Name, action.Data);
				index++;
			}

			// TODO: serializator do przerobienia odpowiedzi na jedną spójną

			return null;
		}

		public JObject PerformAction(string actionName, JObject actionData) {
			if (!ActionsDictionary.ContainsKey(actionName)) {
				throw new ActionNotFoundException(actionName, "Action " + actionName + " has not been defined for this manager.");
			}

			return ActionsDictionary[actionName].PerformAction(actionData);
		}
	}

	public struct ActionMeta
	{
		public string Name;
		public JObject Data;
	}
}
