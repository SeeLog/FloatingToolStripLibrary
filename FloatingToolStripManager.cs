using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FloatingToolStripLibrary
{
    public partial class FloatingToolStripManager
    {
        /// <summary>
        /// FloatingToolStripLibrary.FloatingToolStripManager クラスの新しいインスタンスを初期化します．
        /// </summary>
        public FloatingToolStripManager()
        {

        }

        List<FloatingToolStrip> _floatingtoolstrips = new List<FloatingToolStrip>();
        //public ContextMenuStrip ContextMenu = new ContextMenuStrip();

        /// <summary>
        /// 管理中の FloatingToolStrip 配列
        /// </summary>
        public FloatingToolStrip[] FloatingToolStrips
        {
            set
            {
                _floatingtoolstrips.Clear();
                _floatingtoolstrips.AddRange(value);
            }
            get
            {
                return _floatingtoolstrips.ToArray();
            }
        }

        /// <summary>
        /// FloatingToolStripを追加します．
        /// </summary>
        /// <param name="SourceToolStrip">追加するToolStrip</param>
        /// <param name="ParentPanels">ドッキング先のパネルの配列</param>
        /// <param name="ParentForm">親フォーム</param>
        /// <param name="Text">フローティング時に表示するテキスト</param>
        public void AddFloationgToolStrip(ToolStrip SourceToolStrip, ToolStripPanel[] ParentPanels, Form ParentForm, string Text)
        {
            _floatingtoolstrips.Add(new FloatingToolStrip(SourceToolStrip, ParentPanels, ParentForm, Text));
        }


        /// <summary>
        /// 管理している FloatingToolStrip の中から Textプロパティの一致するものを検索します．
        /// ただし見つからない場合は null を返します．
        /// </summary>
        /// <param name="text">検索する文字列</param>
        /// <param name="startindex">検索を開始する位置</param>
        /// <returns></returns>
        public FloatingToolStrip FindFloatingToolStripByText(string text, int startindex)
        {
            for (int i = startindex; i < _floatingtoolstrips.Count; i++)
            {
                if (_floatingtoolstrips[i].Text == text)
                {
                    return _floatingtoolstrips[i];
                }
            }

            return null;
        }


        /// <summary>
        /// 管理している FloatingToolStrip の中から Textプロパティの一致するものを検索する．
        /// ただし見つからない場合は null を返す．
        /// </summary>
        /// <param name="text">検索する文字列</param>
        /// <returns></returns>
        public FloatingToolStrip FindFloatingToolStripByText(string text)
        {
            return FindFloatingToolStripByText(text, 0);
        }

    }
}

