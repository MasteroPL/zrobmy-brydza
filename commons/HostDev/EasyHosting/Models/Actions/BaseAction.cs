using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	/// <summary>
	/// Klasa do definiowania jednostkowych czynności, wywoływanych przez API
	/// </summary>
	public abstract class BaseAction
	{
		/// <summary>
		/// Wydarzenie wywoływane kiedy dowolna akcja zostanie wywołana
		/// </summary>
		public static event EventHandler<ActionInvokeEventArgs> Invoked;
		/// <summary>.,
		/// Wydarzenie wywoływane kiedy ta akcja zostanie wywołana
		/// </summary>
		public event EventHandler<ActionInvokeEventArgs> InvokedThis;
		/// <summary>
		/// Wydarzenie wywoływane kiedy dowolna akcja się zakończy
		/// </summary>
		public static event EventHandler<ActionFinishEventArgs> Finished;
		/// <summary>
		/// Wydarzenie wywoływane kiedy ta akcja się zakończy
		/// </summary>
		public event EventHandler<ActionFinishEventArgs> FinishedThis;

		private Type _RequestSerializerType;
		/// <summary>
		/// Serializator używany do walidacji danych wejściowych
		/// </summary>
		public Type RequestSerializerType { get { return _RequestSerializerType; } protected set { _RequestSerializerType = value; } }

		private Type _ResponseSerializerType;
		/// <summary>
		/// Serializator używany do przygotowania odpowiedzi (danych wyjściowych)
		/// </summary>
		public Type ResponseSerializerType { get { return _ResponseSerializerType; } protected set { _ResponseSerializerType = value; } }


		/// <param name="requestSerializerType">Serializator danych wejściowych</param>
		/// <param name="responseSerializerType">Serializator danych wyjściowych</param>
		public BaseAction(Type requestSerializerType, Type responseSerializerType) {
			if (!requestSerializerType.IsSubclassOf(typeof(BaseSerializer))) {
				throw new ArgumentException("requestSerializerType has to derive from BaseSerializer");
			}
			if(!responseSerializerType.IsSubclassOf(typeof(BaseSerializer))) {
				throw new ArgumentException("responseSerializerType has to derive from BaseSerializer");
			}

			this._RequestSerializerType = requestSerializerType;
			this._ResponseSerializerType = responseSerializerType;
		}

		/// <summary>
		/// Wywołuje wykonanie akcji
		/// </summary>
		/// <param name="requestData">Dane wejściowe</param>
		/// <returns>Odpowiedź od akcji</returns>
		public JObject Invoke(JObject requestData) {
			BaseSerializer requestSerializer = (BaseSerializer)Activator.CreateInstance(RequestSerializerType);
			requestSerializer.SetData(requestData);
			requestSerializer.Validate();

			// Wywołanie wydarzenia
			var argsInvk = new ActionInvokeEventArgs() {
				RequestData = requestSerializer
			};
			InvokedThis?.Invoke(this, argsInvk);
			Invoked?.Invoke(this, argsInvk);

			// Główna metoda wykonująca akcję
			BaseSerializer responseSerializer = PerformAction(requestSerializer);

			// Wywołanie wydarzenia
			var argsFnshd = new ActionFinishEventArgs() {
				RequestData = requestSerializer,
				ResponseData = responseSerializer
			};
			FinishedThis?.Invoke(this, argsFnshd);
			Finished?.Invoke(this, argsFnshd);

			// Zwrócenie odpowiedzi
			return responseSerializer.GetApiObject();
		}

		/// <summary>
		/// Inicjalizuje serializator odpowiedzi w oparciu o zdefiniowany w konstruktorze typu serializator
		/// </summary>
		/// <returns>Zainicjalizowany serializator</returns>
		protected BaseSerializer InitializeResponseSerializer() {
			return (BaseSerializer)Activator.CreateInstance(ResponseSerializerType);
		}


		/// <summary>
		/// Właściwa metoda wykonująca akcję. Otrzymuje na wejściu zwalidowane dane po walidacji
		/// </summary>
		/// <param name="requestData">Dane wejściowe wpisane do serializatora. Serializator przekazywany na wejściu jest typu "requestSerializerType", definiowanego w konstruktorze</param>
		/// <returns>Odpowiedź w postaci serializatora</returns>
		protected abstract BaseSerializer PerformAction(BaseSerializer requestData);
	}
}
