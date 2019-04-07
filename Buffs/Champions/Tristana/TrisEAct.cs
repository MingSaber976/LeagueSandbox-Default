using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace TrisEAct
{
    internal class TrisEAct : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _statMod = new StatsModifier();
            _statMod.HealthRegeneration.PercentBonus -= 50f / 100f;
            _visualBuff = AddBuffHudVisual("DetonatingShot", 5.0f, 1, BuffType.COMBAT_DEHANCER, unit,5.0f);
            _visualBuff = AddBuffHudVisual("GrievousWound", 5.0f, 1, BuffType.COMBAT_DEHANCER, unit,5.0f);
            //Immunity to slowness not added
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            //RemoveBuffHudVisual(_visualBuff);
            //RemoveBuffHudVisual(_visualBuff);
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
