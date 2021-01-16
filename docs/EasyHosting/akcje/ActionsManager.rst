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


******************
Dokumentacja klasy
******************

.. sphinxsharp:type:: public class ActionsManager

    Klasa do zarządzania dostępnymi akcjami

Konstruktory
============

.. sphinxsharp:method:: public ActionsManager(Dictionary<string, BaseAction> actionsDictionary)
    :param(1): Inicjalny słownik akcji

    Konstruktor z inicjalną definicją słownika akcji


.. sphinxsharp:method:: public ActionsManager()

    Domyślny konstruktor, pozostawia słownik akcji pusty, do manualnego uzupełnienie

Metody
======

.. sphinxsharp:method:: public void AddAction(string actionName, BaseAction action)
    :param(1): Nazwa (identyfikator) akcji
    :param(2): Obiekt definiujący akcję

    Dodaje akcję do listy dostępnych akcji w tym managerze


.. method::
    public void AddActions(Dictionary<string, BaseAction> actions)

Dodaje wiele akcji

    :Parametry:
        * actions: Dictionary<string, BaseAction>
            Słownik akcji do dodania

    :Wyjątki:
        * ArgumentException
            Rzucany, jeśli nazwa akcji jest już zajęta


.. method::
    public JObject PerformActions(ClientConnection conn, JObject actionsData)

Wykonuje akcje zdefiniowane w źródłowym JObject.

    :Parametry:
        * ClientConnection conn
            Połączenie klienta
        * actionsData: JObject
            Definicja akcji. Struktura: ::

                {
                    "actions": [
                        {
                            "name": "action-name-1",
                            "data": { ... }
                        },
                        {
                            "name": "action-name-2",
                            "data": { ... }
                        },
                        ...
                    ]
                }

    :Zwraca:
        Wyniki każdej akcji w kolejności takiej, w jakiej zdefiniowane były akcje w źródłowym JObject.
        Struktura: ::

            {
                "actions": [
                    {
                        "name": "action-name-1",
                        "data": (response)
                    },
                    {
                        "name": "action-name-2",
                        "data": (response)
                    },
                    ...
                ]
            }


.. method::
    public JObject PerformAction(ClientConnection conn, JObject actionData)

Wykonuje pojedynczą akcję

    :Parametry:
        * conn: ClientConnection
            Połączenie klienta
        * actionData: JObject
            Dane pojedycznej akcji
            Struktura: ::

                {
                	"name": "action-name-1",
            		"data": { ... }
                }

    :Zwraca:
        Bezpośrednia odpowiedź z wywołania akcji

    :Wyjątki:
        * ActionNotFoundException
            Rzucany, jeżeli nie znaleziono akcji o danej nazwie
    

.. method::
    public JObject PerformAction(ClientConnection conn, string actionName, JObject actionData)
    :noindex:

Wykonuje pojedynczą akcję

    :Parametry:
        * conn: ClientConnection
            Połączenie klienta
        * actionName: string
            Nazwa akcji
        * actionData: JObject
            Dane akcji

    :Zwraca:
        Bezpośrednia odpowiedź z wywołania akcji

    :Wyjątki:
        * ActionNotFoundException
            Rzucany, jeżeli nie znaleziono akcji o danej nazwie