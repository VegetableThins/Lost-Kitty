using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    private GameController gameController;

    public GameObject explosionParticle;

    public Sprite flameSprite;

    public Sprite[] numberSprites;

    public int explosionDamage = 3;
    public float explosionRadius = 3f;
    public int explosionDelay = 3;
    public string[] whatExplodes = { "Obstacles", "Interactables" };

    private int explosionTimer = 0;

    // Start is called before the first frame update
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if (collision.gameObject.CompareTag("PlayerMovePoint"))
        {
            gameController.m_TurnEvent.AddListener(ExplosionCheck);
            ExplosionCheck();
        }
    }

    private void ExplosionCheck()
    {
        if (explosionTimer == 0)
        {
            GetComponent<SpriteRenderer>().sprite = flameSprite;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        Debug.Log(explosionTimer);
        transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[explosionDelay - explosionTimer - 1];
        explosionTimer++;
        if (explosionTimer == explosionDelay) Explode();
    }

    private void Explode()
    {
        Instantiate(explosionParticle, gameObject.transform.position, Quaternion.identity);
        RaycastHit2D[] objectsHit = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach (var objectHit in objectsHit)
        {
            GameObject gameObjectHit = objectHit.transform.gameObject;
            Debug.Log("gameobject hit name: " + gameObjectHit.name + "and its health component is..." + gameObjectHit.GetComponent<Health>());
            if (gameObjectHit.GetComponent<Health>() != null)
            {
                Debug.Log("Explosion Damage dealt to: " + gameObjectHit.name);
                gameObjectHit.GetComponent<Health>().DecreaseHealth(explosionDamage);
            }
            else
            {
                foreach (string tag in whatExplodes)
                {
                    if (gameObjectHit.CompareTag(tag))
                    {
                        Destroy(gameObjectHit);
                    }
                }
            }
        }
    }
}