using UnityEngine;

public class AndriodButtonUi : MonoBehaviour
{
    void Awake()
    {
        #if !UNITY_ANDROID
        Destroy(gameObject);
        #endif
    }
}
