using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector:Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10f;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;
    public bool notifietOfDestruction = false;
    
    


    protected BoundsCheck bndCheck;

    public Vector3 pos
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        // Получить материалы и цвет этого игрового объекта и его потомков
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

    }


    void Update()
    {
        Move();
        if (showingDamage && Time.time > damageDoneTime)
        { 
            UnShowDamage();
        }

        if (bndCheck != null && bndCheck.ofDown)
        {
            Destroy(gameObject);
        }
        
    }
    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGo = coll.gameObject;
        switch(otherGo.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGo.GetComponent<Projectile>();
                if(!bndCheck.isOnScreen)
                {
                    Destroy(otherGo);
                    break;
                }
                ShowDamage();
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0)
                {
                    ScoreCount.score += score;
                    if (!notifietOfDestruction){
                        Main.S.ShipDestroyed(this);
                    }
                    notifietOfDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGo);
                break;
            default:
                print("Enemy Hit ne projectile Hero" + otherGo.name);
                break;
        }
    }

    void ShowDamage()
    { 
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }
    void UnShowDamage()
    { 
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }


}
