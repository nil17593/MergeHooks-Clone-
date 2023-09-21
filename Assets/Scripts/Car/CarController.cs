using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour, IDamagable
{
    public int health;
    private  BoxCollider collider;
    public bool canPull = false;
    private int row, column;
    
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

    public void SetPosition(int _row, int _column)
    {
        row = _row;
        column = _column;
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
            if (CarSpawner.Instance.carsOnGrid.Contains(this))
            {
                CarSpawner.Instance.carsOnGrid.Remove(this); 
            }
            GameObject cash = Instantiate(UIController.Instance.cashPrefab, transform.position, UIController.Instance.cashPrefab.transform.rotation);
            GameManager.Instance.AddCash(5);
            CarSpawner.Instance.occupiedPositions[row, column] = false;
            Destroy(gameObject);
        }
    }


    public void PullCar()
    {
        transform.Translate(Vector3.back * 10f * Time.deltaTime);
    }
}

interface IDamagable
{
   void TakeDamage(int damage);
}