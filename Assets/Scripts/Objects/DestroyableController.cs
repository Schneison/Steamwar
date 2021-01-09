using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steamwar.Objects
{
    public class DestroyableController : SteamBehaviour
    {

        public virtual void Damage(ObjectContainer attackedUnit)
        {
            float damage = GetDamage(attackedUnit);
            float health = GetHealth();
            if (damage > 0)
            {
                health -= damage;
            }
            if (health <= 0)
            {
                OnDeath(attackedUnit);
                //attackedUnit.OnKill(this);
            }
        }

        public virtual float GetHealth()
        {
            //return Data.Health;
            return 0;
        }

        public virtual float GetDamage(ObjectContainer attackedUnit)
        {
            //return Type.damage;
            return 0;
        }

        public virtual void OnDeath(ObjectContainer killedBy)
        {

        }

        public virtual void OnKill(ObjectContainer killedUnit)
        {

        }
    }
}
