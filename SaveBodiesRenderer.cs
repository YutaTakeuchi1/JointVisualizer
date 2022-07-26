using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;
using System;

public class SaveBodiesRenderer : MonoBehaviour
{
    public float[] allPos;
    int fpsCheck = 0;
    string timeStr, dateStr;

    int humanID = 0;
    int maxHumanID = 0;

    [SerializeField] float depthThreshold = 2.5f;//描画しない距離の閾値
    [SerializeField] GameObject saveBodies;//インスタンス化するPrefab

    int[] currentBodiesId;
    int[] preBodiesId;
    int id;
    GameObject saveBodiesTemp;
    bool destroyFrag;  

    // Start is called before the first frame update
    void Start()
    {
        //各関節の座標を一時保存するための配列(32個の関節×3+1)
        allPos = new float[97];

        //エラーが出てしまうので、とりあえず適当な大きさで初期化
        //100人同時に映っても大丈夫
        currentBodiesId = new int[100];
        preBodiesId = new int[100];

        GetMaxHumanID();
    }

    //再起動(実行)時に前回の最大のhumanIDを取得
    void GetMaxHumanID()
    {
        //Sellect 
        SqliteDatabase sqlDB = new SqliteDatabase("joint.db");
        string selectQuery = $"select distinct humanID from testJoint";
        DataTable dataTable = sqlDB.ExecuteQuery(selectQuery);

        foreach (DataRow dr in dataTable.Rows)
        {
            humanID = (int)dr["humanID"];
            if (maxHumanID < humanID)
            {
                maxHumanID = humanID;
            }
        }
    }

    //1人以上トラッキングされていれば呼び出される
    public void updateBodies(BackgroundData trackerFrameData)
    {
        fpsCheck++;
        dateStr = DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Day.ToString().PadLeft(2, '0');
        timeStr = DateTime.Now.ToLongTimeString();

        //そのフレームでトラッキングされたBodyの数だけオブジェクトをインスタンス化
        for (int i = 0; i < (int)trackerFrameData.NumOfBodies; i++)
        {           
            //シーン内に同じ名前のオブジェクトがあればインスタンス化しない
            if (!GameObject.Find($"saveBodies_{(int)trackerFrameData.Bodies[i].Id}"))
            {
                saveBodiesTemp = Instantiate(saveBodies);
                //名前を変える場合はインスタンスしてからすぐじゃないと変わらないみたい
                saveBodiesTemp.name = $"saveBodies_{(int)trackerFrameData.Bodies[i].Id}";
            }

            //現在のフレームでトラッキングしているBodyの中で今回のループ毎に見られているBodyのid
            id = (int)trackerFrameData.Bodies[i].Id;

            //現在のフレームでトラッキングしているBodyのidを保持
            currentBodiesId[i] = id;    

            //先ほど作ったオブジェクトの中に現在見ているidのBodyがあるか探す
            for(int j=0;j< (int)trackerFrameData.NumOfBodies; j++)
            {
                if (id == (int)trackerFrameData.Bodies[j].Id)
                {
                    //探しているBodyが見つかったらそのBodyを一旦保持
                    Body skeleton = trackerFrameData.Bodies[j];

                    //idが以前保存したものと被らないようにする
                    humanID = id + maxHumanID;

                    //遠くの人を表示しないようにする(軌跡を使う場合に有効)
                    var pelvisPosition = skeleton.JointPositions3D[(int)JointId.Pelvis];
                    Vector3 pelvisPos = new Vector3((float)pelvisPosition.X, (float)pelvisPosition.Y, (float)pelvisPosition.Z);

                    //各関節の座標を適用
                    for (int jointNum = 0; jointNum < (int)JointId.Count; jointNum++)
                    {
                        //x.y.zで別々に保持するためにインデックスをずらしながら代入
                        allPos[jointNum * 3] = skeleton.JointPositions3D[jointNum].X;
                        allPos[jointNum * 3+1] = skeleton.JointPositions3D[jointNum].Y;
                        allPos[jointNum * 3+2] = skeleton.JointPositions3D[jointNum].Z;

                        //設定した閾値より近くにいれば表示
                        if (pelvisPos.magnitude < depthThreshold)
                        {
                            Vector3 jointPos = new Vector3(skeleton.JointPositions3D[jointNum].X, skeleton.JointPositions3D[jointNum].Y, skeleton.JointPositions3D[jointNum].Z);
                            GameObject.Find($"saveBodies_{(int)skeleton.Id}").transform.GetChild(jointNum).gameObject.transform.position = jointPos;
                        }
                        else
                        {
                            Destroy(GameObject.Find($"saveBodies_{(int)skeleton.Id}"));
                        }                                      
                    }
                    //クエリが長いため、文字列を結合しながらクエリを作成
                    SqliteDatabase sqlDB = new SqliteDatabase("joint.db");
                    string query = $"insert into testJoint values('{fpsCheck}','{humanID}',";
                    for(int k = 0; k < 96; k++)
                    {
                        query += $"'{allPos[k]}',";
                    }
                    query+=$"'{dateStr}','{timeStr}')";
                    //insert文送信
                    sqlDB.ExecuteNonQuery(query);
                }
            }
        }

        AdjustmentPeople();
    }

    //前のフレームでトラッキングされたBodyのidと同じIdの人がいるか確認
    void AdjustmentPeople()
    { 
        for (int j = 0; j < preBodiesId.Length; j++)
        {
            destroyFrag = true;
            for (int k = 0; k < currentBodiesId.Length; k++)
            {
                //前のフレームと同じIdの人が映っていれば対応するオブジェクトを消さないようにする
                if (currentBodiesId[k] == preBodiesId[j])
                {
                    destroyFrag = false;
                }
            }
            //前のフレームでは映っていたけど、現在のフレームでは映っていないIdのBodyオブジェクトを消す
            if (destroyFrag)
            {
                Destroy(GameObject.Find($"saveBodies_{preBodiesId[j]}"));
            }
        }

        //比較のため配列をコピー
        Array.Copy(currentBodiesId, preBodiesId, currentBodiesId.Length);
    }
}
