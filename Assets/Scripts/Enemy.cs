using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject deathEffect;

    [Header("Unity Stuff")] public Image healthBar;

    private bool isDead;

    [HideInInspector] public float speed;

    public float startHealth = 100;

    public float startSpeed = 10f;

    public int worth = 50;

    public float Health { get; private set; }

    private void Start()
    {
        speed = startSpeed;
        Health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        healthBar.fillAmount = Health / startHealth;

        if (Health <= 0 && !isDead) Die();
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    private void Die()
    {
        isDead = true;

        PlayerStats.Money += worth;

        var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }
}