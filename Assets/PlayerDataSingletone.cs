using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSingletone : MonoBehaviour
{
    public static PlayerDataSingletone instance = null;
    public int HP = 100;
    public GameObject HealthBar;
    public int EnemiesKilled = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public void InflictDamage(int damage)
    {
        HP -= damage;
        HealthBar.transform.localScale = new Vector3( 1f, -((float)HP / 100));
    }
}
