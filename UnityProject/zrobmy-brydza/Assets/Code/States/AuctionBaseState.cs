﻿using Assets.Code.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerLib.Models;

[CreateAssetMenu(menuName = "AuctionState")]
public class AuctionBaseState : ScriptableObject
{
    private List<Contract> PossibleContracts;
    public Contract CurrentContract { get; set; }
    public Contract ContractCache { get; set; }

    public void Init()
    {
        CurrentContract = null;
        ContractCache = new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.NOBODY);
        PossibleContracts = new List<Contract>();

        foreach(int height in System.Enum.GetValues(typeof(ContractHeight))){
            foreach(int color in System.Enum.GetValues(typeof(ContractColor)))
            {
                if(height != -1 && color != -1)
                {
                    PossibleContracts.Add(new Contract((ContractHeight)height, (ContractColor)color, PlayerTag.NOBODY));
                    PossibleContracts.Add(new Contract((ContractHeight)height, (ContractColor)color, PlayerTag.NOBODY, true));
                    PossibleContracts.Add(new Contract((ContractHeight)height, (ContractColor)color, PlayerTag.NOBODY, false, true));
                }
            }
        }
    }

    public bool DeclareX()
    {
        if (CurrentContract == null || CurrentContract.XEnabled || CurrentContract.XXEnabled)
        {
            return false;
        }
        ContractCache.ContractHeight = CurrentContract.ContractHeight;
        ContractCache.ContractColor = CurrentContract.ContractColor;
        ContractCache.XEnabled = true;
        return true;
    }

    public bool DeclareXX()
    {
        if (CurrentContract == null || !CurrentContract.XEnabled)
        {
            return false;
        }
        ContractCache.ContractHeight = CurrentContract.ContractHeight;
        ContractCache.ContractColor = CurrentContract.ContractColor;
        ContractCache.XEnabled = false;
        ContractCache.XXEnabled = true;
        return true;
    }

    public void DeclareContractHeight(ContractHeight ContractHeight)
    {
        ContractCache.ContractHeight = ContractHeight;
        ContractCache.XEnabled = false;
        ContractCache.XXEnabled = false;
    }

    public void DeclareContractColor(ContractColor ContractColor)
    {
        ContractCache.ContractColor = ContractColor;
        ContractCache.XEnabled = false;
        ContractCache.XXEnabled = false;
    }

    public bool UpdateContract()
    {
        bool isContractConsistent = IsContractConsistent(ContractCache);
        //Debug.Log(ContractCache.ToString() + " : " + isContractConsistent);
        if (isContractConsistent)
        {
            CurrentContract = ContractCache;
            ContractCache = new Contract(ContractHeight.NONE, ContractColor.NONE, PlayerTag.NOBODY);
            return true;
        }
        return false;
    }

    private bool IsContractConsistent(Contract PotencialContract) // check if player declared higher contract that actually is declared
    {
        if (PotencialContract.ContractHeight == ContractHeight.NONE || PotencialContract.ContractColor == ContractColor.NONE)
            return false;
        if (CurrentContract == null)
            return true;
        int newContractIndex = PossibleContracts.FindIndex(item => item.Equals(PotencialContract));
        int currentContractIndex = PossibleContracts.FindIndex(item => item.Equals(CurrentContract));
        return (newContractIndex > currentContractIndex);
    }
}
