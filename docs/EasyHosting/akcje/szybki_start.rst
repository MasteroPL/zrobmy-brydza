############
Szybki Start
############

Definicja źródłowa akcji jest trywialna. ::
    
    public interface IAction
    {
        JObject PerformAction(JObject actionData);
    }

Aby zdefiniować akcję musimy stworzyć klasę, implementującą ten interfejs. Najprostszy przykład byłby następujący: ::
    
    public class SampleAction : IAction
    {
        // Definicje klasy

        public JObject PerformAction(JObject actionData) {
            // Walidacja danych wejściowych
            var requestSerializer = new SampleRequestSerializer(actionData);
            requestSerializer.Validate()
            
            //
            // Tutaj kod twojej akcji
            //
            
            // Przygotowanie odpowiedzi
            var responseSerializer = new SampleResponseSerializer() {
                Value1 = "sample1",
                Value2 = "sample2",
                Value3 = "sample3"
            };
            return responseSerializer.GetApiObject();
        }
    }