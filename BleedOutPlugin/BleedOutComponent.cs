using EXILED.Extensions;
using UnityEngine;

namespace BleedOutPlugin
{
    public class BleedOutComponent : MonoBehaviour
    {
        public void Start()
        {
            player = Player.GetPlayer(gameObject);
        }

        public bool IsStopBleeding = false;

        public void Update()
        {
            if (timeout > 0.0f)
            {
                if (Time.deltaTime > timeout)
                {
                    if (gameObject.GetComponent<StopBleedComponent>())
                        Destroy(gameObject.GetComponent<StopBleedComponent>());
                    Destroy(gameObject.GetComponent<BleedOutComponent>());
                }
                else
                {
                    if (!IsStopBleeding)
                        timeout -= Time.deltaTime;
                }
            }
            if (timeLeft > eventTime)
            {
                if (!IsStopBleeding)
                {
                    if (EthernalMessage != string.Empty)
                    {
                        player.ClearBroadcasts();
                        player.Broadcast(10, EthernalMessage, true);
                    }
                    player.playerStats.HurtPlayer(new PlayerStats.HitInfo(damage, player.GetNickname(), damageType, player.GetPlayerId()), gameObject);
                    gameObject.GetComponent<CharacterClassManager>().RpcPlaceBlood(gameObject.transform.position, 2, bloodSize);
                }
                timeLeft = 0.0f;
            }
            timeLeft += Time.deltaTime;
        }

        private float timeLeft = 0.0f;

        public float bloodSize = 1;

        private ReferenceHub player;

        private float eventTime = 1.0f;

        public BleedOutType BleedOutType = BleedOutType.Miner;

        public int damage = 1;

        public DamageTypes.DamageType damageType = DamageTypes.Grenade;

        public float timeout = 60f;

        public string EthernalMessage = string.Empty;
        public void SetSettings(BleedOutType bleedOutType)
        {
            BleedOutType = bleedOutType;

            switch (BleedOutType)
            {
                case BleedOutType.Miner:
                    damage = 1;
                    eventTime = 1.0f;
                    bloodSize = 0.3f;
                    timeout = 10.0f;
                    break;
                case BleedOutType.Normal:
                    damage = 1;
                    eventTime = 0.5f;
                    bloodSize = 0.6f;
                    timeout = 15.0f;
                    break;
                case BleedOutType.Major:
                    damage = 2;
                    eventTime = 0.5f;
                    bloodSize = 0.9f;
                    timeout = 99999.0f;
                    EthernalMessage = "<color=#9b2d30> У вас сильное кровотечение, найдите Аптечку или SCP-500, иначе вам конец </color>";
                    break;
                case BleedOutType.Arterial:
                    damage = 2;
                    eventTime = 0.2f;
                    bloodSize = 1.3f;
                    timeout = 99999.0f;
                    EthernalMessage = "<color=#c10020> У вас артериальное кровотечение. Найдите SCP-500 и быстро, иначе вам конец </color>";
                    break;
            }
        }
    }
}