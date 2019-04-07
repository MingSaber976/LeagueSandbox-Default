using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class DesperatePower : IGameScript
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
            var time = 4 + spell.Level * 1f;
            owner.AddBuffGameScript("RyzeR", "RyzeR", spell, time, true);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
           
        }

        public void OnUpdate(double diff)
        {
        }
    }
}

