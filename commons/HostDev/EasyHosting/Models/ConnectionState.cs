using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models
{
	public enum ConnectionState
	{
		IDLE = 0,
		PROCESSING_REQUEST = 1,
		AWATING_RESPONSE = 2
	}
}
