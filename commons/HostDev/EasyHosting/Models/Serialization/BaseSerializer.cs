using EasyHosting.Meta;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyHosting.Models.Serialization
{
	public class BaseSerializer
	{
		public JObject DataOrigin { get; private set; }

		private Dictionary<string, List<string>> _Errors = null;
		public Dictionary<string, List<string>> Errors { get { return _Errors; } private set { _Errors = value; } }

		/// <summary>
		/// Server -> User   Serializer constructor
		/// </summary>
		public BaseSerializer() {

		}
		/// <summary>
		/// User -> Server   Serializer constructor
		/// </summary>
		/// <param name="data"></param>
		public BaseSerializer(JObject data) {
			DataOrigin = data;

			var fields = this.GetType().GetFields().Where(
				prop => Attribute.IsDefined(prop, typeof(SerializerFieldAttribute), false)
			);

			if(_Errors == null) {
				_Errors = new Dictionary<string, List<string>>();
			}
			else {
				_Errors.Clear();
			}

			foreach(var field in fields) {
				var fieldMetas = (SerializerFieldAttribute[])field.GetCustomAttributes(typeof(SerializerFieldAttribute), false);

				// Only a single definition allowed
				if(fieldMetas.Length > 1) {
					throw new ConfigurationException("Invalid configuration for SerializerField, field name: " + field.Name + "; in class: " + this.GetType().Name + "; Only accepts 1 definition of SerializerFieldAttribute");
				}

				var fieldMeta = fieldMetas.First();
				if (!data.ContainsKey(fieldMeta.ApiName)) {
					throw new InvalidInputDataException("Missing required ");
				}
			}
		}
	}
}
