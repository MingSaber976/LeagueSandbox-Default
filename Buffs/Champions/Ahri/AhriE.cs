using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace AhriE
{
    internal class AhriE : IBuffGameScript
    {
        private StatsModifier _statMod;
        private UnitCrowdControl _crowd = new UnitCrowdControl(CrowdControlType.DISARM);
        public void OnActivate(IObjAiBase unit,ISpell spell)
        {
            _statMod = new StatsModifier();
            _statMod.MoveSpeed.PercentBonus -= 65f / 100f;            
            unit.ApplyCrowdControl(_crowd);
            unit.AddStatModifier(_statMod);
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            unit.RemoveStatModifier(_statMod);
            unit.RemoveCrowdControl(_crowd);
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(double diff)
        {
            
        }
    }
}
