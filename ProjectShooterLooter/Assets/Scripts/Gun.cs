﻿using UnityEngine;

public enum GunClass
{
    Pistol,
    SMG,
    Shotgun,
    Assault_Rifle,
    Sniper_Rifle
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Gun : MonoBehaviour
{
    private GunClass gunClass;

    private Rarity rarity;

    //Notes: No damage types, just effects:
    // Burn:    Stacking damage over time                  
    // Bleed:   Stacking damage while moving               
    // Chilled: Movement speed reduction                   
    // Frozen:  Actions restricted                         
    // Fear:    Forced movement away from origin           
    // Acid:    Stacking debuff that increases damage taken

    //Always round down
    private int damage;     //damage per bullet
    private int fireRate;   //bullets per sec
    private float accuracy; //determines spread
    private float reloadSpd;//time it takes to reload
    private int magazine;   //how many bullets can be fired before reloading

    private float lastFired;
    private int currentMagazine;
    private float lastReload;
    private bool isReloading;

    public GameObject bullet;

    void Start()
    {
        init(GunClass.Pistol, Rarity.Common, 1, 2, 100, 3, 10);
    }

    private void Update()
    {
        if (isReloading && (Time.time >= lastReload + reloadSpd))
        {
            isReloading = false;
            currentMagazine = magazine;
        }
    }

    public void init(GunClass _gunClass, Rarity _rarity, int _damage, int _fireRate, 
                     float _accuracy, float _reloadSpd, int _magazine)
    {
        gunClass = _gunClass;
        rarity = _rarity;
        damage = _damage;
        fireRate = _fireRate;
        accuracy = _accuracy;
        reloadSpd = _reloadSpd;
        magazine = _magazine;

        lastFired = Time.time - (1 / (float) fireRate);
        currentMagazine = magazine;
        lastReload = Time.time;
        isReloading = false;
    }

    public void generateGun()
    {
        //init();
    }

    public void fire()
    {
        if (checkLastFired() && checkMagazine() && !isReloading)
        {
            lastFired = Time.time;
            currentMagazine--;

            Vector3 dir = transform.up.normalized;
            Debug.Log(dir);
            //dir += new Vector3(0, 0, 0);
            Bullet b = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            b.init(dir, damage, 10);

            Debug.Log("Pew: " + currentMagazine + "/" + magazine);
        }else if(!isReloading && currentMagazine == 0)
        {
            reload();
        }
    }

    public void reload()
    {
        if(currentMagazine < magazine)
        {
            isReloading = true;
            lastReload = Time.time;
        }
    }

    private bool checkLastFired()
    {
        return (Time.time >= lastFired + (1 / (float)fireRate));
    }

    private bool checkMagazine()
    {
        return currentMagazine > 0;
    }
}
