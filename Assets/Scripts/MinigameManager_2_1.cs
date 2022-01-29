using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager_2_1 : MonoBehaviour
{

    [SerializeField]
    private GameObject spawningArea;

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
    private UnityEngine.UI.Text scoreText;

    [SerializeField]
    private IntValue numFemaleThrownSlippers;

    [SerializeField]
    private IntValue numMaleThrownSlippers;

    // Start is called before the first frame update
    void Start()
    {
        numMaleSpawnable = numBaseSpawnable + numMaleThrownSlippers.GetValue();
        numFemaleSpawnable = numBaseSpawnable + numFemaleThrownSlippers.GetValue();
        totalNumSpawnable = numMaleSpawnable + numFemaleSpawnable;

        SpawnPrefabs();

        UpdateScoreText();

        GameEvents.Instance.onCollectObject += IncreaseScore;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Vector2 pos = new Vector2(Random.Range(-spawningArea.transform.localScale.x / 2, spawningArea.transform.localScale.x / 2), Random.Range(-spawningArea.transform.localScale.y / 2, spawningArea.transform.localScale.y / 2));
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
        Instantiate(spawnableObject, pos, rotation);
    }

    private void IncreaseScore()
    {
        score++;
        UpdateScoreText();
        if (score == totalNumSpawnable) // if with timer check also timer end
        {
            EndMinigame();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score + "/" + totalNumSpawnable;
    }

    private void EndMinigame()
    {
        Debug.Log("End Minigame");
        // Dialog
        // NextLevel
    }
}
