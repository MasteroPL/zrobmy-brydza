############
Szybki start
############

Prostym przykładem demonstrującym działanie serializatorów danych może być przykład serializatora weryfikującego strukturę danych do logowania. ::
    
    public class UserAuthorizationSerializer : BaseSerializer
    {
        [SerializerField(apiName: "login")]
        public string Login;
        
        [SerializerField(apiName: "password")]
        public string Password;
        
        public UserAuthorizationSerializer() : base() { }
        public UserAuthorizationSerializer(JObject data) : base(data) { }
    }

Rozbijmy powyższy przykład na części. ::
    
    public class UserAuthorizationSerializer : BaseSerializer

**BaseSerializer** jest klasą bazową serializatorów, w której odbywa się mapowanie i właściwa walidacja danych. Więcej o tej klasie w oddzielnej sekcji. ::
    
    [SerializerField(apiName: "login")]
    public string Login;
    
    [SerializerField(apiName: "password")]
    public string Password;

Tu odbywa się definicja struktury danych, oczekiwana przez zdefiniowany serializator. Najprostsza możliwa definicja (taka jak tutaj) zawiera jedynie dwie informacje:

* Nazwa pola w przychodzącej strukturze danych (np. *[SerializerField(apiName: "login")]*, czyli serializator będzie oczekiwać w przychodzącej strukturze danych pola "*login*")
* Oczekiwany typ pola (np. "*string Login;*", zatem oczekiwanym typem będzie "*string*")

**UWAGA:** Typ pola może być jedynie typem prymitywnym, typem dziedziczącym z BaseSerializer lub tablicą jednego z dwóch poprzednich. Więcej o tym w późniejszych sekcjach. ::
    
    public UserAuthorizationSerializer() : base() { }
    public UserAuthorizationSerializer(JObject data) : base(data) { }

Wreszcie dochodzimy do konstruktorów klasy. Zawsze muszą pojawić się dwa konstruktory zdefiniowane tak jak powyżej. Te dwie linie można zawsze kopiować do nowych plików serializatorów, zmieniając jedynie odpowiednio nazwę konstruktorów.


Tak zdefiniowany serializator będzie oczekiwał na wejściu następującej struktury danych: ::
    
    {
        "login": <string>,
        "password": <string>
    }

Sprawdźmy zatem, czy wszystko działa jak powinno: ::
    
    JObject obj = JObject.Parse("{ \"login\": \"test\", \"password\": \"Qwerty123\" }");
    var serializer = new ActionsSerializer(obj);
    
    serializer.Validate(); // WAŻNE: Dopiero po wywołaniu tej metody surowe dane z "obj" zostaną zmapowane do odpowiednich pól klasy (jeśli przejdą walidację)
    var jsonObj = serializer.getApiObject(); // getApiObject zwraca JObject stworzony na podstawie definicji struktury danych naszego serializatora. W tym przypadku powinniśmy otrzymać zwrotnie identyczny obiekt, jak ten tworzony w pierwszej linii.
    
    Console.WriteLine(jsonObj.ToString());

Po uruchomieniu programu ukaże nam się następujący wynik: ::
    
    {
        "login": "test",
        "password": "Qwerty123"
    }

