using System.Collections;
using UnityEngine;

namespace Farm2D
{
    public class Arc : MonoBehaviour
    {
        private Transform _transform;

        private void Awake() => _transform = transform;

        public IEnumerator TravelArc(Vector3 destination, float duration)
        {
            var startPos = _transform.position;

            float percentComplete = 0f;

            while (percentComplete < 1f)
            {
                percentComplete += Time.deltaTime / duration;

                float currentHeight = Mathf.Sin(Mathf.PI * percentComplete);

                _transform.position = Vector3.Lerp(startPos, destination, percentComplete) + Vector3.up * currentHeight;

                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
