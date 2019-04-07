using System.Linq;
using System.Numerics;
using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class BusterShot : IGameScript
    {
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
            spell.AddProjectileTarget("BusterShot", target, false);            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(target.X, target.Y) - current);
            var range = to * (400 + spell.Level * 200f);
            var trueCoords = current + range;

            var ap = owner.Stats.AbilityPower.Total * 1.5f;
            var damage = 200 + spell.Level * 100 + ap;
            
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, "tristana_bustershot_tar.troy", target, 1);
            AddParticleTarget(owner, "tristana_busterShot_unit_impact.troy", target, 0.25f);
            projectile.SetToRemove();

            foreach (var value in GetUnitsInRange(target, 200, true)
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                DashToLocation((ObjAiBase)value, trueCoords.X, trueCoords.Y, 1100, true);
                CreateTimer(0.35f, () =>
                {
                    CancelDash((ObjAiBase)value);
                });
            }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
