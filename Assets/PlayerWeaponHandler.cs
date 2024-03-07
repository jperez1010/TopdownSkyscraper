using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnProjectile()
    {
        Player player = gameObject.GetComponent<Player>();
        Object bullet = Instantiate(Resources.Load("Bullet"), player.bulletTransform.position, transform.rotation);
        GameObject bulletGO = (GameObject)bullet;
        int dmg = 0;
        for (int j = 0; j < player.attributes.Length; j++)
        {
            if (player.attributes[j].type == Attributes.Strength)
            {
                dmg += player.attributes[j].value.ModifiedValue;
            }
        }
        bulletGO.GetComponent<BulletObject>().dmg = dmg;
        bulletGO.GetComponent<BulletObject>().direction = transform.forward;
    }
}
