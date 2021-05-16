using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    //public void Hit()
    //{
    //    if (gameObject.GetComponent<Enemy>() != null)
    //    {
    //        var enemy = gameObject.GetComponent<Enemy>();
    //        enemy.Die();
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}