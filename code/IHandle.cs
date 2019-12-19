using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis
{
    public interface IHandle<in T>
    {
        void Handle(T eventData);
    }
}
