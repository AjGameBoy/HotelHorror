using System;
using System.Collections;
using UnityEngine;

namespace Model.Hotel.Actions
{
    public class Door : Interactable
    {
        public bool canOpen = true;
        public bool isOpen = false;

        [SerializeField] private float moveSpeed = 1; 

        private bool _processing = false;

        private AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        public override void Interact()
        {
            if (!canOpen)
                return;

            if (_processing)
                return;

            _processing = true;

            _audio.Play();
            StartCoroutine(OpenDoor());
        }

        IEnumerator OpenDoor()
        {
            float t = 0;
            while (t < moveSpeed)
            {
                float factor = (t / moveSpeed);
                if (isOpen)
                    factor = moveSpeed - factor;
                transform.localEulerAngles = new Vector3(0, -90 * factor, 0);
                t += Time.deltaTime;
                yield return null;
            }

            isOpen = !isOpen;
            _processing = false;
        }

    }
}
