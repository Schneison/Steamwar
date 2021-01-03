using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public interface IBoardListener
    {
        //public void OnBoardConstruction();

        public void OnObjectConstructed(ObjectBehaviour obj);

        public void OnObjectDeconstructed(ObjectBehaviour obj);
    }
}
