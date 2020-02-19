using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private float DummyTimer;
    private TextMeshProUGUI AmmoText;
    private string CurrentText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public void Start()
    {
        HealthVignette.SetActive(true);
        AmmoText = AmmoTextGO.GetComponent<TextMeshProUGUI>();
    }

    public void FixedUpdate()
    {
        DummyTimer -= Time.fixedDeltaTime;

        if (DummyTimer <= 0)
        {
            AddHealth(HealthPointsIncrementSize);
            DummyTimer = HealthPointsIncrementTime;
        }
        AmmoText.text = CurrentText;
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

    private void UpdateHealthBar()
    {
        HealthVignette.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1 - HP/100);
        //HealthBar.transform.localScale = new Vector3(1f, -(HP / 100));
    }

    public void UpdateAmmo(string text)
    {
        CurrentText = text;
        //AmmoText.text = text;
    }
}
