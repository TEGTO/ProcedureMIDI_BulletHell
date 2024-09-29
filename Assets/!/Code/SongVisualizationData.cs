using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    [Serializable]
    public struct NoteData
    {
        public NoteName Note;
        [SerializeField, FormerlySerializedAs("numberOfObjects")]
        public int NumberOfObjects;
        public float ExplosionSpeed;
    }
    [CreateAssetMenu(fileName = "SongVisualizationData", menuName = "ScriptableObjects/SongVisualizationData", order = 1)]

    public class SongVisualizationData : ScriptableObject
    {
        [SerializeField]
        private AudioClip audioClip;
        [SerializeField]
        private string midiFilePath;
        [SerializeField]
        private float noteTime = 1f;
        [SerializeField]
        private NoteData[] noteData;
        [SerializeField]
        private float explosionDistance = 500f;
        [SerializeField]
        private float safeZoneRadius = 5f;
        [SerializeField]
        private float despawnZoneRadius = 50f;
        [SerializeField, FormerlySerializedAs("nmberOfObjectsCoeff")]
        private float numberOfObjectsCoeff = 1f;
        [SerializeField]
        private float speedOfObjectsCoeff = 1f;

        public AudioClip AudioClip
        {
            get => audioClip;
        }
        public string MidiFilePath
        {
            get => midiFilePath;
        }
        public float NoteTime
        {
            get => noteTime;
        }
        public float ExplosionDistance
        {
            get => explosionDistance;
        }
        public float SafeZoneRadius
        {
            get => safeZoneRadius;
        }
        public float DespawnZoneRadius
        {
            get => despawnZoneRadius;
        }

        public NoteData GetNoteDate(NoteName noteRestriction)
        {
            var note = noteData.First(x => x.Note == noteRestriction);
            note.NumberOfObjects = (int)(note.NumberOfObjects * numberOfObjectsCoeff);
            note.ExplosionSpeed *= speedOfObjectsCoeff;
            return note;
        }
    }
}