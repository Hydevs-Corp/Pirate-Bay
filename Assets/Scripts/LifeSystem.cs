using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    private int maxHeart = 6;
    public int startHeart = 6;
    public int currentHealth;
    private int maxHealth;
    private int healthPerHeart = 2;

    public Image[] healthImages;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    void Start()
    {
        currentHealth = startHeart * healthPerHeart;
        maxHealth = maxHeart * healthPerHeart;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < maxHeart; i++)
        {
            if (i < startHeart)
            {

                healthImages[i].enabled = true;
                int heartHealth = Mathf.Clamp(currentHealth - (i * healthPerHeart), 0, healthPerHeart);

                if (heartHealth == healthPerHeart)
                {
                    healthImages[i].sprite = fullHeart;
                }
                else if (heartHealth > 0)
                {
                    healthImages[i].sprite = halfHeart;
                }
                else
                {
                    healthImages[i].sprite = emptyHeart;
                }
            }
            else
            {
                healthImages[i].enabled = false;
            }
        }
    }
}


