using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;

    int hp;
    bool dead = false;

    void Awake()
    {
        hp = maxHP;
    }

    void Start()
    {
        GameManagerUI.Instance?.SetHP(hp);
    }

    public void TakeDamage(int amount)
    {
        if (dead) return;

        hp -= amount;
        hp = Mathf.Max(hp, 0);

        GameManagerUI.Instance?.SetHP(hp);
        Debug.Log("Player HP: " + hp);

        if (hp <= 0)
        {
            dead = true;

            AudioManager.Instance?.PlaySfx(
                AudioManager.Instance.playerDeathClip
            );

            GameManager.Instance?.GameOver();

            gameObject.SetActive(false);
        }
    }
}