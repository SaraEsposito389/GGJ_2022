using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagerUI : MonoBehaviour
{
    [SerializeField]
    private Gender playerGender;

    private Text textField;

    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<Text>();

        if (playerGender == Gender.Female)
        {
            GameEvents.Instance.onChangeFemaleHealth += UpdateUI;
        } else if (playerGender == Gender.Male)
        {
            GameEvents.Instance.onChangeMaleHealth += UpdateUI;
        }
    }

    private void OnDestroy()
    {
        if (playerGender == Gender.Female)
        {
            GameEvents.Instance.onChangeFemaleHealth -= UpdateUI;
        }
        else if (playerGender == Gender.Male)
        {
            GameEvents.Instance.onChangeMaleHealth -= UpdateUI;
        }
    }

    // Update is called once per frame
    void UpdateUI(int newHealthValue)
    {
        textField.text = "Vite Rimanenti: " + newHealthValue;
    }
}
