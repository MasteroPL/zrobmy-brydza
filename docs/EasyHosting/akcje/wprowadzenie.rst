############
Wprowadzenie
############

Podstawową instancją wywoływaną przez komunikację z serwerem jest "akcja". Akcją określamy pojedynczą czynność, o wykonanie której instancja wysyłająca zapytanie może poprosić.

Przykłady "akcji":

* Wyłożenie karty
* Rozdanie kart
* Rozpoczęcie gry
* Prośba o pauzę

Idea akcji jest następująca:

* Akcja otrzymuje dane zapytania w formacie JObject, czyli w formacie nieustrukturyzowanym
* Akcja zostaje wykonana w oparciu o dane wejściowe
* Akcja zwraca odpowiedź w formacie JObject

API przekazuje zatem do akcji bezpośrednio dane wejściowe oraz zwrotnie oczekuje bezpośrednio danych wyjściowych.

W dalszych sekcjach znajduje się opis definicji a oraz rejestrowania akcji przy użyciu "ActionsManager" oraz w jaki sposób dokonywać rejestracji akcji w przypadku socketa serwerowego.