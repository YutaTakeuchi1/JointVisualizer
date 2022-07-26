using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Line : MonoBehaviour
{
    float[] allPos;
    Vector3 startPos;
    Vector3[] endPos;
    int loadBodyCnt;
    string[] publicHumanID;
    LineRenderer lineRenderer;
    Vector3[] positions;

    // Start is called before the first frame update
    void Start()
    {
        positions= new Vector3[5000];
        endPos = new Vector3[5000];
        publicHumanID = this.GetComponent<DataReciever>().publicHumanID;
        loadBodyCnt = this.GetComponent<DataReciever>().loadBodyCnt;

        lineRenderer = gameObject.AddComponent<LineRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        allPos = this.GetComponent<SaveBodiesRenderer>().allPos;
        startPos = new Vector3(allPos[42], allPos[43], allPos[44]);

        for (int i = 0; i < loadBodyCnt; i++)
        {
            endPos[i] = GameObject.Find($"loadBodies_{publicHumanID[i]}").transform.GetChild(8).position;

            positions[0] = startPos;
            positions[i + 1] = endPos[i];
        }       

        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.005f;

        //色を付けると少し重くなる
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        lineRenderer.SetPositions(positions);
    }
}
