using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class ThreshE : IGameScript
    {
        
        public void OnActivate(IChampion owner)
        {
            
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 420;
            var trueCoords = current + range;
            var trueCoordsfrom = current - range;
            spell.AddProjectile("ThreshEMissile1", trueCoordsfrom.X, trueCoordsfrom.Y, trueCoords.X, trueCoords.Y, false);
            var p1 = AddParticle(owner, "Thresh_E_Warn_green.troy", owner.X, owner.Y, 1);

        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var current = new Vector2(target.X, target.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 100;
            var trueCoords = current + range;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 35f + spell.Level * 30f + ap * 0.4f;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            DashToLocation((ObjAiBase)target, trueCoords.X, trueCoords.Y, 300, false, null, 100, 0, 0, 0.3f);
            target.Stats.Size.FlatBonus += 1;
            CancelDash((ObjAiBase)target);
            
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
