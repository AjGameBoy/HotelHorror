using UnityEngine;

namespace Model.Hotel.Modules
{
    [ExecuteAlways]
    public class LightController : MonoBehaviour
    {
        [SerializeField] private Material lightOnMaterial;
        [SerializeField] private Material lightOffMaterial;
        [SerializeField] private MeshRenderer lightMesh;
        private LightStatus _lightStatus;
        private Light _light;

        private float _flickerTimer = -1;

        private float _lastEditorUpdateTime = 0;

        private Light light
        {
            get
            {
                if (_light == null)
                    _light = GetComponentInChildren<Light>();
                return _light;
            }
        }

        public LightStatus lightStatus
        {
            get => _lightStatus;
            set
            {
                _lightStatus = value;
                UpdateLight();
            }
        }

        private void Update()
        {
            if (lightStatus != LightStatus.Flicker)
                return;
            
#if UNITY_EDITOR
            if (_lastEditorUpdateTime == 0) 
                _lastEditorUpdateTime = Time.realtimeSinceStartup;
            
            _flickerTimer -= Time.realtimeSinceStartup - _lastEditorUpdateTime;
            _lastEditorUpdateTime = Time.realtimeSinceStartup;
#else
            _flickerTimer -= Time.deltaTime;
#endif
            if (_flickerTimer < 0)
            {
                _flickerTimer = Random.Range(0.001f, 0.5f);
                SetLight(!light.enabled);
            }
        }

        private void SetLight(bool on)
        {
            light.enabled = on;
            lightMesh.material = on ? lightOnMaterial : lightOffMaterial;
        }

        private void UpdateLight()
        {
            if (lightStatus == LightStatus.Off)
            {
                SetLight(false);
            }
            else if (lightStatus == LightStatus.On)
            {
                SetLight(true);
            }
        }
    }
}