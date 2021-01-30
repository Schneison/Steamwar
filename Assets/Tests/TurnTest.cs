using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamwar.Turns;
using Steamwar;
using Steamwar.Factions;
using Steamwar.Sectors;

public class TurnTest
{
    [Test]
    public void TurnInstance()
    {
        TurnStatInstance instance = TurnStatInstance.CreateInstance(TestConstants.session);
        int count = 0;
        while (!instance.IsEnd())
        {
            Assert.True(count < 100, "Failed turns");
            if (instance.IsPlayer())
            {
                instance.AllowPlayerTransition();
            }
            instance.Update();
            count++;
        }
        
    }
}
