using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("ok");
        if (other.gameObject.tag == "fingerTip")
        {
            Debug.Log("ok2");
            Vector3 fingerScale = this.transform.localScale;
            fingerScale *= 2;
            this.transform.localScale = fingerScale;
        }
    }
}
