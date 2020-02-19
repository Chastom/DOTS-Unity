using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager instance = null;
    public  Camera Camera;
    public  Plane Plane;
    public  Gun CurrentGun;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start()
    {
        Camera = Camera.main;
        Plane = new Plane(Vector3.forward, -5f);
        CurrentGun = Gun.Pistol;
    }

    public void ChangeGun(Gun newGun)
    {
        CurrentGun = newGun;
        
        switch(newGun)
        {
            case Gun.Pistol:
                PlayerDataSingletone.instance.UpdateAmmo("∞");
                break;
            case Gun.MachineGun:
                PlayerDataSingletone.instance.UpdateAmmo(MachineGun.CurrentAmmo + "/" + MachineGun.InitialAmmo);
                break;
            default:
                Debug.Log("What have you picked up?!");
                break;
        }
    }
    
}

public enum Gun
{
    Pistol,
    MachineGun,
    Shotgun,
    Lazer,
    LightningGun,
    GrenadeLauncher
}
