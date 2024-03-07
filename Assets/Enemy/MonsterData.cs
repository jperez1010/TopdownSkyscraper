using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FOW;

public class MonsterData : MonoBehaviour
{
    public Material material;
    public List<EnemyStats> stats = new List<EnemyStats>();
    public int attack;
    public int health;
    public int speed;

    public ItemObject itemObject;
    public GameObject spawnedItem;

    public void Awake()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].attribute == EnemyAttributes.Attack)
            {
                attack = stats[i].stat;
            }
            else if (stats[i].attribute == EnemyAttributes.Health)
            {
                health = stats[i].stat;
            }
            else if (stats[i].attribute == EnemyAttributes.Speed)
            {
                speed = stats[i].stat;
            }
        }
    }
    public void ActivateIdle()
    {
        // Create a new RGBA color using the Color constructor and store it in a variable
        Color customColor = new Color(1f, 0f, 0f, 1.0f);

        // Call SetColor using the shader property name "_Color" and setting the color to the custom color you created
        material.color = customColor;
    }

    public void ActivateChase()
    {
        // Create a new RGBA color using the Color constructor and store it in a variable
        Color customColor = new Color(1f, 1f, 1f, 1.0f);

        // Call SetColor using the shader property name "_Color" and setting the color to the custom color you created
        material.color = customColor;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            for (int i = 0; i < stats.Count; i++)
            {
                if (stats[i].attribute == EnemyAttributes.Health)
                {
                    Debug.Log("true");
                    health -= other.GetComponent<BulletObject>().dmg;
                }
            }
            if (health <= 0)
            {
                GameObject newObject = Instantiate(spawnedItem);
                GameObject player = this.gameObject;
                newObject.transform.position = player.transform.position + new Vector3(player.transform.forward.x * 2, -player.transform.position.y, player.transform.forward.z * 2);
                newObject.GetComponent<GroundItemHandler>().groundItem.item = itemObject;
                newObject.GetComponent<Billboard>().camera = Camera.main;
                newObject.GetComponentInChildren<SpriteRenderer>().sprite = itemObject.uiDisplay;
                Destroy(gameObject);
            } 
        }
    }
}

public enum EnemyAttributes
{
    Attack,
    Health,
    Speed
}

[System.Serializable]
public struct EnemyStats
{
    public EnemyAttributes attribute;
    public int stat;
}
