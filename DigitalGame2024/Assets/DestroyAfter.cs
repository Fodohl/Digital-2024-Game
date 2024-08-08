using System.Collections;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float timeToDestroy = 1f;
    private void Awake(){
        StartCoroutine(Destroy());
    }
    private IEnumerator Destroy(){
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
