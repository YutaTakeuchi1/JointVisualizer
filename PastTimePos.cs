using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastTimePos : MonoBehaviour
{
    [SerializeField] Camera loadCamera;//UIを表示するカメラ
    [SerializeField] GameObject main;//スクリプト(コンポーネント)を取得するため
    [SerializeField] GameObject uiText;//表示するUIのプレハブ

    RectTransform textRect;
    Transform eachHumanTransform;
    string[] publicHumanID;
    GameObject newUItext;
    GameObject eachNewUIText;
    int loadBodyCnt;

    // Start is called before the first frame update
    void Start()
    {
        //表示する人のIDを保持した配列を取得
        publicHumanID = main.GetComponent<DataReciever>().publicHumanID;
        //publicHunamIDのインデックスの最大値となる値を取得
        loadBodyCnt = main.GetComponent<DataReciever>().loadBodyCnt;
        //まずは人数分テキストオブジェクトをインスタンス化して名前を設定
        for (int i = 0; i < loadBodyCnt; i++)
        {
            newUItext = Instantiate(uiText);
            //Canvasに付けているスクリプトなので、Canvas(自分)を親に設定
            newUItext.transform.parent = this.transform;
            newUItext.name = $"pastTime_{publicHumanID[i]}";           
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < loadBodyCnt; i++)
        {
            //座標の変換で引数に使う(追従させるオブジェクトの)transformを取得
            eachHumanTransform = GameObject.Find($"loadBodies_{publicHumanID[i]}").transform.GetChild(26);
            
            //各人型に対応するUIを取得
            eachNewUIText = GameObject.Find($"pastTime_{publicHumanID[i]}");

            //上で取得したテキストオブジェクトのテキストにLoadTrailで随時読みだしている時間の文字列を表示
            eachNewUIText.GetComponent<Text>().text = main.GetComponent<DataReciever>().pastTime[i];
            
            textRect = eachNewUIText.GetComponent<RectTransform>();
            //Cameraクラスはアタッチしてもプレハブ化するとなぜか消えるのでこのスクリプトからポジションを設定
            textRect.position= RectTransformUtility.WorldToScreenPoint(loadCamera, eachHumanTransform.position);
        }   
    }
}
