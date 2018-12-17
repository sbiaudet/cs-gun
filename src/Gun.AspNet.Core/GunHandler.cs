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
        private readonly IDictionary<string, Node> _graph = new Dictionary<string, Node>();

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

            if(msg.PutChanges.Count != 0)
            {
                var change = HAM.Mix(msg.PutChanges, _graph);
            }
            
            await this.SendMessageToAllAsync(JsonConvert.SerializeObject(msg));
          
        }
    }
}
