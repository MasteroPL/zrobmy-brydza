using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AuctionState")]
public class AuctionBaseState : ScriptableObject
{
    private List<string> possibleContracts;
    public string currentContract { get; set; }
    public PlayerTag currentPlayer { get; set; }

    public void init(PlayerTag firstDeclaringPlayer)
    {
        currentContract = null;
        currentPlayer = firstDeclaringPlayer;
        possibleContracts = new List<string>();
        string[] colors = { "C", "D", "H", "S", "NT" };
        string[] contractHeights = { "1", "2", "3", "4", "5", "6", "7" };
        for (int j = 0; j < contractHeights.Length; j++)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                possibleContracts.Add(contractHeights[j] + colors[i]);
            }
        }
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
