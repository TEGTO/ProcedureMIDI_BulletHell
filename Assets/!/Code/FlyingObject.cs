using DG.Tweening;
using UnityEngine;

namespace Code
{
    public class FlyingObject : MonoBehaviour
    {
        private Tween move;
        private Tween rotate;

        public float DespawnZoneRadius { get; set; }

        private void Update()
        {
            if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > DespawnZoneRadius)
            {
                CompleteTweens();
            }
        }

        public void MoveWithDOTween(Vector3 direction, float speed, float distance)
        {
            Vector3 targetPosition = transform.position + direction.normalized * distance;
            float duration = distance / speed;

            move = transform.DOMove(targetPosition, duration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => ReturnToPool());

            rotate = transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360);
        }
        private void ReturnToPool()
        {
            gameObject.SetActive(false);
        }
        private void CompleteTweens()
        {
            if (move != null) move.Complete();
            if (rotate != null) rotate.Complete();
        }
        private void OnCollisionEnter(Collision collision)
        {
            CompleteTweens();
        }
    }
}