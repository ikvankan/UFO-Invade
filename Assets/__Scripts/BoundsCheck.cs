using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Предотвращает выход объекта за границы эеранаб работает только с ортографической камерой и при позиции камеры (0,0,0)
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;
    [Header("Set Dinamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;
    [HideInInspector]
    public bool ofRight, ofLeft, ofUp, ofDown;


    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        ofRight = ofLeft = ofUp = ofDown = false;
        if(pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            ofRight = true;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            ofLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            ofUp = true;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            ofDown = true;
        }
        isOnScreen = !(ofRight || ofLeft || ofUp || ofDown);
        if(keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            ofRight = ofLeft = ofUp = ofDown = false;
        }
        
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        Vector3 boundSize = new Vector3(camWidth*2, camHeight*2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}



