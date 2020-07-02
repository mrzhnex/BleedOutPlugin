using EXILED;

namespace BleedOutPlugin
{
    public class BleedOutPlugin : Plugin
    {
        BleedOutEventHandler bleedOutEventHandler;
        public override string getName => "BleedOutPlugin";

        public override void OnEnable()
        {
            bleedOutEventHandler = new BleedOutEventHandler();
            Events.UsedMedicalItemEvent += bleedOutEventHandler.OnMedkitUse;
            Events.SetClassEvent += bleedOutEventHandler.OnSetRole;
            Events.PlayerHurtEvent += bleedOutEventHandler.OnPlayerHurt;
            Events.RemoteAdminCommandEvent += bleedOutEventHandler.OnRemoveAdminCommand;
            Events.PlayerDeathEvent += bleedOutEventHandler.OnPlayerDeath;
            Events.ConsoleCommandEvent += bleedOutEventHandler.OnConsoleCommand;
            Events.ShootEvent += bleedOutEventHandler.OnShoot;
            Events.PlayerSpawnEvent += bleedOutEventHandler.OnPlayerSpawn;
            Events.WaitingForPlayersEvent += bleedOutEventHandler.OnWaitingForPlayers;
            Events.RoundStartEvent += bleedOutEventHandler.OnRoundStart;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.UsedMedicalItemEvent -= bleedOutEventHandler.OnMedkitUse;
            Events.SetClassEvent -= bleedOutEventHandler.OnSetRole;
            Events.PlayerHurtEvent -= bleedOutEventHandler.OnPlayerHurt;
            Events.RemoteAdminCommandEvent -= bleedOutEventHandler.OnRemoveAdminCommand;
            Events.PlayerDeathEvent -= bleedOutEventHandler.OnPlayerDeath;
            Events.ConsoleCommandEvent -= bleedOutEventHandler.OnConsoleCommand;
            Events.ShootEvent -= bleedOutEventHandler.OnShoot;
            Events.PlayerSpawnEvent -= bleedOutEventHandler.OnPlayerSpawn;
            Events.WaitingForPlayersEvent -= bleedOutEventHandler.OnWaitingForPlayers;
            Events.RoundStartEvent -= bleedOutEventHandler.OnRoundStart;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}