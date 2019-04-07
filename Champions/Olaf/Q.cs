using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class OlafAxeThrowCast : IGameScript
    {
        public IMinion axe;
        public void OnActivate(IChampion owner)
        {
            
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = new Vector2(spell.X, spell.Y) - current;
            Vector2 trueCoords;

            if (to.Length() > 1651)
            {
                to = Vector2.Normalize(to);
                var range = to * 1651;
                trueCoords = current + range;
            }
            else
            {
                trueCoords = new Vector2(spell.X, spell.Y);
            }

            spell.AddProjectile("OlafAxeThrowDamage",current.X,current.Y, trueCoords.X, trueCoords.Y);
            IMinion m = AddMinion(owner, "TestCube", "TestCube", trueCoords.X, trueCoords.Y);
            var distan = owner.GetDistanceTo(m) * 0.00052f;
            CreateTimer(distan, () =>
             {
                 m.Die(m);
                 axe = AddMinion(owner, "OlafAxe", "OlafAxe", trueCoords.X, trueCoords.Y);
                 axe.Stats.MoveSpeed.FlatBonus -= 325;
                 
                 AddParticle(owner, "olaf_axe_trigger.troy", trueCoords.X, trueCoords.Y, 1);
                 AddParticle(owner, "olaf_axe_trigger_02.troy", trueCoords.X, trueCoords.Y, 1);
             });
            
            CreateTimer(4f, () =>
             {
                 axe.Die(axe);
             });
            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {            
            AddParticleTarget(owner, "olaf_axeThrow_tar.troy", target, 1);
            var ad = owner.Stats.AttackDamage.Total * 1f;
            var damage = 25 + spell.Level * 45 + ad;
            var slowtime = 1.5f;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            var distan = owner.GetDistanceTo(target);
            if (distan >= 400)
            {
                slowtime += (0.125f * (distan - 400) / 75);
            }
            target.Stats.MoveSpeed.PercentBonus -= (25f + spell.Level * 4) / 100f;
            AddBuffHudVisual("Slow", slowtime, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)target,slowtime);
            AddParticleTarget(owner, "olaf_waterLog_Slow.troy", target, 1);
            AddParticleTarget(owner, "olaf_waterLog_debuf.troy", target, 1);
            CreateTimer(slowtime, () =>
            {
                target.Stats.MoveSpeed.PercentBonus += (25f + spell.Level * 4) / 100f;
            });
        }

        public void OnUpdate(double diff)
        {
        }
    }
}

