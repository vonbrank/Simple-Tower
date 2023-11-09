using UnityEngine;

namespace UI.InGame
{
    public class FacingCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}