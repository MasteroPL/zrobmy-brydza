#########
Licytacja
#########

Moduł ten korzysta z podpiętych do niego elementów interfejsu użytkownika (podpięcia do modułu widoczne są w inspektorze). 
Tutaj dodawana jest obsługa zdarzeń dla każdego obiektu UI. Metodę obsługującą zdarzenie można dodać na 2 sposoby: ::

    ElementUI.onClick.AddListener( () => {} );

::

    ElementUI.onClick.AddListener( SomeMethodInClass ); 

W module tym następuje pobranie propozycji kontraktu od gracza, jego walidowanie i aktualizacja licytacji - wszystko zgodnie z konwencją "odpytywania" 
stanu o poprawność nowej wartości i możliwość jej aktualizacji (opisanej w sekcji "Szybki start").

*************************
Obsługa zdarzeń licytacji
*************************

initAuctionModule
=================
Metoda inicjująca stan licytacji poprzez przypisanie nasłuchiwaczy zdarzeń do komponentów UI oraz podpięcie modułu do menedżera rozgrywki.

    :Parametry:
            mainModule : GameScriptManager
                    Menedżer rozgrywki do którego podpinamy moduł licytacji.
            userData : UserData
                    Obiekt przechowujący dane gracza (m. in. jego pseudonim, pozycję przy stole).
            startingPlayer : PlayerTag
                    Identyfikator gracza rozpoczynającego licytację.
    :Zwraca:
            Nie zwraca nic

declareNewContract
==================
Metoda aktualizująca deklarowany kontrakt gry. Korzysta z możliwości auto-walidacji stanu licytacji i w oparciu o odpowiedź go uaktualnia.

    :Parametry:
            Brak
    :Zwraca:
            Nie zwraca nic

Przykład użycia: ::

    bool isContractConsistent = AuctionState.IsContractConsistent(contractString);

Gdzie "contractString" jest reprezentacją kontraktu w postaci łańcucha znaków, zaś AuctionState jest obiektem stanu (podłączonym w inspektorze w środowisku Unity).

**********************************
Stan licytacji i jego aktualizacja
**********************************

Stan licytacji jest obiektem utworzonym w środowisku Unity. Podpięty jest on do instancji klasy "GameManagerScript" 
i tam wykorzystywany do walidowania potencjalnie nowego kontraktu i jego przechowywania. Lista metod instancji klasy "AuctionBaseState" 
(którą wykorzystujemy w środowisku Unity) prezentuje się poniżej :

init
====
    Metoda inicjalizuje stan oraz przeprowadza preprocessing rankingu możliwych kontraktów (bez kontry i rekontry)

    :Parametry:
        firstDeclaringPlayer : PlayerTag
                        Identyfikator gracza rozpoczynającego licytację.
    :Zwraca:
        Nic

IsContractConsistent
====================
    Metoda waliduje podaną jako argument propozycję kontraktu

    :Parametry:
        newContract : string
                        Proponowany nowy kontrakt.

    :Zwraca:
            Nie zwraca nic
            