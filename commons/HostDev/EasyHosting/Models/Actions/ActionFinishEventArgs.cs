using EasyHosting.Models.Serialization;

namespace EasyHosting.Models.Actions
{
	public class ActionFinishEventArgs
	{
		public BaseSerializer RequestData;
		public BaseSerializer ResponseData;
	}
}
