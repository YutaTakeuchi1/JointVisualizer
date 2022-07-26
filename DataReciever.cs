using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DataReciever : MonoBehaviour
{
    string[,] personData;//レコード毎データを保持

    [SerializeField] GameObject loadBodyOrigin;//インスタンス化の元となる人型オブジェクト
    GameObject loadBodiesTemp;

    //他のスクリプトから使うけどUnityエディタからは編集する必要がない場合このような書き方
    [HideInInspector] public string[] pastTime;//時間を表示するスクリプトに渡す
    [HideInInspector] public string[] publicHumanID;//今回扱う全てのhumanIDを保持する配列
    [HideInInspector] public int loadBodyCnt=0;//publicHumanIDのインデックスの最大値

    int fps = 1;
    string fpsStr = "1";
    int fpsCnt = 0;
    long l = 0;//longじゃなくてもよかった

    

    //UI表示のStart()で受け取るためにAwake()で先に実行
    void Awake()
    {
        pastTime = new string[1000];

        publicHumanID = new string[100];
        BodyInstanciate();
        
        //人毎に保持するデータの数35
        personData = new string[100,36];
        GetColumnCount();
    }   

    //人型オブジェクトのインスタンス化と、後で探しやすい名前の設定
    void BodyInstanciate()
    {
        SqliteDatabase sqlDB = new SqliteDatabase("joint.db");
        string selectQuery = $"select distinct humanID from testJoint where humanID>='{1}' AND humanID<='{2}'";
        DataTable dataTable = sqlDB.ExecuteQuery(selectQuery);

        foreach (DataRow dr in dataTable.Rows)
        {
            loadBodiesTemp = Instantiate(loadBodyOrigin);
            //名前を変える場合はインスタンスしてからすぐじゃないと変わらないみたい
            loadBodiesTemp.name = $"loadBodies_{dr["humanID"].ToString()}";
            //外部に渡せる変数にidを記録
            publicHumanID[loadBodyCnt] = dr["humanID"].ToString();
            //publicHumanIDのインデックスをインクリメント
            loadBodyCnt++;
        }   
    }

    //クエリで呼び出してくる人の中で最大のフレーム数(最も長く映ってたフレーム)を数えて保持
    void GetColumnCount()
    {
        //Sellect 
        SqliteDatabase sqlDB = new SqliteDatabase("joint.db");
        string selectQuery = $"select distinct frameCount from testJoint where humanID>='{1}' AND humanID<='{2}'";
        DataTable dataTable = sqlDB.ExecuteQuery(selectQuery);

        foreach (DataRow dr in dataTable.Rows)
        {
            fpsCnt++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LoadData();
        LoadDataLoop();
    }

    //再生し終わったら再びクエリ(に使う文字列)を初期化(1に)してループ再生をさせる
    void LoadDataLoop()
    {
        if (fpsCnt < int.Parse(fpsStr))
        {
            fpsStr = "1";
        }
    }

    //データの読み出し
    void LoadData()
    {
        //Sellect 
        SqliteDatabase sqlDB = new SqliteDatabase("joint.db");
        string selectQuery = $"select * from testJoint where frameCount='{fpsStr}' AND humanID>='{1}' AND humanID<='{2}'";
        DataTable dataTable = sqlDB.ExecuteQuery(selectQuery);

        foreach (DataRow dr in dataTable.Rows)
        {
            personData[l,0] = dr["frameCount"].ToString();
            personData[l, 1] = dr["humanID"].ToString();
            
            personData[l, 2] = "(" + $"{dr["pelvisX"].ToString()}" + "," + $"{dr["pelvisY"].ToString()}" + "," + $"{dr["pelvisZ"].ToString()}" + ")";
            personData[l, 3] = "(" + $"{dr["spineNavalX"].ToString()}" + "," + $"{dr["spineNavalY"].ToString()}" + "," + $"{dr["spineNavalZ"].ToString()}" + ")";
            personData[l, 4] = "(" + $"{dr["spineChestX"].ToString()}" + "," + $"{dr["spineChestY"].ToString()}" + "," + $"{dr["spineChestZ"].ToString()}" + ")";
            personData[l, 5] = "(" + $"{dr["neckX"].ToString()}" + "," + $"{dr["neckY"].ToString()}" + "," + $"{dr["neckZ"].ToString()}" + ")";
            personData[l, 6] = "(" + $"{dr["leftClavicleX"].ToString()}" + "," + $"{dr["leftClavicleY"].ToString()}" + "," + $"{dr["leftClavicleZ"].ToString()}" + ")";
            personData[l, 7] = "(" + $"{dr["leftShoulderX"].ToString()}" + "," + $"{dr["leftShoulderY"].ToString()}" + "," + $"{dr["leftShoulderZ"].ToString()}" + ")";
            personData[l, 8] = "(" + $"{dr["leftElbowX"].ToString()}" + "," + $"{dr["leftElbowY"].ToString()}" + "," + $"{dr["leftElbowZ"].ToString()}" + ")";
            personData[l, 9] = "(" + $"{dr["leftWristX"].ToString()}" + "," + $"{dr["leftWristY"].ToString()}" + "," + $"{dr["leftWristZ"].ToString()}" + ")";
            personData[l, 10] = "(" + $"{dr["leftHandX"].ToString()}" + "," + $"{dr["leftHandY"].ToString()}" + "," + $"{dr["leftHandZ"].ToString()}" + ")";
            personData[l, 11] = "(" + $"{dr["leftFingerTipX"].ToString()}" + "," + $"{dr["leftFingerTipY"].ToString()}" + "," + $"{dr["leftFingerTipZ"].ToString()}" + ")";
            personData[l, 12] = "(" + $"{dr["leftThumbX"].ToString()}" + "," + $"{dr["leftThumbY"].ToString()}" + "," + $"{dr["leftThumbZ"].ToString()}" + ")";
            personData[l, 13] = "(" + $"{dr["rightClavicleX"].ToString()}" + "," + $"{dr["rightClavicleY"].ToString()}" + "," + $"{dr["rightClavicleZ"].ToString()}" + ")";
            personData[l, 14] = "(" + $"{dr["rightShoulderX"].ToString()}" + "," + $"{dr["rightShoulderY"].ToString()}" + "," + $"{dr["rightShoulderZ"].ToString()}" + ")";
            personData[l, 15] = "(" + $"{dr["rightElbowX"].ToString()}" + "," + $"{dr["rightElbowY"].ToString()}" + "," + $"{dr["rightElbowZ"].ToString()}" + ")";
            personData[l, 16] = "(" + $"{dr["rightWristX"].ToString()}" + "," + $"{dr["rightWristY"].ToString()}" + "," + $"{dr["rightWristZ"].ToString()}" + ")";
            personData[l, 17] = "(" + $"{dr["rightHandX"].ToString()}" + "," + $"{dr["rightHandY"].ToString()}" + "," + $"{dr["rightHandZ"].ToString()}" + ")";
            personData[l, 18] = "(" + $"{dr["rightFingerTipX"].ToString()}" + "," + $"{dr["rightFingerTipY"].ToString()}" + "," + $"{dr["rightFingerTipZ"].ToString()}" + ")";
            personData[l, 19] = "(" + $"{dr["rightThumbX"].ToString()}" + "," + $"{dr["rightThumbY"].ToString()}" + "," + $"{dr["rightThumbZ"].ToString()}" + ")";
            personData[l, 20] = "(" + $"{dr["leftHipX"].ToString()}" + "," + $"{dr["leftHipY"].ToString()}" + "," + $"{dr["leftHipZ"].ToString()}" + ")";
            personData[l, 21] = "(" + $"{dr["leftKneeX"].ToString()}" + "," + $"{dr["leftKneeY"].ToString()}" + "," + $"{dr["leftKneeZ"].ToString()}" + ")";
            personData[l, 22] = "(" + $"{dr["leftAnkleX"].ToString()}" + "," + $"{dr["leftAnkleY"].ToString()}" + "," + $"{dr["leftAnkleZ"].ToString()}" + ")";
            personData[l, 23] = "(" + $"{dr["leftFootX"].ToString()}" + "," + $"{dr["leftFootY"].ToString()}" + "," + $"{dr["leftFootZ"].ToString()}" + ")";
            personData[l, 24] = "(" + $"{dr["rightHipX"].ToString()}" + "," + $"{dr["rightHipY"].ToString()}" + "," + $"{dr["rightHipZ"].ToString()}" + ")";
            personData[l, 25] = "(" + $"{dr["rightKneeX"].ToString()}" + "," + $"{dr["rightKneeY"].ToString()}" + "," + $"{dr["rightKneeZ"].ToString()}" + ")";
            personData[l, 26] = "(" + $"{dr["rightAnkleX"].ToString()}" + "," + $"{dr["rightAnkleY"].ToString()}" + "," + $"{dr["rightAnkleZ"].ToString()}" + ")";
            personData[l, 27] = "(" + $"{dr["rightFootX"].ToString()}" + "," + $"{dr["rightFootY"].ToString()}" + "," + $"{dr["rightFootZ"].ToString()}" + ")";
            personData[l, 28] = "(" + $"{dr["headX"].ToString()}" + "," + $"{dr["headY"].ToString()}" + "," + $"{dr["headZ"].ToString()}" + ")";
            personData[l, 29] = "(" + $"{dr["noseX"].ToString()}" + "," + $"{dr["noseY"].ToString()}" + "," + $"{dr["noseZ"].ToString()}" + ")";
            personData[l, 30] = "(" + $"{dr["eyeLeftX"].ToString()}" + "," + $"{dr["eyeLeftY"].ToString()}" + "," + $"{dr["eyeLeftZ"].ToString()}" + ")";
            personData[l, 31] = "(" + $"{dr["earLeftX"].ToString()}" + "," + $"{dr["earLeftY"].ToString()}" + "," + $"{dr["earLeftZ"].ToString()}" + ")";
            personData[l, 32] = "(" + $"{dr["eyeRightX"].ToString()}" + "," + $"{dr["eyeRightY"].ToString()}" + "," + $"{dr["eyeRightZ"].ToString()}" + ")";
            personData[l, 33] = "(" + $"{dr["earRightX"].ToString()}" + "," + $"{dr["earRightY"].ToString()}" + "," + $"{dr["earRightZ"].ToString()}" + ")";

            personData[l, 34] = dr["date"].ToString();
            personData[l,35] = (string)dr["time"];
            
            //外部に渡せる変数に時刻を代入
            pastTime[l] = personData[l, 35];

            ApplyJointPos();
            
            //次の人のデータを見れるようにインデックスをインクリメント
            l++;
        }
        l = 0;//次のフレームのデータを見るのにまた1人目から見れるようにインデックスを初期化

        fps = int.Parse(personData[l, 0]);
        fps++;//次のレコードに移るためにインクリメント
        fpsStr = fps.ToString();//クエリに使うために文字列に変換

    }

    //各関節に座標を適用する
    void ApplyJointPos()
    {
        //32個の関節
        for (int i = 0; i < 32; i++)
        {
            //今見ているidがpersonData[l, 1]の各ループでの関節のオブジェクトを取得
            GameObject currentJoint = GameObject.Find($"loadBodies_{personData[l, 1]}").transform.GetChild(i).gameObject;
            //i+2はフレーム数とidの2つを飛ばしたインデックス
            Vector3 currentJointPos = StringToVector3(personData[l, i + 2]);
            //currentJointPos.x += 10000.0f;//2画面使うときは座標調整
            currentJoint.transform.position = currentJointPos;//関節に座標を代入

            //PrefabでTrailRendererやVFX Graphをオフにしておくことで、
            //インスタンスされたばかりのときに中心に点が集まったり、そこからのトレイルの描画を防ぐ
            currentJoint.GetComponent<VisualEffect>().enabled = true;
        }
    }

    //StringからVector3に変換
    Vector3 StringToVector3(string input)
    {
        // 前後に丸括弧があれば削除し、カンマで分割
        var elements = input.Trim('(', ')').Split(',');

        var result = Vector3.zero;

        // ループ回数をelementsの数以下かつ3以下にする
        var elementCount = Mathf.Min(elements.Length, 3);

        for (var i = 0; i < elementCount; i++)
        {
            float value;

            float.TryParse(elements[i], out value);
            result[i] = value;
        }
        return result;
    }
}
