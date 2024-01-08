using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public Action playerDieEvent;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth; // 나중에 둘다 private로
    public int CurrentHealth => currentHealth;
    public bool IsAlive { get; private set; }

    [SerializeField] private HealthBarUI _healthBarUI;

    [Header("Sound")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    private void Start()
    {
        currentHealth = maxHealth;
        _healthBarUI.SetValue(currentHealth);
    }

    public void OnDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        // ui update
        _healthBarUI.SetValue(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player Die");

            IsAlive = false;
            playerDieEvent.Invoke();
            MakeSound(dieSound);
        }
        else
        {
            // hurt sound
            MakeSound(hurtSound);
        }
    }

    private void MakeSound(AudioClip clip)
    {
        AudioObj audioObjClone = PoolManager.Instance.Pop("AudioObj") as AudioObj;
        audioObjClone.PlayClip(clip);
    }
}
