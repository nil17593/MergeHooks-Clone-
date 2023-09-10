using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour, IDamagable
{
    public int health;
    private  BoxCollider collider;
    public bool canPull = false;
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (canPull)
        {
            PullCar();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            collider.isTrigger = true;
            GameManager.Instance.carControllers.Add(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShredderArea"))
        {
            GameManager.Instance.carControllers.Remove(this);
            GameManager.Instance.AddCash(5);
            Destroy(gameObject);
        }
    }


    public void PullCar()
    {
        transform.Translate(Vector3.back * 5f * Time.deltaTime);
    }
}

interface IDamagable
{
   void TakeDamage(int damage);
}