using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject {
    [SerializeField] string text; 

    public string GetStateText() {
        return text;
    }
}
