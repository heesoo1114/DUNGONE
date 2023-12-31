using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public Action playerDieEvent;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth; // ���߿� �Ѵ� private��
    public int CurrentHealth => currentHealth;
    public bool IsLiving => (currentHealth > 0);

    [Header("Sound")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;

        // ui update

        if (currentHealth <= 0)
        {
            Debug.Log("Player Die");
            playerDieEvent.Invoke();
            MakeSound(dieSound);

            // ������ �����ؾ� �Ѵ�.
            return;
        }

        // hurt sound
        MakeSound(hurtSound);
    }

    private void MakeSound(AudioClip clip)
    {
        AudioObj audioObjClone = PoolManager.Instance.Pop("AudioObj") as AudioObj;
        audioObjClone.PlayClip(clip);
    }
}
