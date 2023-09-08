using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour, IDamagable
{
    public int health;
    private  BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            collider.isTrigger = true;
        }
    }
}

interface IDamagable
{
   void TakeDamage(int damage);
}