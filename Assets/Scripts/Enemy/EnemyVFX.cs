using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VFX
{
    public string name;
    public GameObject particleVFX;
}
public class EnemyVFX : MonoBehaviour
{
    public VFX[] VisualEffects;
    [SerializeField] private Dictionary<string, GameObject> VFXList = new();

    private void Start()
    {
        foreach (var vfx in VisualEffects)
        {
            VFXList.Add(vfx.name, vfx.particleVFX);
        }    
    }

    public void TriggerVFX(string VFXName)
    {
        VFXList.TryGetValue(VFXName, out var VFX);
        VFX?.GetComponent<ParticleSystem>()?.Play();
    }
    public void TriggerVFX(string VFXName, float angle)
    {
        VFXList.TryGetValue(VFXName, out var VFX);
        VFX.transform.rotation = Quaternion.Euler(0,0,angle);
        VFX?.GetComponent<ParticleSystem>()?.Play();
    }
}
