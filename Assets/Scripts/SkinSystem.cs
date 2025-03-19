using System.Collections.Generic;
using UnityEngine;

public class SkinSystem : MonoBehaviour
{

    public int skinIndex = 0;

    private List<Material> colormaps;
    public List<GameObject> flags = new();
    public List<GameObject> sails = new();

    void Start()
    {
        colormaps = new List<Material>(Resources.LoadAll<Material>("BoatSkins"));

        UpdateSkin();
    }

    void UpdateSkin()
    {

        skinIndex = PlayerPrefs.GetInt("skinIndex", 0);
        if (skinIndex >= colormaps.Count)
        {
            skinIndex = 0;
        }

        for (int i = 0; i < flags.Count; i++)
        {
            flags[i].GetComponent<MeshRenderer>().material = colormaps[skinIndex];
        }
        for (int i = 0; i < sails.Count; i++)
        {
            sails[i].GetComponent<MeshRenderer>().material = colormaps[skinIndex];
        }
        gameObject.GetComponent<MeshRenderer>().material = colormaps[skinIndex];
    }
}
