using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class DetonatingShot : IGameScript
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
            //spell.AddProjectileTarget("DetonatingShot", target, false);
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 35 + spell.Level * 45 + ap;
            var preDamage = damage / 5;
            var p1 = AddParticleTarget(owner, "tristana_explosiveShot_cas.troy", owner,1);
            var p4 = AddParticleTarget(owner, "global_grievousWound_tar.troy", target, 1,"head");
                        
            for (var i = 0.0f; i <= 4.0f; i+=1.0f)
            {
                CreateTimer(i, () => { Bleeding(owner, spell, target); });                
            }
            ((ObjAiBase)target).AddBuffGameScript("TrisEAct","TrisEAct",spell,5.0f,true);
            CreateTimer(0.1f, () => 
            {
                RemoveParticle(p4);
            });
            var p2 = AddParticleTarget(owner,"tristana_explosiveShot_tar.troy",target,1);
            var p3 = AddParticleTarget(owner, "tristana_explosiveShot_unit_tar.troy", target, 1);
            
            

            //projectile.SetToRemove();
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            
        }

        public void Bleeding(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 35 + spell.Level * 45 + ap;
            var preDamage = damage / 5;
            target.TakeDamage(owner, preDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            //     if (target.IsDead == true)
            //     {
            //         foreach (var value in GetChampionsInRange(target, 300, true).Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            //         {
            //             var ap = owner.Stats.AbilityPower.Total * 0.25f;
            //             var damage = 25f + spell.Level * 25f + ap;
            //             value.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PASSIVE, false);
            //         }
            //     }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
