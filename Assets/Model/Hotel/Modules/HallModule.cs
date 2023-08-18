using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Hotel.Modules
{
    public class HallModule : MonoBehaviour
    {
        private float moduleSize = 3f;
        [SerializeField] private GameObject carpetPrefab;
        [SerializeField] private GameObject carpetEndPrefab;

        [SerializeField] private Transform carpetContainer;
        public DoorWay[] doors;

        public void UpdateModule(HallModule[] modules)
        {
            List<CompassDirection> neighbors = new();
            foreach (var m in modules)
            {
                if (m == this)
                    continue;

                var pos = m.transform.position - transform.position;

                if (pos == new Vector3(moduleSize, 0, 0))
                {
                    neighbors.Add(CompassDirection.West);
                }
                else if (pos == new Vector3(-moduleSize, 0, 0))
                {
                    neighbors.Add(CompassDirection.East);
                }
                else if (pos == new Vector3(0, 0, moduleSize))
                {
                    neighbors.Add(CompassDirection.South);
                }
                else if (pos == new Vector3(0, 0, -moduleSize))
                {
                    neighbors.Add(CompassDirection.North);
                }
            }

            bool hasLightAlready = false;

            HashSet<CompassDirection> closedWalls = new();

            foreach (var x in GetComponentsInChildren<WallModule>())
            {
                if (doors.Count(d => d.direction == x.side) > 0)
                {
                    x.isOpen = false;
                    x.hasLight = false;
                    x.isDoor = doors.Count(d => d.direction == x.side && d.sort == x.sort) > 0;
                    closedWalls.Add(x.side);
                }
                else if (neighbors.Contains(x.side))
                {
                    x.isOpen = true;
                }
                else
                {
                    closedWalls.Add(x.side);
                    x.isOpen = false;
                    x.isDoor = false;
                    if (!hasLightAlready && x.sort == 1)
                    {
                        x.hasLight = true;
                        hasLightAlready = true;
                    }
                    else
                        x.hasLight = false;
                }

                x.UpdateWall();
            }

            var tempList = carpetContainer.Cast<Transform>().ToList();
            foreach (var child in tempList)
                DestroyImmediate(child.gameObject);

            bool northCarpets = !(!closedWalls.Contains(CompassDirection.West) && !closedWalls.Contains(CompassDirection.East));

            for (int i = 0; i < 3; i++)
            {
                GameObject carpet;
                int x = i;
                if (northCarpets && i == 0 && closedWalls.Contains(CompassDirection.North))
                {
                    carpet = Instantiate(carpetEndPrefab, carpetContainer);
                    x++;
                }else if(northCarpets && i == 2 && closedWalls.Contains(CompassDirection.South)) {
                    carpet = Instantiate(carpetEndPrefab, carpetContainer);
                    carpet.transform.localEulerAngles = new Vector3(0, 180, 0);
                }else{
                    carpet = Instantiate(carpetPrefab, carpetContainer);
                }

                if (northCarpets)
                {
                    carpet.transform.localPosition = new Vector3(0, 0, -1.5f + x);
                }
                else
                {
                    carpet.transform.localEulerAngles += new Vector3(0, 90, 0);
                    carpet.transform.localPosition = new Vector3(-1.5f + x, 0, 0);
                }
            }
        }
    }
}