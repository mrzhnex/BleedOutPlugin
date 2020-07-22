using EXILED;

namespace BleedOutPlugin
{
    public class MainSettings : Plugin
    {
        public SetEvents SetEvents { get; set; }
        public override string getName => nameof(BleedOutPlugin);

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.UsedMedicalItemEvent += SetEvents.OnMedkitUse;
            Events.SetClassEvent += SetEvents.OnSetRole;
            Events.PlayerHurtEvent += SetEvents.OnPlayerHurt;
            Events.RemoteAdminCommandEvent += SetEvents.OnRemoveAdminCommand;
            Events.PlayerDeathEvent += SetEvents.OnPlayerDeath;
            Events.ConsoleCommandEvent += SetEvents.OnConsoleCommand;
            Events.ShootEvent += SetEvents.OnShoot;
            Events.PlayerSpawnEvent += SetEvents.OnPlayerSpawn;
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.UsedMedicalItemEvent -= SetEvents.OnMedkitUse;
            Events.SetClassEvent -= SetEvents.OnSetRole;
            Events.PlayerHurtEvent -= SetEvents.OnPlayerHurt;
            Events.RemoteAdminCommandEvent -= SetEvents.OnRemoveAdminCommand;
            Events.PlayerDeathEvent -= SetEvents.OnPlayerDeath;
            Events.ConsoleCommandEvent -= SetEvents.OnConsoleCommand;
            Events.ShootEvent -= SetEvents.OnShoot;
            Events.PlayerSpawnEvent -= SetEvents.OnPlayerSpawn;
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}