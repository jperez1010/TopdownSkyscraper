using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    private GameObject player;
    public float maxDistance;
    public Material solidMaterial;
    public Material seeThroughMaterial;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            SetVisibility(wall, -1);
        }
        (List<GameObject>, List<float>) wallValues = CastRays();
        List<GameObject> hiddenWalls = wallValues.Item1;
        List<float> distances = wallValues.Item2;

        for (int i = 0; i < hiddenWalls.Count; i++)
        {
            SetVisibility(hiddenWalls[i], distances[i]);
        }
    }

    void SetVisibility(GameObject wall, float distance)
    {
        if (distance < 0)
        {
            wall.GetComponent<MeshRenderer>().enabled = true;
            wall.GetComponent<MeshRenderer>().material = solidMaterial;
        }
        else if (distance < maxDistance)
        {
            wall.GetComponent<MeshRenderer>().enabled = true;
            wall.GetComponent<MeshRenderer>().material = seeThroughMaterial;
            wall.GetComponent<MeshRenderer>().material.color = new Color(seeThroughMaterial.color.r, seeThroughMaterial.color.g, seeThroughMaterial.color.b, 1 - (distance / maxDistance));
        }
        else
        {
            wall.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    (List<GameObject>, List<float>) CastRays()
    {
        List<GameObject> walls = new List<GameObject>();
        List<float> distances = new List<float>();

        float N = 50;

        for (int n = 0; n <= N; n++)
        {
            float theta = (Mathf.PI/2) * (n / N);

            Vector3 direction = Mathf.Cos(theta) * Vector3.back + Mathf.Sin(theta) * Vector3.right;

            RaycastHit[] hits = Physics.RaycastAll(player.transform.position + Vector3.up, direction, 30f, LayerMask.GetMask("Obstacle"));

            Debug.DrawRay(player.transform.position + Vector3.up, direction * maxDistance);

            foreach (RaycastHit hit in hits)
            {
                GameObject wall = hit.transform.gameObject;
                if (wall.tag == "Wall")
                {
                    if (walls.Contains(wall))
                    {
                        if (hit.distance < distances[walls.FindIndex(x => x.Equals(wall))])
                        {
                            distances[walls.FindIndex(x => x.Equals(wall))] = hit.distance;
                        }
                    }
                    else
                    {
                        walls.Add(wall);
                        distances.Add(hit.distance);
                    }
                }
            }
        }
        return (walls, distances);

    }
}
