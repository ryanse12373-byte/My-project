using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    
    public Terrain terrain;

    public int radius = 5;
    public float maxRange;
    public GameObject plant;

    void Start()
    {
            if (terrain != null)
            {
                terrain.terrainData = Instantiate(terrain.terrainData);
            }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            TryTill();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Plant(plant);
        }
    }

    #region Labourer

    void TryTill()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.distance > maxRange) return;
            Terrain t = hit.collider.GetComponent<Terrain>();
            if (t != null)
            {
                TillTerrain(hit.point, t);
            }
        }
    }

    void TillTerrain(Vector3 worldPos, Terrain terrain)
    {
        TerrainData data = terrain.terrainData;

        Vector3 terrainPos = worldPos - terrain.transform.position;

        int mapX = Mathf.RoundToInt(
            terrainPos.x / data.size.x * data.alphamapWidth
        );

        int mapY = Mathf.RoundToInt(
            terrainPos.z / data.size.z * data.alphamapHeight
        );

        Paint(mapX, mapY, data);
    }

    void Paint(int x, int y, TerrainData data)
    {
        int size = 2;

        float[,,] maps = data.GetAlphamaps(x, y, size, size);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                maps[i, j, 0] = 0f; // retire herbe
                maps[i, j, 1] = 1f; // ajoute terre labourée
            }
        }

        data.SetAlphamaps(x, y, maps);
    }
    bool IsTilled(Vector3 worldPos)
{
    TerrainData data = terrain.terrainData;

    Vector3 localPos = worldPos - terrain.transform.position;

    int x = Mathf.RoundToInt(localPos.x / data.size.x * data.alphamapWidth);
    int y = Mathf.RoundToInt(localPos.z / data.size.z * data.alphamapHeight);

    float[,,] map = data.GetAlphamaps(x, y, 1, 1);

    return map[0, 0, 1] > 0.5f;
}

    #endregion 

    void Plant(GameObject plant)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.distance > 5) return;
            if (IsTilled(hit.point))
            {
                GameObject p = Instantiate(plant, hit.point, Quaternion.identity);
                p.transform.localScale = Vector3.zero;
            }
        }
    }

}

