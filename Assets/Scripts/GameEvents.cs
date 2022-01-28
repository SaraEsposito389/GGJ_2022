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

}
