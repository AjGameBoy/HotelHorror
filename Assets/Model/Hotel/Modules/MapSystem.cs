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
            List<Vector2Int> halls = new();
            List<Vector2Int> doors = new();

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
                                    halls.Add(new Vector2Int(x, y));
                                    break;
                                case "spawn":
                                    spawnObject.position = new Vector3(x, 0, y);
                                    break;
                                case "door":
                                    doors.Add(new Vector2Int(x, y));
                                    break;
                            }
                        }
                    }
                }
            }
            
            

            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
                DestroyImmediate(child.gameObject);

            foreach (Vector2Int hall in halls)
            {
                GameObject h = Instantiate(hallModule, transform);
                h.transform.localPosition = new Vector3(hall.x, 0, hall.y);
                List<DoorWay> halldoors = new();
                for (int x = -2; x <= 2; x++)
                {
                    for (int y = -2; y <= 2; y++)
                    {
                        if ((Mathf.Abs(y) != 2 && Mathf.Abs(x) != 2) || (Mathf.Abs(x) == 2 && Mathf.Abs(y) == 2))
                            continue;
                        if (doors.Contains(new Vector2Int(hall.x + x, hall.y + y)))
                        {
                            //Debug.Log($"Hall at {hall.x}, {hall.y} has a door at {x}, {y}");
                            CompassDirection dir;
                            if (Mathf.Abs(y) == 2)
                            {
                                if (y > 0)
                                    dir = CompassDirection.South;
                                else
                                    dir = CompassDirection.North;
                            }
                            else
                            {
                                if (x > 0)
                                    dir = CompassDirection.West;
                                else
                                    dir = CompassDirection.East;
                            }
                            halldoors.Add(new DoorWay()
                            {
                                direction = dir,
                                sort = (dir == CompassDirection.North || dir == CompassDirection.South ? x : y) + 1
                            });
                        }
                    }
                }

                h.GetComponent<HallModule>().doors = halldoors.ToArray();
            }
            
            HallModule[] modules = GetComponentsInChildren<HallModule>();
            foreach (var m in modules)
                m.UpdateModule(modules);
        }
    }
}