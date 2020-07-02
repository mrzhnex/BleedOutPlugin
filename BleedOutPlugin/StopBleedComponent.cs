using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace BleedOutPlugin
{
    public class StopBleedComponent : MonoBehaviour
    {
        private float Timer = 0.0f;
        private readonly float TimeIsUp = 0.5f;
        private float Progress = 0.0f;
        private Vector3 StartPosition = Vector3.zero;
        private ReferenceHub PlayerHub;
        private bool IsStandart = false;
        public BleedOutType BleedOutType = BleedOutType.Miner;
        private readonly System.Random Random = new System.Random();

        public void Start()
        {
            PlayerHub = Player.GetPlayer(gameObject);
            StartPosition = gameObject.transform.position;
            if (PlayerHub.GetRole() == RoleType.ClassD)
                IsStandart = true;
        }

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > TimeIsUp)
            {
                Timer = 0.0f;
                Progress += TimeIsUp;
                if (CheckForStop())
                {
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, "<color=#dc314c>Остановить кровотечение не удалось - вы сдвинулись с места</color>", true);
                    Destroy(gameObject.GetComponent<StopBleedComponent>());
                }
            }
            if (Progress > 10.0f)
            {
                if (GetChanceToStop())
                {
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, SuccessMessage, true);
                    if (gameObject.GetComponent<BleedOutComponent>())
                        Destroy(gameObject.GetComponent<BleedOutComponent>());
                }
                else if (GetChanceToDoNotStop())
                {
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, FailedMessage, true);
                }
                else if (GetChanceToDoNotStopWithDebuff())
                {
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, UberFailedMessage, true);
                    PlayerHub.playerStats.HurtPlayer(new PlayerStats.HitInfo(10, PlayerHub.nicknameSync.Network_myNickSync, DamageTypes.Grenade, PlayerHub.GetPlayerId()), gameObject);
                }
                else
                {
                    PlayerHub.ClearBroadcasts();
                    PlayerHub.Broadcast(10, FailedMessage, true);
                }
                Destroy(gameObject.GetComponent<StopBleedComponent>());
            }
        }

        public void OnDestroy()
        {
            if (gameObject.GetComponent<BleedOutComponent>())
                gameObject.GetComponent<BleedOutComponent>().IsStopBleeding = false;
            gameObject.AddComponent<CooldownStopBleed>();
        }

        private bool GetChanceToStop()
        {
            switch (BleedOutType)
            {
                case BleedOutType.Miner:
                    if (IsStandart && Random.NextDouble() < 0.7)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 1.0)
                        return true;
                    break;
                case BleedOutType.Normal:
                    if (IsStandart && Random.NextDouble() < 0.25)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.6)
                        return true;
                    break;
                case BleedOutType.Major:
                    if (IsStandart && Random.NextDouble() < 0.05)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.1)
                        return true;
                    break;
                case BleedOutType.Arterial:
                    if (IsStandart && Random.NextDouble() < 0.01)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.05)
                        return true;
                    break;
            }
            return false;
        }

        private bool GetChanceToDoNotStop()
        {
            switch (BleedOutType)
            {
                case BleedOutType.Miner:
                    if (IsStandart && Random.NextDouble() < 0.2)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.0)
                        return true;
                    break;
                case BleedOutType.Normal:
                    if (IsStandart && Random.NextDouble() < 0.5)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.35)
                        return true;
                    break;
                case BleedOutType.Major:
                    if (IsStandart && Random.NextDouble() < 0.5)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.7)
                        return true;
                    break;
                case BleedOutType.Arterial:
                    if (IsStandart && Random.NextDouble() < 0.5)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.5)
                        return true;
                    break;
            }
            return false;
        }

        private bool GetChanceToDoNotStopWithDebuff()
        {
            switch (BleedOutType)
            {
                case BleedOutType.Miner:
                    if (IsStandart && Random.NextDouble() < 0.1)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.0)
                        return true;
                    break;
                case BleedOutType.Normal:
                    if (IsStandart && Random.NextDouble() < 0.25)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.05)
                        return true;
                    break;
                case BleedOutType.Major:
                    if (IsStandart && Random.NextDouble() < 0.45)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.2)
                        return true;
                    break;
                case BleedOutType.Arterial:
                    if (IsStandart && Random.NextDouble() < 0.49)
                        return true;
                    if (!IsStandart && Random.NextDouble() < 0.4)
                        return true;
                    break;
            }
            return false;
        }

        private bool CheckForStop()
        {
            if (Vector3.Distance(StartPosition, gameObject.transform.position) > 1.0f)
                return true;
            return false;
        }

        private readonly string SuccessMessage = "<color=#00ff00> Вы остановили кровотечение </color>";
        private readonly string FailedMessage = "<color=#dc314c> Вы попытались остановить кровотечение, но у вас не вышло</color>";
        private readonly string UberFailedMessage = "<color=#8b0000> Вы попытались остановить кровотечение, но у вас не вышло и вы нанесли себе урон</color>";
    }
}