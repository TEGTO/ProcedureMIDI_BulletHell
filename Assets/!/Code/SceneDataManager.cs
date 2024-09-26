using UnityEngine;

namespace Code
{
    public class SceneDataManager : MonoBehaviour
    {
        public static SceneDataManager Instance { get; private set; }

        [SerializeField]
        private SongVisualizationData songData;

        public SongVisualizationData SongData { get => songData; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }
            Destroy(this);
        }
    }
}