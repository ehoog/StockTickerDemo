// --------------------------------------------------------------------------------------
// <copyright file="IndexTicker.cs" company="Erik Hoogendoorn">
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
    using System.Timers;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Demo code")]
    public class IndexTicker
    {
        private const int TimerInterval = 5000;

        private static object lockField = new object();

        private static IndexTicker defaultInstanceField;

        private IndexTicker()
        {
        }

        public event EventHandler<IndexTickerEventArgs> Update;

        public static IndexTicker DefaultInstance
        {
            get
            {
                lock (IndexTicker.lockField)
                {
                    if (IndexTicker.defaultInstanceField == null)
                    {
                        IndexTicker.defaultInstanceField = new IndexTicker();
                        IndexTicker.defaultInstanceField.Initialize();
                    }
                }

                return IndexTicker.defaultInstanceField;
            }
        }

        private static Timer Timer { get; set; }

        public void Start()
        {
            lock (IndexTicker.lockField)
            {
                if (!IndexTicker.Timer.Enabled)
                {
                    IndexTicker.Timer.Start();
                }
            }
        }

        public void Stop()
        {
            lock (IndexTicker.lockField)
            {
                if (IndexTicker.Timer.Enabled)
                {
                    IndexTicker.Timer.Stop();
                }
            }
        }

        protected virtual void OnUpdate(int quoteIndex)
        {
            if (this.Update != null)
            {
                this.Update(
                    this,
                    new IndexTickerEventArgs()
                    {
                        QuoteIndex = quoteIndex
                    });
            }
        }

        private void Initialize()
        {
            IndexTicker.Timer = new Timer(IndexTicker.TimerInterval);
            IndexTicker.Timer.Elapsed += this.Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var randomizer = new Random();
            var quote_index = randomizer.Next(0, 4);
            this.OnUpdate(quote_index);
        }
    }
}