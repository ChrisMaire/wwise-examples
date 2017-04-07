using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  System.Linq;

[System.Serializable]
public class TextureSwitchPair
{
    public string TextureName;
    public string SwitchName;
}

public class CharacterSoundPlayer : MonoBehaviour
{
    public List<TextureSwitchPair> FootstepSwitches;
    private Dictionary<string, string> switchesByTexture;

    private float vol;
    private TerrainData terrainData;
    private Vector3 terrainPos;

    void Start()
    {
        //create a dictionary mapping texture names to switch names
        switchesByTexture = FootstepSwitches.ToDictionary(t => t.TextureName, s => s.SwitchName);

        terrainData = Terrain.activeTerrain.terrainData;
        terrainPos =  Terrain.activeTerrain.GetPosition();

        vol = GetAndPrintVolume();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Equals) || Input.GetKeyUp(KeyCode.Minus))
        {
            if (Input.GetKeyUp(KeyCode.Equals))
            {
                vol += 10;
            }
            else
            {
                vol -= 10;
            }
            AkSoundEngine.SetRTPCValue("Volume", vol + 10, null);

            GetAndPrintVolume();
        }
    }

    public float GetAndPrintVolume()
    {
        float v = 0.5f;
        int vtype = (int)RTPCValue_type.RTPCValue_Global;
        uint pid = (uint)RTPCValue_type.RTPCValue_PlayingID;
        AkSoundEngine.GetRTPCValue("Volume", gameObject, pid, out v, ref vtype);

        Debug.Log("volume is now " + v);

        return v;
    }

    /// <summary>
    /// Set the correct switch based on the floor, and play a footstep audio event.
    /// </summary>
    /// <param name="animEvent"></param>
    public void PlayFootstep(AnimationEvent animEvent)
    {
        //Debug.Log(animEvent.animatorClipInfo.weight);

        if (animEvent.animatorClipInfo.weight < 0.5)
        {
            Texture floorText = getTerrainTextureAt(transform.position);

            AkSoundEngine.SetSwitch("Material", switchesByTexture[floorText.name], gameObject);
            AkSoundEngine.PostEvent("sfx_footstep", gameObject);
        }
    }

    //based on http://answers.unity3d.com/questions/14998/how-can-i-perform-some-action-based-on-the-terrain.html
    public Texture getTerrainTextureAt(Vector3 position)
    {
        int AX = (int)(((transform.position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int AZ = (int)(((transform.position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
        float[,,] alphaMap = Terrain.activeTerrain.terrainData.GetAlphamaps(AX, AZ, 1, 1);

        Texture retval = terrainData.splatPrototypes[0].texture;
        int idx = 0;
        for (int i = 0; i < terrainData.splatPrototypes.Length; i++)
        {
            if (alphaMap[0, 0, i] > alphaMap[0, 0, idx])
            {
                idx = i;
                retval = terrainData.splatPrototypes[i].texture;
            }
        }

        return retval;
    }
}
