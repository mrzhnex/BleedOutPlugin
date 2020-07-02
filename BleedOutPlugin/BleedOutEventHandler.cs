using EXILED;
using EXILED.Extensions;
using System;

namespace BleedOutPlugin
{
    public class BleedOutEventHandler
    {
        private readonly Random Random = new Random();
        public void OnMedkitUse(UsedMedicalItemEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<BleedOutComponent>())
            {
                switch (ev.Player.gameObject.GetComponent<BleedOutComponent>().BleedOutType)
                {
                    case BleedOutType.Miner:
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#00ff00> У вас остановилось кровотечение </color>", true);
                        UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        break;
                    case BleedOutType.Normal:
                        if (new ItemType[] { ItemType.SCP500, ItemType.Adrenaline, ItemType.Medkit }.Contains(ev.ItemType))
                        {
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "<color=#00ff00> У вас остановилось кровотечение </color>", true);
                            UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        }
                        break;
                    case BleedOutType.Major:
                        if (new ItemType[] { ItemType.SCP500, ItemType.Medkit }.Contains(ev.ItemType))
                        {
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "<color=#00ff00> У вас остановилось кровотечение </color>", true);
                            UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        }
                        break;
                    case BleedOutType.Arterial:
                        if (new ItemType[] { ItemType.SCP500 }.Contains(ev.ItemType))
                        {
                            ev.Player.ClearBroadcasts();
                            ev.Player.Broadcast(10, "<color=#00ff00> У вас остановилось кровотечение </color>", true);
                            UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        }
                        break;
                }
            }
        }

        internal void OnRoundStart()
        {
            Global.CanUseCommands = true;
        }

        internal void OnWaitingForPlayers()
        {
            Global.CanUseCommands = false;
            try
            {
                Global.IsFullRp = Plugin.Config.GetBool("IsFullRp");
            }
            catch (Exception ex)
            {
                Log.Info("Catch an exception while getting boolean value from config file: " + ex.Message);
                Global.IsFullRp = false;
            }
        }

        internal void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<CooldownStopBleed>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<CooldownStopBleed>());
            }
            if (ev.Player.gameObject.GetComponent<StopBleedComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<StopBleedComponent>());
            }
            if (ev.Player.gameObject.GetComponent<BleedOutComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
            }
        }

        public void OnShoot(ref ShootEvent ev)
        {
            if (ev.Shooter.gameObject.GetComponent<StopBleedComponent>())
            {
                ev.Shooter.ClearBroadcasts();
                ev.Shooter.Broadcast(10, "<color=#dc314c>Остановить кровотечение не удалось - вы произвели выстрел</color>", true);
                UnityEngine.Object.Destroy(ev.Shooter.gameObject.GetComponent<StopBleedComponent>());
            }
            if (ev.Target != null && ev.Target.GetComponent<StopBleedComponent>())
            {
                if (Player.GetPlayer(ev.Target) != null)
                {
                    Player.GetPlayer(ev.Target).ClearBroadcasts();
                    Player.GetPlayer(ev.Target).Broadcast(10, "<color=#dc314c>Остановить кровотечение не удалось - в вас попала пуля</color>", true);
                }
                UnityEngine.Object.Destroy(ev.Target.GetComponent<StopBleedComponent>());
            }
            if (ev.Shooter.gameObject.GetComponent<SuicideComponent>())
            {
                if (ev.Shooter.inventory.curItem != ItemType.None)
                {
                    DamageTypes.DamageType damageTypes = DamageTypes.Com15;
                    switch (ev.Shooter.GetCurrentItem().id)
                    {
                        case ItemType.GunCOM15:
                            damageTypes = DamageTypes.Com15;
                            break;
                        case ItemType.GunUSP:
                            damageTypes = DamageTypes.Usp;
                            break;
                        case ItemType.GunMP7:
                            damageTypes = DamageTypes.Mp7;
                            break;
                        case ItemType.GunProject90:
                            damageTypes = DamageTypes.P90;
                            break;
                        case ItemType.GunE11SR:
                            damageTypes = DamageTypes.E11StandardRifle;
                            break;
                        case ItemType.GunLogicer:
                            damageTypes = DamageTypes.Logicer;
                            break;
                    }
                    ev.Shooter.playerStats.HurtPlayer(new PlayerStats.HitInfo(99999, ev.Shooter.nicknameSync.Network_myNickSync, damageTypes, ev.Shooter.GetPlayerId()), ev.Shooter.gameObject);
                    ev.Shooter.ClearBroadcasts();
                    ev.Shooter.Broadcast(10, "<color=#dc314c>Вы покончили жизнь самоубийством</color>", true);
                }
            }
        }

        public void OnConsoleCommand(ConsoleCommandEvent ev)
        {
            if (!Global.CanUseCommands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            if (ev.Command.ToLower() == "stopbleed")
            {
                if (!new RoleType[] { RoleType.ClassD, RoleType.Scientist, RoleType.NtfCommander, RoleType.NtfLieutenant, RoleType.NtfScientist, RoleType.NtfCadet, RoleType.FacilityGuard, RoleType.ChaosInsurgency, RoleType.Scp049 }.Contains(ev.Player.GetRole()))
                {
                    ev.ReturnMessage = "Вы не можете использовать эту команду";
                    return;
                }
                if (ev.Player.gameObject.GetComponent<BleedOutComponent>() == null)
                {
                    ev.ReturnMessage = "У вас нет кровотечения";
                    return;
                }
                if (ev.Player.gameObject.GetComponent<StopBleedComponent>())
                {
                    ev.ReturnMessage = "Вы уже пытаетесь остановить кровотечение";
                    return;
                }
                if (ev.Player.gameObject.GetComponent<CooldownStopBleed>())
                {
                    ev.ReturnMessage = "Подождите еще какое то время";
                    return;
                }
                ev.Player.gameObject.GetComponent<BleedOutComponent>().IsStopBleeding = true;
                ev.Player.gameObject.AddComponent<StopBleedComponent>();
                ev.Player.gameObject.GetComponent<StopBleedComponent>().BleedOutType = ev.Player.gameObject.GetComponent<BleedOutComponent>().BleedOutType;
                ev.ReturnMessage = "Вы пытаетесь остановить кровотечение...";
                return;
            }
            if (ev.Command.ToLower() == "suicide" && Global.IsFullRp)
            {
                if (ev.Player.gameObject.GetComponent<SuicideComponent>())
                {
                    UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<SuicideComponent>());
                    ev.ReturnMessage = "Вы передумали заканчивать жизнь самоубийством.";
                    return;
                }
                else
                {
                    ev.Player.gameObject.AddComponent<SuicideComponent>();
                    ev.ReturnMessage = "Вы решили покончить жизнь самоубийством. Ваш следующий выстрел убьет вас.";
                    return;
                }
            }
        }

        public void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<CooldownStopBleed>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<CooldownStopBleed>());
            }
            if (ev.Player.gameObject.GetComponent<StopBleedComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<StopBleedComponent>());
            }
            if (ev.Player.gameObject.GetComponent<BleedOutComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
            }
            if (ev.Player.gameObject.GetComponent<SuicideComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<SuicideComponent>());
            }
        }

        public void OnPlayerHurt(ref PlayerHurtEvent ev)
        {
            if (ev.Player.GetTeam() == Team.SCP && (ev.Player.GetRole() != RoleType.Scp93953 || ev.Player.GetRole() != RoleType.Scp93989 || ev.Player.GetRole() != RoleType.Scp049))
            {
                return;
            }

            if (GetArterial(ev.DamageType))
            {
                if (ev.Player.gameObject.GetComponent<BleedOutComponent>() != null)
                {
                    if (new BleedOutType[] { BleedOutType.Miner, BleedOutType.Normal, BleedOutType.Major, BleedOutType.Arterial }.Contains(ev.Player.gameObject.GetComponent<BleedOutComponent>().BleedOutType))
                    {
                        UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        ev.Player.gameObject.AddComponent<BleedOutComponent>();
                        ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Arterial);
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#c10020> У вас артериальное кровотечение. Найдите SCP-500 и быстро, иначе вам конец </color>", true);
                    }
                }
                else
                {
                    ev.Player.gameObject.AddComponent<BleedOutComponent>();
                    ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Arterial);
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(10, "<color=#c10020> У вас артериальное кровотечение. Найдите SCP-500 и быстро, иначе вам конец </color>", true);
                }
            }
            if (GetMajor(ev.DamageType))
            {
                if (ev.Player.gameObject.GetComponent<BleedOutComponent>() != null)
                {
                    if (new BleedOutType[] { BleedOutType.Miner, BleedOutType.Normal, BleedOutType.Major }.Contains(ev.Player.gameObject.GetComponent<BleedOutComponent>().BleedOutType))
                    {
                        UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        ev.Player.gameObject.AddComponent<BleedOutComponent>();
                        ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Major);
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#9b2d30> У вас сильное кровотечение, найдите Аптечку или SCP-500, иначе вам конец </color>", true);
                    }
                }
                else
                {
                    ev.Player.gameObject.AddComponent<BleedOutComponent>();
                    ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Major);
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(10, "<color=#9b2d30> У вас сильное кровотечение, найдите Аптечку или SCP-500, иначе вам конец </color>", true);
                }
            }
            if (GetNormal(ev.DamageType))
            {
                if (ev.Player.gameObject.GetComponent<BleedOutComponent>() != null)
                {
                    if (new BleedOutType[] { BleedOutType.Miner, BleedOutType.Normal }.Contains(ev.Player.gameObject.GetComponent<BleedOutComponent>().BleedOutType))
                    {
                        UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        ev.Player.gameObject.AddComponent<BleedOutComponent>();
                        ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Normal);
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#8b0000> Вы истекаете кровью. Найдите Аптечку, Шприц, SCP-500. </color>", true);
                    }
                }
                else
                {
                    ev.Player.gameObject.AddComponent<BleedOutComponent>();
                    ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Normal);
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(10, "<color=#8b0000> Вы истекаете кровью. Найдите Аптечку, Шприц, SCP-500. </color>", true);
                }
            }
            if (GetMiner(ev.DamageType))
            {
                if (ev.Player.gameObject.GetComponent<BleedOutComponent>() != null)
                {
                    if (new BleedOutType[] { BleedOutType.Miner }.Contains(ev.Player.gameObject.GetComponent<BleedOutComponent>().BleedOutType))
                    {
                        UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
                        ev.Player.gameObject.AddComponent<BleedOutComponent>();
                        ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Miner);
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#dc314c> Вы слегка истекаете кровью. Используйте любой медикамент, либо подождите, пока кровь сама остановится </color>", true);
                    }
                }
                else
                {
                    ev.Player.gameObject.AddComponent<BleedOutComponent>();
                    ev.Player.gameObject.GetComponent<BleedOutComponent>().SetSettings(BleedOutType.Miner);
                    ev.Player.ClearBroadcasts();
                    ev.Player.Broadcast(10, "<color=#dc314c> Вы слегка истекаете кровью. Используйте любой медикамент, либо подождите, пока кровь сама остановится </color>", true);
                }
            }
        }

        private string GetUsageBo()
        {
            return " Usage: bo <id/nickname> <seconds> <damage> <bloodsize>";
        }

        public void OnRemoveAdminCommand(ref RACommandEvent ev)
        {
            string[] args = ev.Command.Split(' ');

            if (args[0] != "bo")
                return;
            if (args.Length != 5)
            {
                ev.Sender.RAMessage("Out of args." + GetUsageBo());
                return;
            }

            ReferenceHub referenceHub = Player.GetPlayer(args[1]);
            if (referenceHub == null)
            {
                ev.Sender.RAMessage("Player not found" + GetUsageBo());
                return;
            }
            if (!int.TryParse(args[2], out int seconds) || !int.TryParse(args[3], out int damage) || !float.TryParse(args[4], out float size) || seconds < 0 || damage < 0 || size < 0.0f)
            {
                ev.Sender.RAMessage("Only positive numbers" + GetUsageBo());
                return;
            }
            if (referenceHub.GetComponent<BleedOutComponent>())
                UnityEngine.Object.Destroy(referenceHub.GetComponent<BleedOutComponent>());
            BleedOutComponent bleedOutComponent = referenceHub.gameObject.AddComponent<BleedOutComponent>();
            bleedOutComponent.damageType = DamageTypes.E11StandardRifle;
            bleedOutComponent.damage = damage;
            bleedOutComponent.timeout = seconds;
            bleedOutComponent.bloodSize = size;

            ev.Sender.RAMessage("Add bleedoutcomponent to " + referenceHub.nicknameSync.Network_myNickSync);
            return;
        }

        public bool GetMiner(DamageTypes.DamageType damageType)
        {
            if (damageType ==  DamageTypes.Com15 && Random.NextDouble() < 0.4)
                return true;
            if (damageType == DamageTypes.Scp0492 && Random.NextDouble() < 0.1)
                return true;
            return false;
        }

        public bool GetNormal(DamageTypes.DamageType damageType)
        {
            if ((damageType == DamageTypes.P90 || damageType == DamageTypes.Mp7) && Random.NextDouble() < 0.4)
                return true;
            if (damageType == DamageTypes.Falldown && Random.NextDouble() < 0.15)
                return true;
            if ((damageType == DamageTypes.Logicer || damageType == DamageTypes.E11StandardRifle || damageType == DamageTypes.Usp) && Random.NextDouble() < 0.5)
                return true;
            return false;
        }

        public bool GetMajor(DamageTypes.DamageType damageType)
        {
            if (damageType == DamageTypes.Usp && Random.NextDouble() < 0.05)
                return true;
            if ((damageType == DamageTypes.P90 || damageType == DamageTypes.Mp7) && Random.NextDouble() < 0.1)
                return true;
            if (damageType == DamageTypes.E11StandardRifle && Random.NextDouble() < 0.2)
                return true;
            if (damageType == DamageTypes.Logicer && Random.NextDouble() < 0.3)
                return true;
            if (damageType == DamageTypes.Scp939 && Random.NextDouble() < 0.8)
                return true;
            return false;
        }

        public bool GetArterial(DamageTypes.DamageType damageType)
        {
            if ((damageType == DamageTypes.P90 || damageType == DamageTypes.Mp7 || damageType == DamageTypes.Usp || damageType == DamageTypes.Com15) && Random.NextDouble() < 0.01)
                return true;
            if (damageType == DamageTypes.E11StandardRifle && Random.NextDouble() < 0.02)
                return true;
            if (damageType == DamageTypes.E11StandardRifle && Random.NextDouble() < 0.03)
                return true;
            if (damageType == DamageTypes.Scp939 && Random.NextDouble() < 0.3)
                return true;
            return false;
        }

        public void OnSetRole(SetClassEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<CooldownStopBleed>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<CooldownStopBleed>());
            }
            if (ev.Player.gameObject.GetComponent<StopBleedComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<StopBleedComponent>());
            }
            if (ev.Player.gameObject.GetComponent<BleedOutComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<BleedOutComponent>());
            }
            if (ev.Player.gameObject.GetComponent<SuicideComponent>())
            {
                UnityEngine.Object.Destroy(ev.Player.gameObject.GetComponent<SuicideComponent>());
            }
        }
    }

    public enum BleedOutType
    {
        Miner, Normal, Major, Arterial
    }
}