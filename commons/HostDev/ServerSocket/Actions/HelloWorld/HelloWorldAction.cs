using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Actions.HelloWorld {
    class HelloWorldAction : BaseAction {
        public HelloWorldAction() : base(
            typeof(RequestSerializer),
            typeof(RequestSerializer)
        ) { }

        protected override BaseSerializer PerformAction(BaseSerializer requestData) {
            // requestData ma typ taki, jak zdefiniujemy w konstruktorze,
            // więc możemy zrobić proste rzutowanie
            RequestSerializer data = (RequestSerializer)requestData;
            // Od razu inicjalizujemy też serializator do odpowiedzi
            // Poniższa linia robi to samo co "var resp = new ResponseSerializer();"
            RequestSerializer resp = (RequestSerializer)InitializeResponseSerializer();

            Console.WriteLine("Hello " + data.Name + "!");

            resp.Name = "System";
            return resp;
        }
    }
}
