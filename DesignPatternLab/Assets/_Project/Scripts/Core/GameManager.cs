using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DesignPatternLab.Core
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
            TimeSpan duration = _sessionEndTime.Subtract(_sessionStartTime);
            Debug.Log("Game session ended @: " + _sessionEndTime);
            Debug.Log("Game session lasted: " + duration);
        }

        private void OnGUI()
        {
            // Keep the Chapter 4 test controls away from the bike controls.
            GUILayout.BeginArea(new Rect(Screen.width - 236, 12, 224, 138), GUI.skin.box);
            GUILayout.Label("DesignPatternLab");
            GUILayout.Label("Scene: " + SceneManager.GetActiveScene().name);

            if (GUILayout.Button("Next Scene"))
            {
                int sceneCount = SceneManager.sceneCountInBuildSettings;

                if (sceneCount > 1)
                {
                    int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) % sceneCount;
                    SceneManager.LoadScene(nextScene);
                }
                else
                {
                    Debug.LogWarning("Add and enable the project scenes in the build scene list.");
                }
            }

            GUILayout.Label("Init > Main > SingletonTest");
            GUILayout.EndArea();
        }
    }
}
