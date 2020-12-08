using EasyHosting.Models.Serialization;

namespace EasyHosting.Models.Actions
{
	public class ActionFinishEventArgs<TSessionData>
	{
		public TSessionData SessionData;
		public BaseSerializer RequestData;
		public BaseSerializer ResponseData;
	}
}
