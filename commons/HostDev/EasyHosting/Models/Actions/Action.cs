using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyHosting.Models.Actions
{
	public abstract class Action<TSessionData>
	{
		/// <summary>
		/// Wydarzenie wywoływane kiedy dowolna akcja zostanie wywołana
		/// </summary>
		public static event EventHandler<ActionInvokeEventArgs<TSessionData>> Invoked;
		/// <summary>
		/// Wydarzenie wywoływane kiedy ta akcja zostanie wywołana
		/// </summary>
		public event EventHandler<ActionInvokeEventArgs<TSessionData>> InvokedThis;
		/// <summary>
		/// Wydarzenie wywoływane kiedy dowolna akcja się zakończy
		/// </summary>
		public static event EventHandler<ActionFinishEventArgs<TSessionData>> Finished;
		/// <summary>
		/// Wydarzenie wywoływane kiedy ta akcja się zakończy
		/// </summary>
		public event EventHandler<ActionFinishEventArgs<TSessionData>> FinishedThis;

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

		public Action(Type requestSerializerType, Type responseSerializerType) {
			if (!requestSerializerType.IsSubclassOf(typeof(BaseSerializer))) {
				throw new ArgumentException("requestSerializerType has to derive from BaseSerializer");
			}
			if(!responseSerializerType.IsSubclassOf(typeof(BaseSerializer))) {
				throw new ArgumentException("responseSerializerType has to derive from BaseSerializer");
			}

			this._RequestSerializerType = requestSerializerType;
			this._ResponseSerializerType = responseSerializerType;
		}

		public JObject Invoke(TSessionData sessionData, JObject requestData) {
			BaseSerializer requestSerializer = (BaseSerializer)Activator.CreateInstance(RequestSerializerType);
			requestSerializer.SetData(requestData);
			requestSerializer.Validate();

			// Wywołanie wydarzenia
			var argsInvk = new ActionInvokeEventArgs<TSessionData>() {
				SessionData = sessionData,
				RequestData = requestSerializer
			};
			InvokedThis?.Invoke(this, argsInvk);
			Invoked?.Invoke(this, argsInvk);

			// Główna metoda wykonująca akcję
			BaseSerializer responseSerializer = PerformAction(sessionData, requestSerializer);

			// Wywołanie wydarzenia
			var argsFnshd = new ActionFinishEventArgs<TSessionData>() {
				SessionData = sessionData,
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
		/// <returns></returns>
		protected BaseSerializer InitializeResponseSerializer() {
			return (BaseSerializer)Activator.CreateInstance(ResponseSerializerType);
		}

		protected abstract BaseSerializer PerformAction(TSessionData sessionData, BaseSerializer requestData);
	}
}
