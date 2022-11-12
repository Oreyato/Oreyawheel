using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownManager : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        /*
        dropdown = gameObject.GetComponent<Dropdown>();
        
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
        
        text.text = "First Value : " + dropdown.value;
        */
    }

    void DropdownValueChanged(Dropdown change)
    {
        text.text =  "New Value : " + change.value;
        Debug.Log(text.text);
    }    
}
