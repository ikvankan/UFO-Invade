using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    static public Hero S;
    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    public GameOverScrean gameOverScrean;
    [Header("Set Dinamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject lastTriggerGo = null;
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;

    private void Start()
    {
        if (S == null)
            S = this;
        else
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        //fireDelegate += TempFire;
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);

    }

   

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
        //if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}
    }

    //void TempFire()
    //{
    //    GameObject projGo = Instantiate<GameObject>(projectilePrefab);
    //    projGo.transform.position = transform.position;
    //    Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
    //    //rigidB.velocity = Vector3.up * projectileSpeed;
    //    Projectile proj = projGo.GetComponent<Projectile>();
    //    proj.type = WeaponType.blaster;
    //    float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
    //    rigidB.velocity = Vector3.up * tSpeed;
    //}

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("triggered: " + go.name);
        if (go == lastTriggerGo)
            return;
        lastTriggerGo = go;
        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);
        }
        else if (go.tag == "PowerUp")
        {
            // Если защитное поле столкнулось с бонусом
            AbsorbPowerUp(go);
        }
        else
            print("cтолкнулось с" + go.name);
    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                try
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                    else
                    {
                        ClearWeapon(pu);
                        w = GetEmptyWeaponSlot();
                        w.SetType(pu.type);
                    }
                    break;
                }
                catch 
                {
                    weapons[0].SetType(pu.type);
                    break;
                }
        }
                pu.AbsorbedBy(this.gameObject);
    }
    public float shieldLevel
    {
        get
        {
            return _shieldLevel;
        } 
        set
        { 
            _shieldLevel = Mathf.Min(value, 4) ;
            if (value < 0)
            {
                Destroy(this.gameObject);
                GameOver();
                
                
            }
        }
    }
    void GameOver()
    {
        gameOverScrean.SetUp(ScoreCount.score);
        ScoreCount.score = 0;
    }
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
    void ClearWeapon(PowerUp pu)
    {
        
        if (pu.type == WeaponType.blaster)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].type == WeaponType.spread)
                {
                    weapons[i].SetType(WeaponType.none);
                    break;
                }
            }
        }
        if (pu.type == WeaponType.spread)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].type == WeaponType.blaster)
                {
                    weapons[i].SetType(WeaponType.none);
                    break;
                }
            }
        }
    }
}
