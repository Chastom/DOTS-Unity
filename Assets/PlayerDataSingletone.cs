using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public float difficulty = 1;
    public float difficultyIncrease = .5f;
    public float maxDifficulty = 20;

    public GameObject GameOverText;
    public GameObject RestartButon;
    public GameObject AmmoTextGO;

    private float DummyTimer;
    private TextMeshProUGUI AmmoText;
    private string CurrentText;

    public GameObject KillCountGO;
    public GameObject TimeSurvivedGO;

    private TextMeshProUGUI KillCountText;
    private TextMeshProUGUI TimeSurvivedText;

    private float ElapsedTime;
    private float timeScale;

    void Awake()
    {
        timeScale = Time.timeScale;
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

        if (ElapsedTime > 270)
            difficulty = 20;
        else if (ElapsedTime > 230)
            difficulty = 13;
        else if (ElapsedTime > 200)
            difficulty = 9;
        else if (ElapsedTime > 170)
            difficulty = 6f;
        else if (ElapsedTime > 140)
            difficulty = 4f;
        else if (ElapsedTime > 110)
            difficulty = 3.3f;
        else if (ElapsedTime > 80)
            difficulty = 2.5f;
        else if (ElapsedTime > 50)
            difficulty = 2f;
        else if (ElapsedTime > 25)
            difficulty = 1.5f;
        else if (ElapsedTime > 10)
            difficulty = 1.2f;


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
            RestartButon.SetActive(true);
        }
    }

    private void UpdateHealthBar()
    {
        HealthVignette.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1 - HP/100);
        //HealthBar.transform.localScale = new Vector3(1f, -(HP / 100));
    }

    public void Restart()
    {
        Time.timeScale = timeScale;
        MachineGun.CurrentAmmo = MachineGun.InitialAmmo;
        Shotgun.CurrentAmmo = Shotgun.InitialAmmo;

        difficulty = 1;

        SceneManager.LoadScene(0);
    }
}
