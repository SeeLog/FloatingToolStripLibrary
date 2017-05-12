# FloatingToolStripLibrary
FloatingToolStripLibrary for .NET Framework 3.5

TEST目的で524514833454841年位前に書いた古典を上げてみました．


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
