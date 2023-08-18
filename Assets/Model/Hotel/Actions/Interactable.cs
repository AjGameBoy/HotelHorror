using UnityEngine;

namespace Model.Hotel.Actions
{
    public class Interactable : MonoBehaviour
    {
        public virtual void Interact()
        {
            Debug.Log($"Do the action on {name}");
        }
    }
}
