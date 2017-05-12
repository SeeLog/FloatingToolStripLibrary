# FloatingToolStripLibrary
FloatingToolStripLibrary for .NET Framework 3.5

TEST目的で524514833454841年位前に書いた古典を上げてみました．

![スクリーンショット1](http://imgur.com/SHRSx0O.png)
![スクリーンショット2](http://imgur.com/cBOteQj.png)
![スクリーンショット3](http://imgur.com/a0kSJBX.png)
![スクリーンショット4](http://imgur.com/pW40YKa.png)

# 使い方
1. using FloatingToolStripLibrary; を追加
2. フローティング後にToolStripを設置したい場所にToolStripPanel，またはFloatingToolStripGradientPanelを設置する．(両者は背景色にグラデーションが利用できるかどうかの違いしかないのでどっちでもいいです．)
3. 例えば2.でデザイナを使って4つ設置していた場合は以下のようにコーディングします．設置するFormのクラス内にコーディングする場合を想定しています．

```csharp
// 表記めんどくさくてごめんなさい
FloatingToolStrip strip1 = new FloatingToolStrip(
                toolStrip1,
                new ToolStripPanel[]
                {
                    floatingToolStripGradientPanel1,
                    floatingToolStripGradientPanel2,
                    floatingToolStripGradientPanel3,
                    floatingToolStripGradientPanel4
                },
                this,
                "Toolbar");
```
第1引数はフローティングさせたい対象のToolStrip  
第2引数はフローティング後，ドッキング可能なToolStripPanelの配列  
第3引数はToolStripが属するForm  
第4引数はフローティングウインドウに表示するテキスト  
となります．  

もっと簡単にドッキング先を指定できると記述量が減って良さそうですね．いい案があったらほしいです．  

また，上の例であれば，  
`strip1.HideToolStrip();` で非表示に  
`strip1.ShowToolStrip();` で表示することができます．  
`strip1.Showing;` で現在表示しているかどうかが取得できます．

# クラスの説明
適当に何が実装されているかとか書いておきます．  
どうでもいいやつとかは説明省きます．
## FloatingToolStrip.cs
本体です．  
### コンストラクタ
初期化する際に使用します．  
引数として
1. フローティングさせたい対象のToolStrip
2. フローティング後，ドッキング可能なToolStripPanelの配列
3. ToolStripが属するForm
4. フローティングウインドウに表示するテキスト
を与えてください．

### HideToolStripメソッド
FloatingToolStripを非表示にします．

### ShowToolStripメソッド
FloatingToolStripを表示します．  
主にHideToolStripによって非表示にされたものに対して使用します．

### Showing プロパティ
現在(フローティング状態やドッキング状態を問わず)表示しているかどうかを示しています．
読み込み限定のプロパティです．設定したい場合は上記の2つのメソッドを使用します．

### Textプロパティ 
フローティング時のタイトルに表示するテキストを取得または設定します．

### RangeColorプロパティ
ドッキングをする際に，ドッキング先の領域の背景色を取得または設定します．
半透明になるのは仕様です．

### EdgeColorプロパティ
ドッキングをする際に，ドッキング先の領域の境界部分の色を取得または設定します．

## FloatingToolStripGradientPanel.cs
おまけです．System.Windows.Forms.ToolStripPanelを継承して作成しています．  
オリジナルとの違いは背景色をグラデーションにできるだけなので無理に使う必要はありません．  
というか今時ダサいグラデーションしかできないので使わなくていいです．

### BackColorプロパティ
グラデーションの開始色を取得または設定します．

### BackColor2プロパティ
グラデーションの終了色を取得または設定します．

### GradientModeプロパティ
グラデーションの方向を取得または設定します．
System.Drawing.Drawing2D.LinearGradientModeで取得または設定してください．

## FloatingToolStripManager.cs
複数のFloatingToolStripを一括管理するためのクラスです．

### AddFloatingToolStripメソッド
ToolStripをベースとしてFloatingToolStripを作成した後，マネージャーへ追加します．
もし予めFloatingToolStripを作成している場合は，それらを配列にし，FlatingToolStripsプロパティへぶち込んでください．

### FindFloatingToolStripByTextメソッド
その名の通り，Textプロパティが一致するものを検索して返します．見つからなかった場合はnullを返します．  
第2引数にインデックスを指定することで，どこから検索するかを指定することができます．

## FloatingWindow.cs
フローティング時にToolStripを乗せているFormです．ライブラリの外部から使われることを想定していないためinternalにしてあります．

## RectForm.cs
ドッキング時に表示される領域を描画するためのクラスです．  
Formを継承して無理やり実装しています．上に同じく，ライブラリの外部から使われることを想定していないためinternalにしてあります．
