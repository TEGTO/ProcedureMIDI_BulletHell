using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeController : MonoBehaviour
    {
        [SerializeField] private float rollSpeed = 5f;
        [SerializeField] private LayerMask wallLayerMask;
        [SerializeField] private float rayDistance = 1f;
        private bool isMoving;

        private void Update()
        {
            if (isMoving) return;

            if (Input.GetKey(KeyCode.A)) TryMove(Vector3.left);
            else if (Input.GetKey(KeyCode.D)) TryMove(Vector3.right);
            else if (Input.GetKey(KeyCode.W)) TryMove(Vector3.forward);
            else if (Input.GetKey(KeyCode.S)) TryMove(Vector3.back);
        }

        private void TryMove(Vector3 direction)
        {
            if (CanMove(direction))
            {
                Assemble(direction);
            }
        }
        private bool CanMove(Vector3 direction)
        {
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, rayDistance, wallLayerMask))
            {
                Debug.Log("Blocked by a wall in the direction: " + direction);
                return false;
            }
            return true;
        }

        private void Assemble(Vector3 dir)
        {
            var anchor = transform.position + (Vector3.down + dir) * 0.5f;
            var axis = Vector3.Cross(Vector3.up, dir);
            StartCoroutine(Roll(anchor, axis));
        }

        private IEnumerator Roll(Vector3 anchor, Vector3 axis)
        {
            isMoving = true;
            for (var i = 0; i < 90 / rollSpeed; i++)
            {
                transform.RotateAround(anchor, axis, rollSpeed);
                yield return new WaitForSeconds(0.01f);
            }
            isMoving = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("FlyingObject"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}