using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class OlafRagnarok : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnProc);
            // wait for API (OnAutoAttack)
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        private void OnProc(IAttackableUnit target, bool isCrit)
        {
            //_spell.LowerCooldown(1);
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {            
            
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var p1 = AddParticleTarget(owner, "olaf_ragnorok_buff.troy", owner);
            var p2 = AddParticleTarget(owner, "olaf_ragnorok_enraged.troy", owner);
            var p3 = AddParticleTarget(owner, "olaf_ragnorok_shield_01.troy", owner);
            var p4 = AddParticleTarget(owner, "olaf_ragnorok_shield_02.troy", owner);
            owner.AddBuffGameScript("OlafR", "OlafR", spell, 6f, true);
            //for (float i = 0.00f; i <= 6.0f; i += 0.01f)
            //{
            //    ((ObjAiBase)owner).ClearAllCrowdControl();
            //}
            CreateTimer(6f, () =>
            {
                RemoveParticle(p1);
                RemoveParticle(p2);
                RemoveParticle(p3);
                RemoveParticle(p4);
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

