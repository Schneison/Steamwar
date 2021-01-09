using Steamwar.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.UI
{
    public interface IJudgeListener
    {
        public void OnJudgeChange(ObjectContainer container);

        public void OnJudgeClear(ObjectContainer container);
    }
}
