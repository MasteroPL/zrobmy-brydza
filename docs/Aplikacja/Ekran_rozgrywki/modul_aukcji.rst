############
Moduł aukcji
############

Moduł ten korzysta z podpiętych do niego elementów interfejsu użytkownika (podpięcia do modułu widoczne są w inspektorze). 
Tutaj dodawana jest obsługa zdarzeń dla każdego obiektu UI. Metodę obsługującą zdarzenie można dodać na 2 sposoby: ::

    ElementUI.onClick.AddListener( () => {} );

::

    ElementUI.onClick.AddListener( SomeMethodInClass ); 