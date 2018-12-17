using System.Linq;

namespace Gun.Core
{
    public static class GunNodeExtensions
    {
        public static Node Clone(this Node node, string key)
        {
            var tmp = new Node();
            tmp.Metadata = new Metadata() { Soul = node.Metadata.Soul };
            tmp.Metadata.HAMState = new HAMState(node.Metadata.HAMState.Where(s => s.Key == key).ToDictionary(t=> t.Key, t=> t.Value));
            tmp.Properties[key] = node.Properties[key];
            return tmp;
        }
    }
}