using UnityEngine;

namespace LGamesDev
{
    public class CameraScrolling : MonoBehaviour
    {
        public float moveSpeed;

        // Update is called once per frame
        private void Update()
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}