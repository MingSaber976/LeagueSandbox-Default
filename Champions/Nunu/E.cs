using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class IceBlast : IGameScript
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
            //spell.AddProjectileTarget("IceBlast", target, false);
            var travel = owner.GetDistanceTo(target) * 0.0001f;
            var ap = owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 40 + spell.Level * 40 + ap;
            CreateTimer(travel,()=>{
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);                
            });
            ((ObjAiBase)target).AddBuffGameScript("NunuESlow", "NunuESlow", spell, 3f, true);
            AddParticleTarget(owner, "Global_Slow.troy", target, 1); //Frozen Mallet Slow
            AddParticleTarget(owner, "Item_TrueIce_Freeze_Slow.troy", target, 1);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            //projectile.SetToRemove();



        }

        public void OnUpdate(double diff)
        {
        }
    }
}
