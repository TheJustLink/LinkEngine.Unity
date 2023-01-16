using UnityEngine;

class Test : MonoBehaviour
{
    private void Awake() => Debug.Log("AWAKE");

    private void Reset() => Debug.Log("RESET");
    private void OnEnable() => Debug.Log("ENABLE");
    private void OnDisable() => Debug.Log("DISABLE");
    private void OnDestroy() => Debug.Log("DESTROY");
    
    private void Start() => Debug.Log("START");
    
    private void FixedUpdate() => Debug.Log("FIXED UPDATE");
    private void Update() => Debug.Log("UPDATE");
    private void LateUpdate() => Debug.Log("LATE UPDATE");
}