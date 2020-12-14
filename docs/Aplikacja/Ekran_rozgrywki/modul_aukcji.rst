######
Aukcja
######

Moduł ten korzysta z podpiętych do niego elementów interfejsu użytkownika (podpięcia do modułu widoczne są w inspektorze). 
Tutaj dodawana jest obsługa zdarzeń dla każdego obiektu UI. Metodę obsługującą zdarzenie można dodać na 2 sposoby: ::

    ElementUI.onClick.AddListener( () => {} );

::

    ElementUI.onClick.AddListener( SomeMethodInClass ); 

W module tym następuje pobranie propozycji kontraktu od gracza, jego walidowanie i aktualizacja licytacji - wszystko zgodnie z konwencją "odpytywania" 
stanu o poprawność nowej wartości i możliwość jej aktualizacji (opisanej w sekcji "Szybki start").
