using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEvents : MonoBehaviour
{
    private static GameEvents _instance;

    public static GameEvents Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public event Action<int> onChangeScore;
    public void ChangeScore(int newValue)
    {
        onChangeScore?.Invoke(newValue);
    }

    public event Action onDestroyMaleSlipperBullet;
    public void DestroyMaleSlipperBullet()
    {
        onDestroyMaleSlipperBullet?.Invoke();
    }

    public event Action onDestroyFemaleSlipperBullet;
    public void DestroyFemaleSlipperBullet()
    {
        onDestroyFemaleSlipperBullet?.Invoke();
    }

    public event Action<int> onChangeMaleHealth;
    public void ChangeMaleHealth(int newValue)
    {
        onChangeMaleHealth?.Invoke(newValue);
    }

    public event Action<int> onChangeFemaleHealth;
    public void ChangeFemaleHealth(int newValue)
    {
        onChangeFemaleHealth?.Invoke(newValue);
    }

    public event Action onCollectObject;
    public void CollectObject()
    {
        onCollectObject?.Invoke();
    }

    public event Action<string, GameObject> onCollectObjectByTag;
    public void CollectObjectByTag(string tagObj, GameObject keeper)
    {
        onCollectObjectByTag?.Invoke(tagObj, keeper);
    }

    public event Action<GameObject, GameObject> onCollectGameObject;
    public void CollectGameObject(GameObject collectedObj, GameObject keeper)
    {
        onCollectGameObject?.Invoke(collectedObj, keeper);
    }

    public event Action<bool> onChangeVisibilityBucket;
    public void ChangeVisibilityBucket(bool visibility)
    {
        onChangeVisibilityBucket?.Invoke(visibility);
    }

    public event Action onCanTakeClips;
    public void CanTakeClips()
    {
        onCanTakeClips?.Invoke();
    }

    public event Action onSwitchPressed;
    public void SwitchPressed()
    {
        onSwitchPressed?.Invoke();
    }

    public event Action onSwitchPressedEnd;
    public void SwitchPressedEnd()
    {
        onSwitchPressedEnd?.Invoke();
    }

    public event Action onDropNewBucket;
    public void DropNewBucket()
    {
        onDropNewBucket?.Invoke();
    }

    public event Action<Gender> onPlayerDead;
    public void PlayerDead(Gender genderPlayerDead)
    {
        onPlayerDead?.Invoke(genderPlayerDead);
    }

    public event Action onSlipperThrown;
    public void SlipperThrown()
    {
        onSlipperThrown?.Invoke();
    }
}
