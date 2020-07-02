using UnityEngine;

namespace BleedOutPlugin
{
    public class CooldownStopBleed : MonoBehaviour
    {
        private float Timer = 0.0f;
        public void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > 10.0f)
            {
                Destroy(gameObject.GetComponent<CooldownStopBleed>());
            }
        }
    }
}