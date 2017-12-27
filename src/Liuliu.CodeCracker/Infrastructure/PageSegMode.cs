using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liuliu.CodeCracker.Infrastructure
{
    /// <summary>
    /// 表示可能的页面布局的分析模式
    /// </summary>
    public enum PageSegMode
    {
        /// <summary>
        /// 0 =只进行定向和脚本检测（OSD）
        /// </summary>
        OsdOnly,
        /// <summary>
        /// 1 =通过OSD进行页面自动分割
        /// </summary>
        AutoOsd,
        /// <summary>
        /// 2 =自动分割，但没有OSD，或OCR
        /// </summary>
        AutoOnly,
        /// <summary>
        /// 3 =全自动翻页分割，但没有OSD（默认）
        /// </summary>
        Auto,
        /// <summary>
        /// 4 =假设待识别图片是一列的文本
        /// </summary>
        SingleColumn,
        /// <summary>
        /// 5 =假设待识别图片是一个统一的垂直对齐的文本块
        /// </summary>
        SingleBlockVertText,
        /// <summary>
        /// 6 =假设待识别图片是一个统一的文本块
        /// </summary>
        SingleBlock,
        /// <summary>
        /// 7 =把图像作为一个单一的文本行
        /// </summary>
        SingleLine,
        /// <summary>
        /// 8 =把图像当作一个字
        /// </summary>
        SingleWord,
        /// <summary>
        /// 9 =把图像作为一个字在一个圆圈中
        /// </summary>
        CircleWord,
        /// <summary>
        /// 10 =把图像当作一个单独的字符
        /// </summary>
        SingleChar,

        Count
    }
}
