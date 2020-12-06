############
Wprowadzenie
############
Komunikacja przy użyciu protokołu TCP ma charakter nieustrukturyzowany. Tzn. że nie znamy formalnie struktury ciągu bajtów, który odbiorca otrzymuje. Z uwagi na to potrzebna jest odpowiednia walidacja tego ciągu - sprawdzenie czy posiada wszystkie wymagane pola oraz poprawność tych pól.

Manualnie jest to praca żmudna, wymagająca ciągłego powtarzania tych samych czynności za każdym razem, kiedy wymagana jest walidacja struktury danych oraz samych danych. Dodatkowo taka walidacja jest podatna na pomyłki. Dlatego utworzyliśmy mechanizm serializatorów, które na podstawie definicji klasy obsługują pełną walidację na poziomie struktury danych oraz poprawności typów danych.

W następnych sekcjach opisujemy proste przykłady użycia oraz dokładną dokumentację serializatorów danych.