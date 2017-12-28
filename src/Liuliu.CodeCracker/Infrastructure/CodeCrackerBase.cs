// -----------------------------------------------------------------------
//  <copyright file="CodeCrackerBase.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-28 11:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;

using OSharp.Utility.Extensions;

using Tesseract;


namespace Liuliu.CodeCracker.Infrastructure
{
    /// <summary>
    /// 验证码识别基类
    /// </summary>
    public abstract class CodeCrackerBase : ICodeCracker
    {
        private readonly TesseractEngine _engine;

        /// <summary>
        /// 初始化一个<see cref="CodeCrackerBase"/>类型的新实例
        /// </summary>
        protected CodeCrackerBase()
            : this("eng", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
        { }

        /// <summary>
        /// 初始化一个<see cref="CodeCrackerBase"/>类型的新实例
        /// </summary>
        protected CodeCrackerBase(string language, string charlist)
            : this(language, charlist, "tessdata")
        { }

        /// <summary>
        /// 初始化一个<see cref="CodeCrackerBase"/>类型的新实例
        /// </summary>
        protected CodeCrackerBase(string language, string charlist, string tesspath)
        {
            if (!Directory.Exists(tesspath))
            {
                throw new DirectoryNotFoundException($"未找到字库路径：“{tesspath}”");
            }
            if (!File.Exists(Path.Combine(tesspath, $"{language}.traineddata")))
            {
                throw new FileNotFoundException($"字典路径“{tesspath}”无法找到字库“{language}.traineddata”");
            }
            TesseractEngine engine = new TesseractEngine(tesspath, language, EngineMode.Default);
            if (charlist != null)
            {
                engine.SetVariable("tessedit_char_whitelist", charlist);
            }
            _engine = engine;
        }

        /// <summary>
        /// 识别验证码
        /// </summary>
        /// <param name="bmp">要识别的图像</param>
        /// <param name="mode">页面分析模式</param>
        /// <returns>识别出的字符串</returns>
        public string CrackCode(Bitmap bmp, PageSegMode mode)
        {
            Bitmap binBmp = Binary(bmp);
            return CrackCodeCore(binBmp, mode);
        }

        /// <summary>
        /// 将图片二值化，去噪点，返回白色背景的纯字符图片
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        protected abstract Bitmap Binary(Bitmap bmp);

        /// <summary>
        /// 识别白底黑字的二值化字符图片
        /// </summary>
        /// <param name="bmp">白底黑字的二值化字符图片</param>
        /// <param name="mode">页面分析模式</param>
        /// <returns>识别出的字符串</returns>
        protected virtual string CrackCodeCore(Bitmap bmp, PageSegMode mode)
        {
            if (_engine == null)
            {
                throw new InvalidOperationException("Tesseract识别引擎 未初始化");
            }
            Tesseract.PageSegMode tmode = mode.CastTo(Tesseract.PageSegMode.Auto);
            using (Page page = _engine.Process(bmp, tmode))
            {
                string text = page.GetText().Replace("\n", "").Replace(" ", "");
                if (mode == PageSegMode.SingleBlockVertText)
                {
                    text = text.ReverseString();
                }
                return text;
            }
        }
    }
}