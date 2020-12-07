#########
Walidacja
#########

Serializatory pozwalają na zdefiniowanie walidacji dla każdego pola. Walidację można wykonać używając gotowych, zawartych w bibliotece walidatorów, lub użyć własnych.

********************
Wbudowane walidatory
********************

NullValidator
=============
NullValidator określa, czy wartość pola może być NULLem. Pozostałe walidatory domyślnie akceptują NULLa.

Przykład użycia: ::
    
    public class SampleSerializer : BaseSerializer
    {
        [NullValidator(canBeNull: false)]
        public string Value;
    }


RangeValidator
==============
RangeValidator określa akceptowalny zakres wartości dla pola.

Przykład użycia: ::
    
    public class SampleSerializer : BaseSerializer
    {
        [RangeValidator(minValue: <object>, maxValue: <object>)]
        public IComparable Value;
    }

+----------+----------------------------------------------------------------------------------+
| Pole     | Opis                                                                             |
+==========+==================================================================================+
| minValue | Minimalna wartość jaka zostanie zaakceptowana przez walidator.                   |
|          |                                                                                  |
|          | **Jeśli NULL**, minimalna wartość nie będzie uwzględniania                       |
+----------+----------------------------------------------------------------------------------+
| maxValue | Maksymalna wartość jaka zostanie zaakceptowana przez walidator                   |
|          |                                                                                  |
|          | **Jeśli NULL**, maksymalna wartość nie będzie uwzględniania.                     |
+----------+----------------------------------------------------------------------------------+
| Value    | Value lub jakkolwiek nazwane zostanie pole serializatora może być dowolnego      |
|          |                                                                                  |
|          | typu. Ten typ **musi** jednak implementować interfejs IComparable.               |
+----------+----------------------------------------------------------------------------------+


TextLengthValidator
===================
TextLengthValidator określa akceptowalną długość tekstu dla pola.

Przykład użycia: ::
    
    public class SampleSerializer : BaseSerializer
    {
        [TextLengthValidator(minValue: <int>, maxValue: <int>)]
        public string Text;   
    }

+----------+----------------------------------------------------------------------------------+
| Pole     | Opis                                                                             |
+==========+==================================================================================+
| minValue | Minimalna długość tekstu jaka zostanie zaakceptowana przez walidator.            |
|          |                                                                                  |
|          | **Jeśli -1**, minimalna długość nie będzie uwzględniania                         |
+----------+----------------------------------------------------------------------------------+
| maxValue | Maksymalna wartość jaka zostanie zaakceptowana przez walidator                   |
|          |                                                                                  |
|          | **Jeśli -1**, maksymalna długość nie będzie uwzględniania.                       |
+----------+----------------------------------------------------------------------------------+
| Value    | Musi być typu "string", lub typu dziedziczącego ze "string"                      |
+----------+----------------------------------------------------------------------------------+


****************
Ręczna walidacja
****************

W celu zbudowania własnej walidacji pól, należy rozszerzyć metodę "Validate" klasy BaseSerializer.

Przykład użycia: ::
    
    public class SampleSerializer : BaseSerializer
    {
        [TextLengthValidator(minValue: 5, maxValue: 25)]
        public string Value;
        
        public override void Validate(bool throwException = true) {
            // Bazowa walidacja. Tutaj zabraniamy bazowej walidacji wywołać wyjątek.
            // Np. możemy chcieć wykonać wszystkie walidacje przed wyrzuceniem wyjątku, żeby mieć pełną informację o błędzie.
            base.Validate(throwException = false);
            
            // Sekcja w której wykonujemy serię naszych walidacji
            if(Value != "poprawna wartość") {
                AddError("Value", "KOD_BŁĘDU", "TREŚĆ_BŁĘDU");
            }
            
            // Jeżeli były jakieś błędy, sprawdzamy, czy mamy pozwolenie na wyrzucenie wyjątku
            if(Errors.Count > 0 && throwException) {
                ThrowException(); // Wyrzucenie wyjątku
            }
        }
    }

Jeśli chcemy zakończyć walidację od razu, jeżeli bazowa walidacja zwróciła błędy wprowadzimy do powyższego kodu niewielką zmianę: ::
    
    public class SampleSerializer : BaseSerializer
    {
        [TextLengthValidator(minValue: 5, maxValue: 25)]
        public string Value;
        
        public override void Validate(bool throwException = true) {
            // Bazowa walidacja. Tutaj określamy, że wyjątek ma być
            // wyrzucony od razu, jeśli w bazowej walidacji wystąpią błędy
            // oraz jeśloi pozwala na to wywołanie metody
            base.Validate(throwException = throwException);
            
            // Jeżeli walidacja bazowa nie powiodła się, chcemy od razu zakończyć walidację
            if(Errors.Count > 0) {
                return;
            }
            
            // Sekcja w której wykonujemy serię naszych walidacji
            if(Value != "poprawna wartość") {
                AddError("Value", "KOD_BŁĘDU", "TREŚĆ_BŁĘDU");
            }
            
            // Jeżeli były jakieś błędy, sprawdzamy, czy mamy pozwolenie na wyrzucenie wyjątku
            if(Errors.Count > 0 && throwException) {
                ThrowException(); // Wyrzucenie wyjątku
            }
        }
    }
