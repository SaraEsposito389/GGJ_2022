using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField]
    private string levelToLoad;

    [SerializeField]
    private float delayBeforeChangeScene = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onEndLevel += LoadNewLevel;
    }

    void OnDestroy()
    {
        GameEvents.Instance.onEndLevel -= LoadNewLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNewLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    private IEnumerator LoadLevelCo()
    {
        yield return new WaitForSeconds(delayBeforeChangeScene);

        SceneManager.LoadScene(levelToLoad);
    }
}
