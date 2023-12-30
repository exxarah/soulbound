using System.Collections;
using UnityEngine;

namespace Core.Unity.Utils
{
    public class TimedDestroyComponent : MonoBehaviour
    {
        public void Initialise(float secondsTilDeath)
        {
            StartCoroutine(KillAfterWait(secondsTilDeath));
        }

        private IEnumerator KillAfterWait(float waitSeconds)
        {
            yield return new WaitForSeconds(waitSeconds);
            
            Destroy(this.gameObject);
        }
    }
}