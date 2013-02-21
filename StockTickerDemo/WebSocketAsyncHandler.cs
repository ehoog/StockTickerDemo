// --------------------------------------------------------------------------------------
// <copyright file="WebSocketAsyncHandler.cs" company="Erik Hoogendoorn">
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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.WebSockets;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Demo code")]
    public abstract class WebSocketAsyncHandler : HttpTaskAsyncHandler
    {
        /// <summary>
        /// Gets a value indicating whether this handler can be reused for another request.
        /// Should return false in case your Managed Handler cannot be reused for another request, or true otherwise.
        /// Usually this would be false in case you have some state information preserved per request.
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: <see cref="http://go.microsoft.com/?linkid=8101007" />
        /// </summary>
        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private WebSocket Socket { get; set; }

        public override async Task ProcessRequestAsync(HttpContext httpContext)
        {
            await Task.Run(() =>
            {
                if (httpContext.IsWebSocketRequest)
                {
                    httpContext.AcceptWebSocketRequest(async delegate(AspNetWebSocketContext context)
                    {
                        this.Socket = context.WebSocket;

                        while (this.Socket != null || this.Socket.State != WebSocketState.Closed)
                        {
                            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                            WebSocketReceiveResult receiveResult = await this.Socket.ReceiveAsync(buffer, CancellationToken.None);

                            try
                            {
                                switch (receiveResult.MessageType)
                                {
                                    case WebSocketMessageType.Text:
                                        string message = Encoding.UTF8.GetString(buffer.Array, 0, receiveResult.Count);
                                        this.OnMessageReceived(message);
                                        break;
                                    case WebSocketMessageType.Binary:
                                        this.OnMessageReceived(buffer.Array);
                                        break;
                                    case WebSocketMessageType.Close:
                                        this.OnClosing(true, receiveResult.CloseStatusDescription);
                                        break;
                                }

                                switch (this.Socket.State)
                                {
                                    case WebSocketState.Connecting:
                                        this.OnConnecting();
                                        break;
                                    case WebSocketState.Open:
                                        this.OnOpen();
                                        break;
                                    case WebSocketState.CloseSent:
                                        this.OnClosing(false, string.Empty);
                                        break;
                                    case WebSocketState.CloseReceived:
                                        this.OnClosing(true, string.Empty);
                                        break;
                                    case WebSocketState.Closed:
                                        this.OnClosed();
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                this.OnError(ex);
                            }
                        }
                    });
                }
            });
        }

        protected virtual void OnConnecting()
        {
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnMessageReceived(string message)
        {
        }

        protected virtual void OnMessageReceived(byte[] bytes)
        {
        }

        protected virtual void OnClosing(bool isClientRequest, string message)
        {
        }

        protected virtual void OnClosed()
        {
        }

        protected virtual void OnError(Exception ex)
        {
        }

        [DebuggerStepThrough]
        protected async Task SendMessageAsync(byte[] message)
        {
            await this.SendMessageAsync(message, WebSocketMessageType.Binary);
        }

        [DebuggerStepThrough]
        protected async Task SendMessageAsync(string message)
        {
            await this.SendMessageAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text);
        }

        private async Task SendMessageAsync(byte[] message, WebSocketMessageType messageType)
        {
            await this.Socket.SendAsync(
                new ArraySegment<byte>(message),
                messageType,
                true,
                CancellationToken.None);
        }
    }
}
