using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public interface IAction
	{
		JObject PerformAction(JObject actionData);
	}
}
