using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CinematicWithText
{
    public Sprite cineImage;
    public string associatedText;
}

public class CinematicManager : MonoBehaviour
{
    [SerializeField]
    private List<CinematicWithText> cinematics;
    [SerializeField]
    private Image imageBox;
    [SerializeField]
    private Text textbox;

    [SerializeField]
    private float durationCinematics = 10f;

    [SerializeField]
    private string levelToLoadAtTheEnd;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchCinematicCo());        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SwitchCinematicCo()
    {
        foreach (CinematicWithText cine in cinematics){
            imageBox.sprite = cine.cineImage;
            textbox.text = cine.associatedText;

            yield return new WaitForSeconds(durationCinematics);
        }

        yield return null;
        SceneManager.LoadScene(levelToLoadAtTheEnd);
    }
}
