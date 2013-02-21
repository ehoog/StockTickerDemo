// --------------------------------------------------------------------------------------
// <copyright file="StockTickerWebSocketAsyncHandler.cs" company="Erik Hoogendoorn">
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
    using System.Globalization;
    using System.Threading.Tasks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Demo code")]
    public class StockTickerWebSocketAsyncHandler : WebSocketAsyncHandler
    {
        private static double[] FacebookQuotes
        {
            get
            {
                return new double[] { 28.38, 28.45, 27.53, 29.11, 29.23 };
            }
        }

        private static double[] GoogleQuotes
        {
            get
            {
                return new double[] { 758, 760, 764, 765, 769 };
            }
        }

        private double[] ActiveQuote { get; set; }

        protected override void OnMessageReceived(string message)
        {
            // Assignment prevents warning "Because this call is not awaited...Consider applying the 'await' operator
            // This is intentional => fire and forget
            Task task;

            if (string.Equals(message, "STOP"))
            {
                IndexTicker.DefaultInstance.Update -= this.IndexTicker_Update;
                this.ActiveQuote = null;

                task = this.SendMessageAsync("Stopped");
            }
            else
            {
                if (this.ActiveQuote == null)
                {
                    IndexTicker.DefaultInstance.Update += this.IndexTicker_Update;
                }

                if (string.Equals(message, "FB"))
                {
                    this.ActiveQuote = FacebookQuotes;
                }
                else
                {
                    this.ActiveQuote = GoogleQuotes;
                }

                task = this.SendMessageAsync("Started");
            }
        }

        protected override void OnError(Exception ex)
        {
            // Assignment prevents warning "Because this call is not awaited...Consider applying the 'await' operator
            // This is intentional => fire and forget
            var task = this.SendMessageAsync(string.Format("Something exceptional happened: {0}", ex.Message));
        }

        private void IndexTicker_Update(object sender, IndexTickerEventArgs e)
        {
            // Assignment prevents warning "Because this call is not awaited...Consider applying the 'await' operator
            // This is intentional => fire and forget
            var task = this.SendMessageAsync(this.ActiveQuote[e.QuoteIndex].ToString());
        }
    }
}