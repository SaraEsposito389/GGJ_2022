using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManagerUI : MonoBehaviour
{
    [SerializeField]
    private Gender playerGender;

    [SerializeField]
    private List<GameObject> heartsList;




    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i < heartsList.Count; i++)
        {
            if (i < newHealthValue)
            {
                heartsList[i].SetActive(true);
            } else
            {
                heartsList[i].SetActive(false);
            }
        }
    }
}
