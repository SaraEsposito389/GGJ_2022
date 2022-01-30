using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private string levelToLoad;

    [SerializeField]
    private float delayBeforeChangeScene = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLevelCo());
    }

    void OnDestroy()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator LoadLevelCo()
    {
        yield return new WaitForSeconds(delayBeforeChangeScene);

        SceneManager.LoadScene(levelToLoad);
    }
}
