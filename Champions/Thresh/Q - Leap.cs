using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class ThreshQLeap : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
            
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var targets = ThreshQ.targets;
            AddParticleTarget(owner, "Thresh_Q_Pull_Sound.troy", owner);
            owner.RemoveSpell(0);
            owner.SetSpell("ThreshQ", 0, true);
            
            DashToUnit(owner, targets, 900, false);
            owner.Stats.MoveSpeed.FlatBonus += 900;
            CreateTimer(0.75f, () =>
            {
                owner.Stats.MoveSpeed.FlatBonus -= 900;
                CancelDash(owner);
            });
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {

        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }
    }
}
