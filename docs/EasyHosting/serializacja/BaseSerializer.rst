####################
Klasa BaseSerializer
####################

Poniżej zamieszczona jest techniczna dokumentacja klasy BaseSerializer


**************
Publiczne pola
**************
+------------+----------------------------------------------+-------------------------------------------------------------+
| Pole       | Typ                                          | Opis                                                        |
+============+==============================================+=============================================================+
| DataOrigin | JObject                                      | Przechowuje oryginalny obiekt JSONa przekazany              |
|            |                                              |                                                             |
|            |                                              | przekazany do serializatora                                 |
+------------+----------------------------------------------+-------------------------------------------------------------+
| Errors     | Dictionary<FieldInfo, List<ValidationError>> | Słownik błędów, które wystąpiły podczas walidacji           |
|            |                                              |                                                             |
|            |                                              | (pole -> lista błędów dla pola)                             |
|            |                                              |                                                             |
|            |                                              | Pod wartością NULL zapisana jest lista błędów globalnych    |
+------------+----------------------------------------------+-------------------------------------------------------------+


****************
Publiczne metody
****************

SetData
^^^^^^^
+--------------------------------------------------------------------------------------+
| SetData(data)                                                                        |
+======================================================================================+
| Ustawia dane źródłowe dla serializatora                                              |
+--------+-----------------+---------------+-------------------------------------------+
| param  | data            | JObject       | Dane źródłowe dla serializatora           |
+--------+-----------------+---------------+-------------------------------------------+

Validate
^^^^^^^^
+--------------------------------------------------------------------------------------------+
| Validate(throwsException = true)                                                           |
+============================================================================================+
| Wywołuje walidację danych źródłowych przypisanych do serializatora                         |
|                                                                                            |
| Walidacja jednocześnie przypisuje zwalidowane wartości do odpowiednio                      |
|                                                                                            |
| przypisanych do nich pól.                                                                  |
+--------+-----------------+---------------------+-------------------------------------------+
| param  | throwsException | bool                | Określa, czy wywołanie metody może rzucić |
|        |                 |                     |                                           |
|        |                 |                     | wyjątkiem w przypadku nieudanej walidacji |
+--------+-----------------+---------------------+-------------------------------------------+
| throws |                 | ValidationException | Wyjątek wyrzucany w przypadku             |
|        |                 |                     |                                           |
|        |                 |                     | nieprawidłowej walidacji.                 |
+--------+-----------------+---------------------+-------------------------------------------+

GetApiObject
^^^^^^^^^^^^
+---------------------------------------------------------------------------------------------+
| GetApiObject(): JObject                                                                     |
+=============================================================================================+
| Zwraca JObject o formacie zdefiniowanym jako format API (W atrybucie SerializerField        |
|                                                                                             |
| argument apiName)                                                                           |
+---------+-----------------+---------------------+-------------------------------------------+
| returns |                 | JObject             | JObject w formacie danych API (apiName)   |
+---------+-----------------+---------------------+-------------------------------------------+


****************
Chronione metody
****************

AddError
^^^^^^^^
+---------------------------------------------------------------------------------------------+
| AddError(fieldName, errorCode, errorMessage)                                                |
+=============================================================================================+
| Służy do dodawania błędów walidacji do słownika błędów dla wybranego pola. Zaten oidczas    |
|                                                                                             |
| walidacji, jeśli stwierdzimy, że pole "Value" jest nieprawidłowe, dodamy dla niego błąd     |
|                                                                                             |
| wywołując AddError("Value", "KOD_BŁĘDU", "TREŚĆ_BŁĘDU");                                    |
+---------+-----------------+---------------------+-------------------------------------------+
| param   | fieldName       | string              | Nazwa pola w klasie, dla którego ma       |
|         |                 |                     |                                           |
|         |                 |                     | dodany błąd.                              |
|         |                 |                     |                                           |
|         |                 |                     | Jeśli NULL, błąd zostanie dodany do       |
|         |                 |                     | błędów                                    |
|         |                 |                     |                                           |
|         |                 |                     | globalnych.                               |
+---------+-----------------+---------------------+-------------------------------------------+
| param   | errorCode       | string              | Kod błędu, który ma być jednoznacznym     |
|         |                 |                     |                                           |
|         |                 |                     | identyfikatorem błędu, który wystąpił.    |
|         |                 |                     |                                           |
|         |                 |                     | Kod błędu jest informacją dla programu.   |
+---------+-----------------+---------------------+-------------------------------------------+
| param   | errorMessage    | string              | Treść błędu. Treść jest informacją dla    |
|         |                 |                     |                                           |
|         |                 |                     | programisty, który będzie czytał ten błąd |
+---------+-----------------+---------------------+-------------------------------------------+
| throws  |                 | ArgumentException   | Wyjątek wyrzucany w przypadku             |
|         |                 |                     | nieznalezienia                            |
|         |                 |                     |                                           |
|         |                 |                     | pola o podanej nazwie                     |
+---------+-----------------+---------------------+-------------------------------------------+