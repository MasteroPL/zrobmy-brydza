using System;
using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;

namespace SocketTesting {
    public class SampleAction : BaseAction {

        public SampleAction() : base(
            typeof(RequestSerializer), // Serializator danych wejściowych
            typeof(ResponseSerializer) // Serializator danych wyjściowych
        ) {}

        // Walidacja danych wejściowych odbywa się w klasie bazowej,
        // tutaj możemy ją pominąć.
        protected override BaseSerializer PerformAction(BaseSerializer requestData) {
            // requestData ma typ taki, jak zdefiniujemy w konstruktorze, 
            // więc możemy zrobić proste rzutowanei 
            RequestSerializer data = (RequestSerializer)requestData;
            // Od razu inicjalizujemy też serializator do odpowiedzi
            // Poniższa linia robi to samo co "var resp = new ResponseSerializer();"
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();

            Console.WriteLine("Hello " + data.MyName + "!");

            resp.HelloWorld = "Hello World!";
            return resp;
        }
    }
}
