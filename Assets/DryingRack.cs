using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryingRack : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> spawnPoints;
    private List<GameObject> freeSpawnPoints;
    private List<GameObject> occupiedSpawnPoints;

    [SerializeField]
    private List<Sprite> spriteSheetList = new List<Sprite>();

    [SerializeField]
    private GameObject sheetPrefab;

    [SerializeField]
    private float secBeforeSpawn = 2f;
    [SerializeField]
    private float secBeforeFalling = 3f;

    private bool objectiveReached;

    [SerializeField]
    private GameObject clipPrefab;

    // Start is called before the first frame update
    void Start()
    {
        freeSpawnPoints = new List<GameObject>();
        occupiedSpawnPoints = new List<GameObject>();
        foreach (GameObject obj in spawnPoints)
        {
            freeSpawnPoints.Add(obj);
        }

        objectiveReached = false;

        GameEvents.Instance.onTryToClipSheet += SpawnClip;
        StartCoroutine(SpawnCo());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator SpawnCo()
    {
        while (!objectiveReached)
        {
            yield return new WaitForSeconds(secBeforeSpawn);
            if (freeSpawnPoints.Count > 0)
            {
                GameObject spawnPoint = freeSpawnPoints[Random.Range(0, freeSpawnPoints.Count)];
                SpawnSheet(spawnPoint);
                occupiedSpawnPoints.Add(spawnPoint);
                freeSpawnPoints.Remove(spawnPoint);
            }
        }
    }

    private void SpawnClip(GameObject spawnPointSelected)
    {
        if (occupiedSpawnPoints.Contains(spawnPointSelected))
        {
            occupiedSpawnPoints.Remove(spawnPointSelected);
            spawnPoints.Remove(spawnPointSelected);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-45, 45)));
            GameObject newClip = Instantiate(clipPrefab, spawnPointSelected.transform.position, rotation);
            newClip.transform.parent = spawnPointSelected.transform;
            newClip.transform.position = new Vector2(newClip.transform.position.x, newClip.transform.position.y + 0.5f);
        }

        if (spawnPoints.Count == 0)
        {
            objectiveReached = true;
            Debug.Log("End level");
            GameEvents.Instance.EndLevel();
        }
    }

    private void SpawnSheet(GameObject spawnPoint)
    {
        GameObject sheet = Instantiate(sheetPrefab, spawnPoint.transform.position, Quaternion.identity);
        sheet.GetComponent<SpriteRenderer>().sprite = spriteSheetList[Random.Range(0, spriteSheetList.Count)];
        sheet.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        sheet.transform.parent = gameObject.transform;
        StartCoroutine(FallingSheetCo(sheet, spawnPoint));
    }

    private IEnumerator FallingSheetCo(GameObject sheet, GameObject spawnPoint)
    {
        yield return new WaitForSeconds(secBeforeFalling);
        if (spawnPoints.Contains(spawnPoint)) // it has to fall
        {
            Rigidbody2D rbSheet = sheet.GetComponent<Rigidbody2D>();
            rbSheet.gravityScale = 1;

            Destroy(sheet, 0.5f);
            yield return new WaitForSeconds(1.0f);

            freeSpawnPoints.Add(spawnPoint);
        }
    }
}
