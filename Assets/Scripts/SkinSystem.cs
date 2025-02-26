using System.Collections.Generic;
using UnityEngine;

public class SkinSystem : MonoBehaviour
{

    public int skinIndex = 0;

    public List<Material> skins = new();
    public List<GameObject> flags = new();
    public List<GameObject> sails = new();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skinIndex = 0;
            UpdateSkin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skinIndex = 1;
            UpdateSkin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skinIndex = 2;
            UpdateSkin();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skinIndex = 3;
            UpdateSkin();
        }
    }

    void UpdateSkin()
    {
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
