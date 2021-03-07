##############
ActionsManager
##############

***********************
Podstawowe zastosowanie
***********************

Akcjami można zarządzać ręcznie. Można zdefiniować "switch/case", który w zależności od zapytania wybierze odpowiednią akcję. Jest to jednak proces żmudny, jeśli akcji ma być wiele. Co więcej, dodawanie nowych akcji może okazać się skomplikowane.

Zamiast ręcznej obsługi akcji, można skorzystać z klasy **ActionsManager**. Klasa ActionsManager automatycznie znajduje odpowiednią akcję i przekazuje do niej dane wywołania.

Prosta definicja ActionsManagera dla naszej pojedynczej akcji 

.. code-block:: C#
    
    var actMng = new ActionsManager(new Dictionary<string, BaseAction>() {
        // Definicja listy akcji
        { "sample-action", new SampleAction() }
    });

    // Znalezienie akcji i wykonanie jej na podstawie wejściowego JSONa.
    JObject resp = actMng.PerformAction(null, JObject.Parse("{ " +
        "\"name\": \"sample-action\", " +
        "\"data\": {" +
            "\"my_name\": \"John\"" +
        "}" +
    "}"));
    Console.WriteLine("Odpowiedź od akcji: \n" + resp.ToString());

Wynik identyczny jak w przypadku manualnego wywołania ::
    
    Hello John!
    Odpowiedź od akcji:
    {
      "hello_world": "Hello World!"
    }

ActionsManager pozwala również na przetworzenie wielu akcji, jedna po drugiej. W poniższym przykładzie, trzykrotne wykonanie naszej przykładowej akcji.

.. code-block:: C#
    
    var actMng = new ActionsManager(new Dictionary<string, BaseAction>() {
        // Definicja listy akcji
        { "sample-action", new SampleAction() }
    });

    var actionsData = JObject.Parse("{ \"actions\": [" +
        "{ " +
            "\"name\": \"sample-action\", " +
            "\"data\": {" +
                "\"my_name\": \"John\"" +
            "}" +
        "}," +
        "{ " +
            "\"name\": \"sample-action\", " +
            "\"data\": {" +
                "\"my_name\": \"Jake\"" +
            "}" +
        "}," +
        "{ " +
            "\"name\": \"sample-action\", " +
            "\"data\": {" +
                "\"my_name\": \"Justin\"" +
            "}" +
        "}" +
    "]}");
    JObject resp = actMng.PerformActions(null, actionsData);
    Console.WriteLine("Odpowiedź od akcji: \n" + resp.ToString());

Oraz wynik ::
    
    Hello John!
    Hello Jake!
    Hello Justin!
    Odpowiedź od akcji:
    {
      "actions": [
        {
          "name": "sample-action",
          "data": {
            "hello_world": "Hello World!"
          }
        },
        {
          "name": "sample-action",
          "data": {
            "hello_world": "Hello World!"
          }
        },
        {
          "name": "sample-action",
          "data": {
            "hello_world": "Hello World!"
          }
        }
      ]
    }

Jak widać z przykładu, akcje wykonują się dokładnie w takiej kolejności, w jakiej zostały przekazane w danych źródłowych.
