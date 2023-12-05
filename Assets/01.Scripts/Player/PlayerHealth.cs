using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public Action playerDieEvent;

    [SerializeField] private int maxHp;
    private int currentHp; // 나중에 둘다 private로

    public int CurrentHP
    {
        get => currentHp;
        set
        {
            currentHp = value;
            Debug.Log("Player is hurt");

            if (currentHp <= 0)
            {
                playerDieEvent.Invoke();
                Debug.Log("Player Die");
            }
        }
    }

    public bool IsLiving() => (currentHp > 0);

    private void Start()
    {
        InitHp();
    }

    public void InitHp()
    {
        currentHp = maxHp;
    }

    public void OnDamage(int damage)
    {
        currentHp -= damage;
    }
}
