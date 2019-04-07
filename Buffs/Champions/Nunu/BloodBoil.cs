using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace BloodBoil
{
    internal class BloodBoil : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _statMod = new StatsModifier();
            _statMod.MoveSpeed.PercentBonus = _statMod.MoveSpeed.PercentBonus + (7f + ownerSpell.Level * 1) / 100f;
            _statMod.AttackSpeed.PercentBonus = _statMod.AttackSpeed.PercentBonus + (20f + ownerSpell.Level * 5) / 100f;
            unit.AddStatModifier(_statMod);
            _visualBuff = AddBuffHudVisual("BloodBoil", 12.0f, 1, BuffType.COMBAT_ENCHANCER, unit);
            //Immunity to slowness not added
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_visualBuff);
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
