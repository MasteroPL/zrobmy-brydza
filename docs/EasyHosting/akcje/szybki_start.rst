############
Szybki Start
############

W pierwszym kroku musimy zdefiniować określoną akcję. Jakie dane będzie przyjmować, jakie zwracać i co będzie robić. Poniżej definicja trywialnej akcji.

.. code-block:: C#
    
    public class SampleAction : BaseAction {
        
        public SampleAction() : base(
            typeof(RequestSerializer), // Serializator danych wejściowych
            typeof(ResponseSerializer) // Serializator danych wyjściowych
        ) {}
        
        // Walidacja danych wejściowych odbywa się w klasie bazowej,
        // tutaj możemy ją pominąć.
        protected override BaseSerializer PerformAction(BaseSerializer requestData) {
            // requestData ma typ taki, jak zdefiniujemy w konstruktorze, 
            // więc możemy zrobić proste rzutowanie 
            RequestSerializer data = (RequestSerializer)requestData;
            // Od razu inicjalizujemy też serializator do odpowiedzi
            // Poniższa linia robi to samo co "var resp = new ResponseSerializer();"
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();

            Console.WriteLine("Hello " + data.MyName + "!");

            resp.HelloWorld = "Hello World!";
            return resp;
        }
    }

W przypadku każdej akcji wymagane są dwa serializatory:

* Serializator danych wejściowych
* Serializator dancyh wyjściowych

Dla powyższego przykładu:

**RequestSerializer**

.. code:: C#
    
    public class RequestSerializer : BaseSerializer {
        // Na wejściu będziemy oczekiwać imienia
        [SerializerField("my_name", required:true)]
        public string MyName;

        public RequestSerializer() : base() { }
        public RequestSerializer(JObject data) : base(data) { }
    }

**ResponseSerializer**

.. code:: C#
    
    public class ResponseSerializer : BaseSerializer {
        // Na wyjściu damy "Hello World"
        [SerializerField("hello_world")]
        public string HelloWorld;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }

.. TIP::
    Więcej o serializatorach w sekcji :doc:`Serializatory <../serializacja/_index>`

Spróbujmy wywołać powyższą akcję

.. code:: C#
    
    var act = new SampleAction();
    JObject resp = act.Invoke(JObject.Parse("{ \"my_name\": \"John\" }"));
    Console.WriteLine("Odpowiedź od akcji: \n" + resp.ToString());

Wynik ::
    
    Hello John!
    Odpowiedź od akcji:
    {
      "hello_world": "Hello World!"
    }

