using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _Type;

    public WeaponType type
    {
        get { return (_Type); } set { SetType( value ); }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid= GetComponent<Rigidbody>();
    }   

    // Update is called once per frame
    void Update()
    {
        if(bndCheck.ofUp)
            Destroy(gameObject);
    }

    public void SetType(WeaponType eType)
    {
        _Type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_Type);
        rend.material.color = def.projctileColor;
    }

}
