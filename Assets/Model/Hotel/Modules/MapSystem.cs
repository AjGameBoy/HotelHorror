using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Model.Hotel.Modules
{
    public class MapSystem: MonoBehaviour
    {
        [SerializeField] private Tilemap[] tilemaps;
        [SerializeField] private GameObject hallModule;
        [SerializeField] private Transform spawnObject;

        public void BuildMap()
        {
            List<Vector2> halls = new();

            foreach (var tilemap in tilemaps)
            {
                BoundsInt bounds = tilemap.cellBounds;

                for (int x = bounds.xMin; x < bounds.xMax; x++)
                {
                    for (int y = bounds.yMin; y < bounds.yMax; y++)
                    {
                        Vector3Int localPlace = new Vector3Int(x, y, bounds.zMin);
                        if(tilemap.HasTile(localPlace))
                        {
                            TileBase tile = tilemap.GetTile(localPlace);
                            switch (tile.name)
                            {
                                case "red30x30":
                                    halls.Add(new Vector2(x, y));
                                    break;
                                case "spawn":
                                    spawnObject.position = new Vector3(x, 0, y);
                                    break;
                            }
                        }
                    }
                }
            }

            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
                DestroyImmediate(child.gameObject);

            foreach (Vector2 hall in halls)
            {
                GameObject h = Instantiate(hallModule, transform);
                h.transform.localPosition = new Vector3(hall.x, 0, hall.y);
            }
            
            HallModule[] modules = GetComponentsInChildren<HallModule>();
            foreach (var m in modules)
                m.UpdateModule(modules);
        }
    }
}