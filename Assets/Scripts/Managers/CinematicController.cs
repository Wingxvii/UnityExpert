using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace Nightmare
{
    public class CinematicController : MonoBehaviour
    {
        public enum CinematicType
        {
            Realtime,
            PreRendered
        }

        private PlayableDirector cinematicTimeline;
        private Camera cineCam;
        private Camera mainCam;
        private VideoPlayer videoPlayer;
        private int currentCinematic;

        // Use this for initialization
        void Start()
        {
            cinematicTimeline = this.GetComponent<PlayableDirector>();
            cineCam = this.GetComponentInChildren<Camera>();
            videoPlayer = this.GetComponentInChildren<VideoPlayer>();

            mainCam = Camera.main;
            videoPlayer.targetCamera = mainCam;
            videoPlayer.loopPointReached += VideoEnded;

            RealtimeCameraMode(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (currentCinematic >= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SkipCinematic();
                }

                if (currentCinematic == 0 && cinematicTimeline.state != PlayState.Playing)
                {
                    RestorePlay();
                }
            }
        }

        public void StartCinematic(CinematicType type)
        {
            if (type < 0)
                return;

            currentCinematic = (int)type;
            EventManager.TriggerEvent("Pause", true);

            if (type == CinematicType.Realtime)
            {
                RealtimeCameraMode(true);
                cinematicTimeline.Play();
            }
            else
            {
                if (videoPlayer.source == VideoSource.VideoClip && videoPlayer.clip == null)
                    Debug.LogError("Pre-rendered video clip not set!  Gameplay paused.");
                else
                    videoPlayer.Play();
            }
        }

        void SkipCinematic()
        {
            if (currentCinematic == 0)
            {
                cinematicTimeline.Stop();
            }
            else
            {
                videoPlayer.Stop();
            }

            RestorePlay();
        }

        void RestorePlay()
        {
            currentCinematic = -1;
            RealtimeCameraMode(false);
            EventManager.TriggerEvent("Pause", false);
        }

        private void RealtimeCameraMode(bool isCinematic)
        {
            cineCam.enabled = isCinematic;
            mainCam.enabled = !isCinematic;
        }

        private void VideoEnded(VideoPlayer player)
        {
            player.Stop();
            RestorePlay();
        }
    }
}