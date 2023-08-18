using System;
using Model.Hotel.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mini_First_Person_Controller.Scripts
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private InputAction actionKey;
        [SerializeField] private float actionDistance = 2;
        [SerializeField] private float actionRadius = 0.3f;

        private Transform _camera;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>().transform;
        }

        private void OnEnable()
        {
            actionKey.Enable();
            actionKey.performed += ActionPressed;
        }

        private void OnDisable()
        {
            actionKey.performed -= ActionPressed;
        }

        private void ActionPressed(InputAction.CallbackContext obj)
        {
            RaycastHit[] hits = new RaycastHit[3];

            int hitCount = Physics.SphereCastNonAlloc(_camera.position, actionRadius, _camera.forward, hits,
                actionDistance, LayerMask.GetMask("Interactable"));

            for (int i = 0; i < hitCount; i++)
            {
                Interactable ob = hits[i].collider.GetComponent<Interactable>();
                if (ob != null)
                {
                    ob.Interact();
                }
            }
        }
    }
}