using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info_Controll : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void INF_SHOW(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    public void INF_HIDE(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
