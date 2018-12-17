using Gun.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Gun.AspNet.Core
{
    public class GunHandler : WebSocketHandler
    {
        private readonly IDuplicateManager _duplicateManager;
        private Graph _graph = new Graph();

        public GunHandler(WebSocketConnectionManager webSocketConnectionManager, IDuplicateManager duplicateManager) : base(webSocketConnectionManager)
        {
            this._duplicateManager = duplicateManager;
        }
        
        public async override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var reader = new StreamReader(new MemoryStream(buffer), Encoding.Default);

            var msg = new JsonSerializer().Deserialize<GunMessage>(new JsonTextReader(reader));

            if (_duplicateManager.Check(msg.Key)) return;

            _duplicateManager.Track(msg.Key);

            if(msg is PutMessage)
            {
                var change = _graph.Mix(((PutMessage)msg).PutChanges);
            }
            
            if(msg is GetMessage)
            {
                var ack = _graph.Get(((GetMessage)msg).Get);
                if (ack != null){
                    var response = new PutMessage() 
                    { 
                        Key =_duplicateManager.Track(DuplicateManager.Random()),
                        At = msg.Key,
                        PutChanges = ack
                    };
                     await this.SendMessageToAllAsync(JsonConvert.SerializeObject(response));
                }

            }
            await this.SendMessageToAllAsync(JsonConvert.SerializeObject(msg));
          
        }
    }
}
