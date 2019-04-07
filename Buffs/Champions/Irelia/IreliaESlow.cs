using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace IreliaESlow
{
    internal class IreliaESlow : IBuffGameScript
    {
        private StatsModifier _statMod;
        public void OnActivate(IObjAiBase unit,ISpell spell)
        {
            _statMod = new StatsModifier();
            _statMod.MoveSpeed.PercentBonus -= 60f / 100f;
            unit.AddStatModifier(_statMod);
        }

        public void OnDeactivate(IObjAiBase unit)
        {            
            unit.RemoveStatModifier(_statMod);
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(double diff)
        {
            
        }
    }
}
