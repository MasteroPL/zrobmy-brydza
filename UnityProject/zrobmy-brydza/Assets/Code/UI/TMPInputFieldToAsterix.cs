using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TMPInputFieldToAsterix : MonoBehaviour
{
    // pass to textToChange Input field to be replays with asterixes
    public TMP_InputField textToChange;
    // Start is called before the first frame update
    void Start()
    {
        textToChange.contentType = TMP_InputField.ContentType.Password;
    }
}
