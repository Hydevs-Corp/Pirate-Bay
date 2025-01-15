using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LifeSystem : MonoBehaviour
{
    private int maxHeart = 6;
    public int startHeart = 6;
    public int currentHealth;
    private int maxHealth;
    private int healthPerheart = 2;

    public Image[] healthImages;
    public Sprite[] healthSprites;

    void Start()
    {
        currentHealth = startHeart * healthPerheart;
        maxHealth = maxHeart * healthPerheart;
        checkHealth();

    }

    void checkHealth()
    {
        for (int i = 0; i < maxHeart; i++)
        {
            if (startHeart <= i)
            {
                healthImages[i].enabled = false;
            }
            else
            {
                healthImages[i].enabled = true;
            }
        }
    }
}