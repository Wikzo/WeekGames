using UnityEngine;
using System.Collections;

public class MoveTerrain : MonoBehaviour
{
    float Speed = 12; //12

    Vector3 StartPos = new Vector3(-955, 0, -5061);
    bool go;

    // Use this for initialization
    void Start()
    {
        Screen.showCursor = false;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);

        if (transform.position.z >= 2 + float.Epsilon && !go)
        {
            go = true;
            GameObject terrain;
            TerrainData _terraindata = new TerrainData();
            TerrainData t = gameObject.GetComponent<Terrain>().terrainData;
            terrain = Terrain.CreateTerrainGameObject(t);



            GameObject ingameTerrainGameObject = (GameObject)Instantiate(terrain, StartPos, Quaternion.identity);
            //GenerateHeights(gameObject.GetComponent<Terrain>(), 2f);
            ingameTerrainGameObject.AddComponent<MoveTerrain>();

            Destroy(terrain);

            Destroy(gameObject);

        }

        /*if (transform.position.z > 20 + float.Epsilon)
            Destroy(gameObject);*/
    }
}