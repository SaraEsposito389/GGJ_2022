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

    public event Action<bool> onChangeVisibilityBucket;
    public void ChangeVisibilityBucket(bool visibility)
    {
        onChangeVisibilityBucket?.Invoke(visibility);
    }

}
