using System.Collections;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public IEnumerator DestroyEffect(GameObject effectToDestroy, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Destroy(effectToDestroy);
        }
    }
}
