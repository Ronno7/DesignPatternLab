using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chapter.Singleton
{
    // Adapted from David Baron, Chapter 4.
    public class GameManager : Singleton<GameManager>
    {
        private DateTime _sessionStartTime;
        private DateTime _sessionEndTime;

        private void Start()
        {
            _sessionStartTime = DateTime.Now;
            Debug.Log("Game session start @: " + _sessionStartTime);
        }

        private void OnApplicationQuit()
        {
            _sessionEndTime = DateTime.Now;
            TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);

            Debug.Log("Game session ended @: " + _sessionEndTime);
            Debug.Log("Game session lasted: " + timeDifference);
        }

        private void OnGUI()
        {
            GUILayout.Label("Scene: " + SceneManager.GetActiveScene().name);

            if (GUILayout.Button("Next Scene", GUILayout.Width(140)))
            {
                int sceneCount = SceneManager.sceneCountInBuildSettings;

                if (sceneCount < 2)
                {
                    Debug.LogWarning("Add and enable Init and TestScene in the build scene list.");
                    return;
                }

                // Return to the first scene after the last scene.
                int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % sceneCount;
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
