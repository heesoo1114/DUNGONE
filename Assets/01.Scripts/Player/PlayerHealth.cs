using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action OnPlayerDieEvent;

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
        IsAlive = true;
        currentHealth = maxHealth;
        _healthBarUI.SettingRatio(maxHealth);
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
            OnPlayerDieEvent.Invoke();
            MakeSound(dieSound);

            GameManager.Instance.OnGameDone();
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
