using System;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class BloodBoil : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        private void ReduceCooldown(IAttackableUnit unit, bool isCrit)
        {
        //No Cooldown reduction on the other skills yet
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {            
            //No increased durations on kills and assists yet
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            // var p0 = AddParticleTarget(owner, "BloodBoil3_cas.troy", owner, 1);
            //CreateTimer()
            //var p1 = AddParticleTarget(owner, "BloodBoil_buf.troy", owner, 1);            
            IChampion allygetbuff = null;
            if (target == owner)
            {
                owner.AddBuffGameScript("BloodBoil", "BloodBoil", spell, 12.0f, true);
                foreach (var value in GetChampionsInRange(owner, 700, true))
                {
                    if (value != owner && value.Team == owner.Team && value != null)
                    {
                        allygetbuff = value;
                    }
                }
                if (allygetbuff != null)
                {
                    allygetbuff.AddBuffGameScript("BloodBoil", "BloodBoil", spell, 12.0f, true);
                }
            }
            if (target != owner && target.Team == owner.Team)
            {
                //var p201 = AddParticleTarget(owner, "BloodBoil_buf.troy", target, 1);
                // var p202 = AddParticleTarget(owner, "BloodBoil_tar.troy", target, 1);
                owner.AddBuffGameScript("BloodBoil", "BloodBoil", spell, 12.0f, true);
                ((ObjAiBase)target).AddBuffGameScript("BloodBoil", "BloodBoil", spell, 12.0f, true);
                //CreateTimer(12.0f, () =>
                //{
                    //    RemoveParticle(p201);
                    //    RemoveParticle(p202);
                //});
            }
            else { return; }
            CreateTimer(12.0f, () =>
            {
                //RemoveParticle(p1);
            });
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {            
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
