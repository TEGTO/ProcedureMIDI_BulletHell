using DG.Tweening;
using UnityEngine;

namespace Code
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float smoothSpeed = 0.5f; // SmoothSpeed will be used as duration for DOTween
        [SerializeField]
        private Vector3 offset;

        private float initialHeight;
        private Quaternion initialRotation;

        private void Start()
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }

            // Store the initial height and rotation if needed (not used in this version)
            initialHeight = transform.position.y;
            initialRotation = transform.rotation;

            // Start the camera smoothly at the offset position
            MoveCamera();
        }

        private void FixedUpdate()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            Vector3 desiredPosition = target.position + offset;

            // Use DOTween to move the camera to the desired position smoothly
            transform.DOMove(desiredPosition, smoothSpeed).SetEase(Ease.OutSine); // OutSine gives a smooth natural ease out
        }
    }
}

