using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;


public class SkinMenu : MonoBehaviour
{

    public List<Material> colormaps = new();
    public GameObject ButtonPrefab;
    public GameObject boatPrefab;

    private int localSkinIndex;

    private List<GameObject> Buttons = new();

    private Color selectedColor = new Color(39f / 255f, 131f / 255f, 70f / 255f);

    void Start()
    {
        localSkinIndex = PlayerPrefs.GetInt("skinIndex", 0);
        SetSkin(localSkinIndex);

        if (boatPrefab == null) return;

        foreach (Material colormap in colormaps)
        {
            GameObject boat = Instantiate(boatPrefab);
            boat.transform.SetParent(transform);
            boat.transform.localScale = new Vector3(12f, 12f, 12f);
            boat.transform.position = this.transform.position;
            float offset = 8 * (colormaps.IndexOf(colormap) - (colormaps.Count - 1) / 2.0f);
            boat.transform.position += new Vector3(offset, 3f, -10f);

            boat.transform.Rotate(new Vector3(0f, 116f + 15f, 347f + 25f));
            boat.GetComponent<MeshRenderer>().material = colormap;
            MeshRenderer[] meshRenderers = boat.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = colormap;
            }

            IddleController iddleController = boat.AddComponent<IddleController>();
            iddleController.rotate = true;
            iddleController.force = 0.1f + 0.01f * colormaps.IndexOf(colormap);

            boat.name = colormap.name;


            GameObject button = Instantiate(ButtonPrefab);
            Buttons.Add(button);
            if (colormaps.IndexOf(colormap) == localSkinIndex)
            {
                button.GetComponent<UnityEngine.UI.Image>().color = selectedColor;
            }
            button.transform.SetParent(transform);
            button.transform.localScale = Vector3.one;
            button.transform.position = this.transform.position;
            button.transform.position += new Vector3(offset, 0f, 0f);
            button.transform.Rotate(new Vector3(36f, 0, 0));

            // onclick
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                SetSkin(colormaps.IndexOf(colormap));
                // change color of button
                foreach (GameObject b in Buttons)
                {
                    b.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                }
                //rgb(39, 131, 70)
                button.GetComponent<UnityEngine.UI.Image>().color = selectedColor;
            });
        }
    }

    void SetSkin(int newSkinIndex)
    {
        PlayerPrefs.SetInt("skinIndex", newSkinIndex);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}
