using GameServerCore;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;

namespace Spells
{
    public class HallucinateFull : IGameScript
    {
        public IChampion SpellOwner { get; set; }
        public IMinion Pet { get; set; }
        public float cloneTimeAlive = 0.00f;

        public void OnActivate(IChampion owner)
        {
            SpellOwner = owner;
        }

        private void OnUnitHit(IAttackableUnit target, bool isCrit)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            //owner.AddBuffGameScript("Invisibility", "Invisibility", spell, cloneduration, true);
            //owner.SetVisibleByTeam(owner.Team, false);
            //owner.SetVisibleByTeam(target.Team, false);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var cloneduration = 18.0f;
            var stats = owner.ChampStats; // Proof of concept
            var items = owner.Inventory; // Proof of concept
            var sightrange = owner.VisionRadius; // Proof of concept
            var aoeradius = 250; // Might be wrong
            // MAGIC DAMAGE: 300 / 450 / 600 (+100% AP)
            var ap = owner.Stats.AbilityPower.Total * 1.0f;
            var aoedamage = new[] { 300, 450, 600 }[spell.Level - 1] + ap;
            //Champion shacoClone = new Champion("Shaco", _netManager.GetNewNetID(), _netManager.GetNewNetID(), new LeagueSandbox.GameServer.Logic.Content.RuneCollection(), null, _netManager.GetNewNetID());
            //shacoClone.setPosition(owner.X + 10, owner.Y + 5);
            //IAttackableUnit clone = new IAttackableUnit("Shaco", new Stats(), 40, owner.X, owner.Y);
            //ApiFunctionManager.SetGameObjectVisibility(clone, true);
            IMinion Shaclone = AddMinion(owner, "Shaco", "Shaco", owner.X, owner.Y);
            Pet = Shaclone;
            owner.AddBuffGameScript("HallucinateGuide", "HallucinateGuide", spell, cloneduration, true);
            AddParticle(owner, "HallucinatePoof.troy", owner.X, owner.Y);
            try
            {
                if (!Pet.IsDead)
                {
                    for (cloneTimeAlive = 0.0f; cloneTimeAlive < cloneduration; cloneTimeAlive += 0.066f) // Once per 2 ticks?
                    {
                        CreateTimer(cloneTimeAlive, () => {
                            if (!Pet.IsDead)
                            {
                                return;
                            }
                            else
                            {
                                AddParticle(owner, "Hallucinate_nova.troy", Pet.X, Pet.Y);
                                var units = GetUnitsInRange(Pet, aoeradius, true);
                                foreach (var unit in units)
                                {
                                    if (owner.Team != unit.Team && unit is IAttackableUnit && !(unit is IBaseTurret) && !(unit is IObjAnimatedBuilding))
                                    {
                                        unit.TakeDamage(owner, aoedamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                                    }
                                }
                            }
                        });
                    }
                    CreateTimer(cloneduration, () =>
                    {
                        if (!Pet.IsDead)
                        {
                            AddParticle(owner, "Hallucinate_nova.troy", Pet.X, Pet.Y);
                            var units = GetUnitsInRange(Pet, aoeradius, true);
                            foreach (var unit in units)
                            {
                                if (owner.Team != unit.Team && unit is IAttackableUnit && !(unit is IBaseTurret) && !(unit is IObjAnimatedBuilding))
                                {
                                    unit.TakeDamage(owner, aoedamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                                }
                            }
                            Pet.Die(Pet);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }

        public void OnData()
        {
        }

    }
}
