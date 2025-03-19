using System.Collections.Generic;
using UnityEngine;

public class SkinSystem : MonoBehaviour
{

    public int skinIndex = 0;

    public List<Material> skins = new();
    public List<GameObject> flags = new();
    public List<GameObject> sails = new();

    void Start()
    {
        UpdateSkin();
    }

    void UpdateSkin()
    {

        skinIndex = PlayerPrefs.GetInt("skinIndex", 0);

        for (int i = 0; i < flags.Count; i++)
        {
            flags[i].GetComponent<MeshRenderer>().material = skins[skinIndex];
        }
        for (int i = 0; i < sails.Count; i++)
        {
            sails[i].GetComponent<MeshRenderer>().material = skins[skinIndex];
        }
        gameObject.GetComponent<MeshRenderer>().material = skins[skinIndex];
    }
}
