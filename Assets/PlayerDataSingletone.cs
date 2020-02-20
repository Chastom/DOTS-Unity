using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PlayerDataSingletone : MonoBehaviour
{
    public static PlayerDataSingletone instance = null;
    public GameObject HealthBar;
    public GameObject HealthVignette;
    public float HP = 100;
    public float HealthPointsIncrementSize = 1;
    public float HealthPointsIncrementTime = 2;
    public int EnemiesKilled = 0;

    public GameObject GameOverText;
    public GameObject AmmoTextGO;
    public List<float> RandomnessList;

    private float DummyTimer;
    private TextMeshProUGUI AmmoText;
    private string CurrentText;

    public GameObject KillCountGO;
    public GameObject TimeSurvivedGO;

    private TextMeshProUGUI KillCountText;
    private TextMeshProUGUI TimeSurvivedText;

    private float ElapsedTime;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public void Start()
    {

        ElapsedTime = 0;
        HealthVignette.SetActive(true);
        AmmoText = AmmoTextGO.GetComponent<TextMeshProUGUI>();
        KillCountText = KillCountGO.GetComponent<TextMeshProUGUI>();
        TimeSurvivedText = TimeSurvivedGO.GetComponent<TextMeshProUGUI>();
    }

    public void FixedUpdate()
    {
        ElapsedTime += Time.fixedDeltaTime;
        DummyTimer -= Time.fixedDeltaTime;

        if (DummyTimer <= 0)
        {
            AddHealth(HealthPointsIncrementSize);
            DummyTimer = HealthPointsIncrementTime;
        }
       

        KillCountText.text = "Kill count: " + EnemiesKilled;
        TimeSurvivedText.text = "Time Survived: " + Math.Round(ElapsedTime);

        switch (GunManager.instance.CurrentGun)
        {
            case Gun.Pistol:
                AmmoText.text = CurrentText = "∞";
                break;
            case Gun.MachineGun:
                AmmoText.text = CurrentText = MachineGun.CurrentAmmo + "/" + MachineGun.InitialAmmo;
                break;
            case Gun.Shotgun:
                AmmoText.text = CurrentText = Shotgun.CurrentAmmo + "/" + Shotgun.InitialAmmo;
                break;
            default:
                Debug.Log("What have you picked up?!");
                break;
        }
    }

    public void AddHealth(float health)
    {
        if (HP + health <= 100)
            HP += health;
        else
            HP = 100;

        UpdateHealthBar();
    }

    public void InflictDamage(float damage)
    {
        HP -= damage;

        if (HP > 0)
            UpdateHealthBar();
        else
        {
            HP = 0;
            UpdateHealthBar();
            Time.timeScale = 0; // Stop game xd
            GameOverText.SetActive(true);
        }
    }


    private void GenerateRandomness()
    {
        Random r = new Random();
        for (int x = 0; x <= 1000; x++)
        {
            RandomnessList.Add(-0.5f+(float)r.NextDouble());
        }
    }
    private void UpdateHealthBar()
    {
        HealthVignette.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1 - HP/100);
        //HealthBar.transform.localScale = new Vector3(1f, -(HP / 100));
    }
}
