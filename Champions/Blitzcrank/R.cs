using System.Linq;
using GameServerCore;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class StaticField : IGameScript
    {
       // private ISpell _spell;
        public void OnActivate(IChampion owner)
        {
            //if (_spell.CurrentCooldown == 0)
            //{
            //    AddParticleTarget(owner, "StaticField_ready.troy", owner, 1);
            //}
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("Spell4", owner);

            var p1 = AddParticleTarget(owner, "StaticField_nova.troy", owner, 1);
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 125 + spell.Level * 125 + ap;
            foreach (var enemyTarget in GetUnitsInRange(owner, 600, true)
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                if (!(enemyTarget is IBaseTurret) || !(enemyTarget is ILaneTurret))
                {
                    enemyTarget.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL,
                      false);
                    ((ObjAiBase)enemyTarget).AddBuffGameScript("Silence", "Silence", spell, 0.5f, true);
                    AddBuffHudVisual("Silence", 0.5f, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)enemyTarget, 0.5f);
                    var p2 = AddParticleTarget(owner, "Global_Silence.troy", (ObjAiBase)enemyTarget, 1, "Head");
                    AddParticleTarget(owner, "StaticField_tar.troy", enemyTarget, 1);
                    CreateTimer(0.5f, () =>
                     {
                         RemoveParticle(p2);
                     });
                }                
            }
            CreateTimer(1f, () => 
            {
                RemoveParticle(p1);
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
