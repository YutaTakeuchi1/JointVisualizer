# JointVisualizer
This Project is using Azure Kinect BodyTracking SDK, SQLiteUnityKit and VFX Graph  

これは私が卒業研究で作成した、UnityとAzure Kinectを用いて人間の身体の関節の動きをトラッキングし、  
関節の動きをグラフィカルに表現できるようにしたシステム(スクリプト)です。  

私のGitHubのプランの性質上、プロジェクトを丸ごと上げることができなかったため(100MB以上のファイルなど)、  
ひとまず自分の書いたスクリプトのファイルだけ上げています。  

この私が作成したスクリプトの中では「Azure Kinect BodyTracking SDK」と「SQLiteUnityKit」が使われており、  
更に表示部分でVFX Graphを用いて関節を表示しています。

### 各スクリプトの説明  

#### SaveBodiesRenderer.cs
・トラッキングした関節の座標データをリアルタイムに保存します。  
・複数人分の関節のデータを表示することも、このスクリプトで行っています。  
・空のオブジェクトにアタッチして、使用します。

#### DataReciever.cs
・過去に記録した関節の座標データをリアルタイムに読みだして、VFX Graphで表示したオブジェクトに座標を代入しています。  
例えば、過去に2人記録されたデータがあれば2人+現在トラッキングされている人数分の関節がビジュアライズ化されます。  
・空のオブジェクトにアタッチして、使用します。

#### PastTimePos.cs
・それぞれの関節に記録された日時をリアルタイムに表示します。

#### Line.cs
・各関節点の座標間をランダムに線分で接続した表現を可能とするスクリプトです。

#### BodyColorChange.cs
・VFX Graphの色を人毎にランダムな色でできるだけ色が被らないように(見づらくならないように)するためのスクリプト。

#### ChangeScale.cs
・関節点の大きさを変えたいときに使用します。

#### DisplayManager.cs
・画面を2つ以上使用する場合に使います。

#### NowTime.cs
・現在時刻を表示したい場合に使います。
