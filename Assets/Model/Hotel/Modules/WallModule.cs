using System;
using UnityEngine;

namespace Model.Hotel.Modules
{
    public class WallModule : MonoBehaviour
    {
        [SerializeField] private GameObject wallObject;
        [SerializeField] private GameObject doorObject;
        [SerializeField] private GameObject lightObject;

        public CompassDirection side;
        public int sort;

        public bool isOpen = false;
        public bool isDoor = false;
        public bool hasLight = false;
        public LightStatus lightStatus = LightStatus.On;

        private void OnEnable()
        {
            lightObject.GetComponent<LightController>().lightStatus = lightStatus;
        }

        public void UpdateWall()
        {
            if (isOpen)
            {
                lightObject.SetActive(false);
                wallObject.SetActive(false);
                doorObject.SetActive(false);
            } else if (isDoor)
            {
                lightObject.SetActive(false);
                wallObject.SetActive(false);
                doorObject.SetActive(true);
            }
            else
            {
                lightObject.SetActive(hasLight);
                wallObject.SetActive(true);
                doorObject.SetActive(false);
            }

            if (lightObject.activeSelf)
                lightObject.GetComponent<LightController>().lightStatus = lightStatus;
        }
    }
}
