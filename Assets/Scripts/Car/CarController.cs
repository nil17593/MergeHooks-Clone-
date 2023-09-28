using UnityEngine;
using DG.Tweening;


public class CarController : MonoBehaviour, IDamagable
{
    public int health;
    private  BoxCollider collider;
    public bool canPull = false;
    private int row, column;
    private Camera cam;
    bool isreachedToShrederArea = false;
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        cam = Camera.main;
    }

    //private void OnEnable()
    //{
    //    CarSpawner.OnCarsPulled += RespawnNewCars;
    //}

    //private void OnDestroy()
    //{
    //    CarSpawner.OnCarsPulled -= RespawnNewCars;
    //}

    private void Update()
    {
        if (canPull && !isreachedToShrederArea)
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
            if (CarSpawner.Instance.carsOnGrid.Contains(this))
            {
                CarSpawner.Instance.carsOnGrid.Remove(this);
            }
            isreachedToShrederArea = true;
            transform.DORotate(new Vector3(-90, 0, 0), 1.5f).OnComplete(() =>
            {
                Vector3 pos = cam.WorldToScreenPoint(transform.position);
                GameObject cash = Instantiate(UIController.Instance.cashPrefab, pos, UIController.Instance.cashPrefab.transform.rotation, UIController.Instance.targetForCash.transform);

                cash.GetComponent<CashAnimation>().MoveCoin(5);
                transform.DOMoveY(-0.50f, .2f).OnComplete(() =>
                {

                    CarSpawner.Instance.occupiedPositions[row, column] = false;
                    if (GameManager.Instance.carControllers.Contains(this))
                    {
                        GameManager.Instance.carControllers.Remove(this);
                    }
                    OnCarPulled();
                    Destroy(gameObject);
                });
            });
        }
    }

    //public void RespawnNewCars()
    //{
    //    //StartCoroutine(GameManager.Instance.SpawnNewCars());
    //    CarSpawner.Instance.ResetGame();
    //    GameManager.Instance.presentGameState = GameManager.GameState.Merging;
    //}

    void OnCarPulled()
    {
        if (GameManager.Instance.carControllers.Count <= 0)
        {
            CarSpawner.Instance.TriggerCarsPulledEvent();
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