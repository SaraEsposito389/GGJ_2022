using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    [SerializeField]
    private string femaleWinScene;
    [SerializeField]
    private string maleWinScene;
    [SerializeField]
    private float delayBeforeChangeScene = 0.75f;

    [SerializeField]
    private float timeBattle = 0f;
    [SerializeField]
    private string sceneNoTimeNoBullet;
    [SerializeField]
    private string sceneNoTimeBullets;
    private bool isSlipperThrown;

    private bool isChangingLevel;
    private Coroutine timedFightCo = null;

    // Start is called before the first frame update
    void Start()
    {
        isSlipperThrown = false;
        isChangingLevel = false;

        GameEvents.Instance.onPlayerDead += AfterPlayerDead;
        GameEvents.Instance.onSlipperThrown += DisableGoodEnding;

        if (timeBattle > 0f)
        {
            timedFightCo = StartCoroutine(StartTimedFightCo(timeBattle));
        }
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onPlayerDead -= AfterPlayerDead;
        GameEvents.Instance.onSlipperThrown -= DisableGoodEnding;
    }

    private IEnumerator StartTimedFightCo(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (isSlipperThrown)
        {
            StartCoroutine(LoadLevelCo(sceneNoTimeBullets));
        } else
        {
            StartCoroutine(LoadLevelCo(sceneNoTimeNoBullet));
        }
    }

    private void DisableGoodEnding()
    {
        isSlipperThrown = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AfterPlayerDead(Gender genderDeadPlayer)
    {
        if (!isChangingLevel)
        {
            isChangingLevel = true;
            string levelToLoad = "";
            if (genderDeadPlayer == Gender.Male)
            {
                levelToLoad = femaleWinScene;
            }
            else if (genderDeadPlayer == Gender.Female)
            {
                levelToLoad = maleWinScene;
            }

            if (timedFightCo != null)
            {
                StopCoroutine(timedFightCo);
            }

            StartCoroutine(LoadLevelCo(levelToLoad));
        }
    }

    private IEnumerator LoadLevelCo(string levelToLoad)
    {
        yield return new WaitForSeconds(delayBeforeChangeScene);

        SceneManager.LoadScene(levelToLoad);
    }
}
