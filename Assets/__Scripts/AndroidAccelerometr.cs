using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class AndroidAccelerometr : MonoBehaviour

{
    [SerializeField] private float _sensivity;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb= GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    // Update is called once per frame
    private void Move()
    {
        Vector3 acceleration = new Vector3(Input.acceleration.x,Input.acceleration.z,0);
        _rb.AddForce(acceleration * _sensivity);
    }
}
