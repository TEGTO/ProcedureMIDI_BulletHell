using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System.Collections.Generic;
using UnityEngine;
using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace Code
{
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField]
        private GameObject objectPrefab;
        [SerializeField]
        private NoteName noteRestriction;
        [SerializeField]
        private Color color;

        public List<double> TimeStamps { get; set; }

        private readonly List<FlyingObject> objectPool = new List<FlyingObject>();
        private int spawnIndex = 0;
        private MidiManager midiManager;
        private GameObject player;
        private float safeZoneRadius;
        private float despawnZoneRadius;
        private float numberOfObjects;
        private float explosionSpeed;
        private float explosionDistance;

        private void Start()
        {
            midiManager = FindAnyObjectByType<MidiManager>();

            var songVisualizationData = SceneDataManager.Instance.SongData;

            safeZoneRadius = songVisualizationData.SafeZoneRadius;
            despawnZoneRadius = songVisualizationData.DespawnZoneRadius;

            var noteData = songVisualizationData.GetNoteDate(noteRestriction);
            numberOfObjects = noteData.NumberOfObjects;
            explosionSpeed = noteData.ExplosionSpeed;
            explosionDistance = songVisualizationData.ExplosionDistance;

            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (spawnIndex < TimeStamps.Count)
            {
                if (midiManager.GetAudioSourceTime() >= TimeStamps[spawnIndex])
                {
                    Explode();
                    spawnIndex++;
                }
            }
        }

        public void SetTimeStamps(Note[] array, TempoMap tempoMap)
        {
            var timeStamps = new List<double>();
            foreach (var note in array)
            {
                if (note.NoteName == noteRestriction)
                {
                    var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
                    timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                }
            }
            TimeStamps = timeStamps;
        }

        private void Explode()
        {
            float angleStep = 360f / numberOfObjects;
            float angle = 0f;

            Vector3 spawnPosition;
            do
            {
                spawnPosition = GetRandomPositionInZone();
            }
            while (Vector3.Distance(spawnPosition, player.transform.position) < safeZoneRadius);

            for (int i = 0; i < numberOfObjects; i++)
            {
                FlyingObject obj = GetObjectFromPool();
                obj.gameObject.SetActive(true);

                obj.transform.position = spawnPosition;

                float dirX = Mathf.Sin(angle * Mathf.Deg2Rad);
                float dirZ = Mathf.Cos(angle * Mathf.Deg2Rad);
                Vector3 moveDirection = new Vector3(dirX, 0, dirZ).normalized;

                obj.MoveWithDOTween(moveDirection, explosionSpeed, explosionDistance);

                angle += angleStep;
            }
        }
        private Vector3 GetRandomPositionInZone()
        {
            float randomDistance = Random.Range(safeZoneRadius, despawnZoneRadius);
            float randomAngle = Random.Range(0f, 360f);

            float spawnX = player.transform.position.x + randomDistance * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float spawnZ = player.transform.position.z + randomDistance * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            return new Vector3(spawnX, 0, spawnZ);
        }
        private FlyingObject GetObjectFromPool()
        {
            foreach (var obj in objectPool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    return obj;
                }
            }

            GameObject newObj = Instantiate(objectPrefab);

            var renderer = newObj.GetComponent<Renderer>();
            renderer.material.SetColor("_EmissionColor", color);
            renderer.material.color = color;
            var flyingObject = newObj.GetComponent<FlyingObject>();
            flyingObject.DespawnZoneRadius = despawnZoneRadius;

            objectPool.Add(flyingObject);
            return flyingObject;
        }
    }
}