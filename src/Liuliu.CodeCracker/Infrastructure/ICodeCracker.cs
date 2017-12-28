// -----------------------------------------------------------------------
//  <copyright file="ICodeCracker.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-28 11:50</last-date>
// -----------------------------------------------------------------------

using System.Drawing;


namespace Liuliu.CodeCracker.Infrastructure
{
    /// <summary>
    /// 定义验证码识别功能
    /// </summary>
    public interface ICodeCracker
    {
        /// <summary>
        /// 识别验证码
        /// </summary>
        /// <param name="bmp">要识别的图像</param>
        /// <param name="mode">页面分析模式</param>
        /// <returns>识别出的字符串</returns>
        string CrackCode(Bitmap bmp, PageSegMode mode);
    }
}