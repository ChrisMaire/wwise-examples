﻿using System.Collections;
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

            if(switchesByTexture.ContainsKey(floorText.name))
            {
                AkSoundEngine.SetSwitch("Material", switchesByTexture[floorText.name], gameObject);
                AkSoundEngine.PostEvent("sfx_footstep", gameObject);
            } else
            {
                Debug.LogWarning("No audio switch found for text " + floorText.name);
            }

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
