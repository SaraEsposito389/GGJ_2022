using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager_2_1 : MonoBehaviour
{

    [SerializeField]
    private GameObject spawningArea;

    [SerializeField]
    private int numBaseSpawnable = 20;
    private int numSpawnable = 0;
    private int level1NumSlippers = 0;

    [SerializeField]
    private GameObject spawnablePrefab;

    private int score = 0;

    [SerializeField]
    private int minScoreToWin = 10;

    [SerializeField]
    private UnityEngine.UI.Text scoreText;

    [SerializeField]
    private IntValue numFemaleThrownSlippers;

    [SerializeField]
    private IntValue numMaleThrownSlippers;

    // Start is called before the first frame update
    void Start()
    {
        level1NumSlippers = numFemaleThrownSlippers.GetValue() + numMaleThrownSlippers.GetValue();
        numSpawnable = numBaseSpawnable + level1NumSlippers;
        SpawnPrefabs();
        GameEvents.Instance.onCollectObject += IncreaseScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < numSpawnable; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-spawningArea.transform.localScale.x / 2, spawningArea.transform.localScale.x / 2), Random.Range(-spawningArea.transform.localScale.y / 2, spawningArea.transform.localScale.y / 2));
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            Instantiate(spawnablePrefab, pos, rotation);
        }
    }

    private void IncreaseScore()
    {
        score++;
        scoreText.text = "" + score;
        if (score == numSpawnable) // if with timer check also timer end
        {
            EndMinigame();
        }
    }

    private void EndMinigame()
    {
        Debug.Log("End Minigame");
        // Dialog
        // NextLevel
    }
}
