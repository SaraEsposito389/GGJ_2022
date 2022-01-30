using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager_2_1 : MonoBehaviour
{

    [SerializeField]
    private GameObject spawningArea;

    [SerializeField]
    private GameObject slippersPile;

    [SerializeField]
    private int numBaseSpawnable = 10;
    private int numMaleSpawnable = 0;
    private int numFemaleSpawnable = 0;
    private int totalNumSpawnable = 0;

    [SerializeField]
    private GameObject spawnableMalePrefab;

    [SerializeField]
    private GameObject spawnableFemalePrefab;

    private int score = 0;

    [SerializeField]
    private string levelToLoad;
    [SerializeField]
    private float delayBeforeChangeScene = 0.75f;

    [SerializeField]
    private IntValue numFemaleThrownSlippers;

    [SerializeField]
    private IntValue numMaleThrownSlippers;

    private bool arePrefabsSpawned;

    // Start is called before the first frame update
    void Start()
    {
        arePrefabsSpawned = false;
        numMaleSpawnable = numBaseSpawnable + numMaleThrownSlippers.GetValue();
        numFemaleSpawnable = numBaseSpawnable + numFemaleThrownSlippers.GetValue();
        totalNumSpawnable = numMaleSpawnable + numFemaleSpawnable;

        

        //UpdateScoreText();

        Debug.Log(totalNumSpawnable);

        GameEvents.Instance.onCollectObject += IncreaseScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!arePrefabsSpawned)
        {
            arePrefabsSpawned = true;
            SpawnPrefabs();
        }
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < numMaleSpawnable; i++)
        {
            SpawnSinglePrefab(spawnableMalePrefab);
        }
        for (int i = 0; i < numFemaleSpawnable; i++)
        {
            SpawnSinglePrefab(spawnableFemalePrefab);
        }
    }

    private void SpawnSinglePrefab(GameObject spawnableObject)
    {
        Vector2 pos = new Vector2(spawningArea.transform.position.x + Random.Range(-spawningArea.transform.localScale.x / 2, spawningArea.transform.localScale.x / 2), spawningArea.transform.position.y + Random.Range(-spawningArea.transform.localScale.y / 2, spawningArea.transform.localScale.y / 2));
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
        var newObj = Instantiate(spawnableObject, pos, rotation);
        newObj.transform.parent = slippersPile.transform;
    }

    
    private void IncreaseScore()
    {
        score++;

        Debug.Log(score);
        
        if (score == totalNumSpawnable) // if with timer check also timer end
        {
            EndMinigame();
        }
    }
    /*
    private void UpdateScoreText()
    {
        scoreText.text = score + "/" + totalNumSpawnable;
    }*/

    private void EndMinigame()
    {
        Debug.Log("End Minigame");
        StartCoroutine(LoadLevelCo(levelToLoad));
    }

    private IEnumerator LoadLevelCo(string levelToLoad)
    {
        yield return new WaitForSeconds(delayBeforeChangeScene);

        SceneManager.LoadScene(levelToLoad);
    }
}
