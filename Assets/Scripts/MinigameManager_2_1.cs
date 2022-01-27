using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager_2_1 : MonoBehaviour
{

    [SerializeField]
    private GameObject spawningArea;

    [SerializeField]
    private int numSpawnable = 20;

    [SerializeField]
    private GameObject spawnablePrefab;

    public int score = 0;

    [SerializeField]
    private int minScoreToWin = 10;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPrefabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPrefabs()
    {
        for (int i = 0; i < numSpawnable; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-spawningArea.transform.localScale.x / 2, spawningArea.transform.localScale.x / 2), Random.Range(-spawningArea.transform.localScale.y / 2, spawningArea.transform.localScale.y / 2));
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            Instantiate(spawnablePrefab, pos, rotation);
        }
    }

    void IncreaseScore()
    {
        score++;
        if (score >= minScoreToWin || score == numSpawnable) // if with timer check also timer end
        {
            EndMinigame();
        }
    }

    void EndMinigame()
    {
        Debug.Log("End Minigame");
        // Dialog
        // NextLevel
    }
}
