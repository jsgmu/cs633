using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public class EventBus
    {
        private static List<object> listeners;

        static EventBus()
        {
            listeners = new List<object>();
        }

        public static void Subscribe(object listener)
        {
            listeners.Add(listener);
        }

        public static void Publish<T>(T eventData)
        {
            foreach (var handler in listeners)
            {
                (handler as IHandle<T>)?.Handle(eventData);
            }
        }
    }
}
