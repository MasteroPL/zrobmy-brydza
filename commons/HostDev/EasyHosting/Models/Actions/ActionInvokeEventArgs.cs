using EasyHosting.Models.Serialization;

namespace EasyHosting.Models.Actions
{
	public class ActionInvokeEventArgs<TSessionData>
	{
		public TSessionData SessionData;
		public BaseSerializer RequestData;
	}
}
