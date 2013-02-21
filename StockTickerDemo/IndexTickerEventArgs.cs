// --------------------------------------------------------------------------------------
// <copyright file="IndexTickerEventArgs.cs" company="Erik Hoogendoorn">
//    Copyright (c) by Erik Hoogendoorn, All rights reserved. http://erikhoogendoorn.com/
//
//    This source is subject to the Microsoft Permissive License.
//    Please see the README.md file for more information.
//    All other rights reserved.
//
//    THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//    EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//    OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------
namespace StockTickerDemo
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Demo code")]
    public class IndexTickerEventArgs : EventArgs
    {
        public int QuoteIndex { get; set; }
    }
}