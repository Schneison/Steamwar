using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects.Elements
{
    public class StateMachine<O> where O: ObjectBehaviour
    {

        public class Shape
        {
            public Object animation;
        }

        public class Container
        {
            public Instance instance;
            public Shape currentState;
        }

        public class Instance {
            public Shape[] shapes;


        }

    }
}
