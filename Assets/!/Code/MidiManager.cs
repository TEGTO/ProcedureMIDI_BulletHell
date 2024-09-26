using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class MidiManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private ExplosionController[] explosionControllers;

        private MidiFile midiFile;
        private float noteTime = 1f;

        private void Start()
        {
            var songVisualizationData = SceneDataManager.Instance.SongData;

            noteTime = songVisualizationData.NoteTime;
            midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + songVisualizationData.MidiFilePath);
            audioSource.clip = songVisualizationData.AudioClip;

            foreach (var controller in explosionControllers)
            {
                controller.SetTimeStamps(ParseMidiFile(midiFile).ToArray(), midiFile.GetTempoMap());
            }

            StartSong();
        }

        private List<Note> ParseMidiFile(MidiFile file)
        {
            return midiFile.GetNotes().ToList();
        }

        public void StartSong()
        {
            audioSource.Play();
        }
        public double GetAudioSourceTime()
        {
            return ((double)audioSource.timeSamples / audioSource.clip.frequency) + noteTime;
        }
    }
}