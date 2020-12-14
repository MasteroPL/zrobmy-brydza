############
Szybki start
############

Ekran rozgrywki korzysta z modelu MVC. Wykorzystywane w kontrolerze (klasie głównej - GameScriptManager - oraz klasach odpowiadających za stan) 
odpowiednie elementy interfejsu użytkownika mają wpływ na stan rozgrywki poprzez przypisane im metody. Walidacja zmian odbywa się w komunikacji 
kontroler - stanem tj. przed aktualizacją stanu, kontroler "odpytuje" stan czy nowa jego wartość może zostać wprowadzona. Przykład takowej komunikacji widać poniżej: ::
    
    public class AuctionBaseState : ScriptableObject
    {
        private List<string> possibleContracts;
        public string currentContract { get; set; }
        public PlayerTag currentPlayer { get; set; }
        public bool xEnabled { get; set; }
        public bool xxEnabled { get; set; }

        public void init(PlayerTag firstDeclaringPlayer)
        {
            // some code here
        }

        public bool IsContractConsistent(string newContract)
        {
            if (currentContract == null)
                return true;
            int newContractIndex = possibleContracts.IndexOf(newContract);
            int currentContractIndex = possibleContracts.IndexOf(currentContract);
            return (newContractIndex > currentContractIndex);
        }
    }

Korzystając z powyższej metody "IsContractConsistent", właściwa aktualizacja stanu przebiegać powinna jak na poniższym fragmencie kodu: ::

    bool isContractConsistent = AuctionState.IsContractConsistent(contractString);
    if (isContractConsistent)
    {
        AuctionState.currentContract = contractString;
        AuctionState.xEnabled = false;
        AuctionState.xxEnabled = false;
    }
    else
        return;

