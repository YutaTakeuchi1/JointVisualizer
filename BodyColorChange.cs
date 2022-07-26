using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Microsoft.Azure.Kinect.BodyTracking;

public class BodyColorChange : MonoBehaviour
{
    void Start()
    {
        float hue = EachHumanColorCalc();
        //各関節に適用
        for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
        {
            this.transform.GetChild(jointNum).gameObject.GetComponent<VisualEffect>().SetFloat("Hue", hue);
        }
    }

    //人毎の色としてランダムな値を返す
    float EachHumanColorCalc()
    {
        float hue = Random.Range(0.1f, 1.0f);
        return hue;
    }
}
