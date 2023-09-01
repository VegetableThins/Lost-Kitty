using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Sprite damageSprite;

    public GameObject bloodParticles;

    private float damageDelay = 0.1f;

    [SerializeField]
    private int health;

    private int maxHealth;

    public HealthBar healthBar;

    public bool isPlayer = false;
    public bool isAlive = true;

    private void Start()
    {
        maxHealth = health;
        isAlive = true;
        if (gameObject.tag == "Player")
        {
            isPlayer = true;
            healthBar = GameObject.FindWithTag("Player").GetComponent<HealthBar>();
            FindObjectOfType<GameController>().UpdateHealthUI(health, maxHealth);
            healthBar.SetMaxHealth(health);
        }
        else
        {
            //enemy
            isPlayer = false;
            healthBar.SetMaxHealth(health);
        }
        

    }

    public int GetHealth()
    {
        return health;
    }

    public void DecreaseHealth(int value)
    {
        health -= value;
        healthBar.SetHealth(health -= value);

        var originalSprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(IndicateDamage(originalSprite));
        GetComponent<SpriteRenderer>().sprite = damageSprite;

        if(isPlayer)
        {
            FindObjectOfType<GameController>().UpdateHealthUI(health, maxHealth);
        }

        if (health <= 0)
        {
            //dead
            Instantiate(bloodParticles, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject, damageDelay);
            isAlive = false;

            if (!isPlayer)
            {
                GameController.playerScore += GetComponent<Score>().score;
                FindObjectOfType<GameController>().UpdateScoreUI();
            }
        }
    }

    private IEnumerator IndicateDamage(Sprite sprite)
    {
        //double hits causing a bug where the sprite is locked to the damage sprite
        yield return new WaitForSeconds(damageDelay);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void IncreaseHealth(int value)
    {
        health += value;
        healthBar.SetHealth(health += value);
        if (health + value >= maxHealth)
        {
            health = maxHealth;
        }
    }
}