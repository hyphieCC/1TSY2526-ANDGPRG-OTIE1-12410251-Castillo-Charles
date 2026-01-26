using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    int hp;

    public float speed;
    public float destroyZ;

    void Awake() => hp = maxHP;
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);

        if (transform.position.z < destroyZ)
            Destroy(gameObject);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) 
        { 
            GameManagerUI.Instance.AddScore(1); 
            Destroy(gameObject); 
        }
    }
}