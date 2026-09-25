using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.PowerUps;
using DesignPatternLab.Systems.Replay;
using DesignPatternLab.Systems.Replay.Commands;
using UnityEngine;

namespace DesignPatternLab.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BikeController), typeof(Invoker))]
    public class ClientVisitor : MonoBehaviour
    {
        public PowerUp enginePowerUp;
        public PowerUp shieldPowerUp;
        public PowerUp weaponPowerUp;
        public PowerUp comboPowerUp;
        private BikeController _bike;
        private Invoker _invoker;

        private void Awake()
        {
            _bike = GetComponent<BikeController>();
            _invoker = GetComponent<Invoker>();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(252, 12, 244, 268), GUI.skin.box);
            GUILayout.Label("Power-ups - Chapter 10 (Visitor)");
            GUILayout.Label("Shield: " + _bike.Shield.health.ToString("0") + "%");
            GUILayout.Label("Turbo boost: " + _bike.Engine.turboBoost.ToString("0") + " / " + _bike.Engine.maxTurboBoost);
            GUILayout.Label("Base weapon range: " + _bike.Weapon.range + " / " + _bike.Weapon.maxRange);
            GUILayout.Label("Base weapon strength: " + _bike.Weapon.strength.ToString("0") + " / " + _bike.Weapon.maxStrength);
            DrawButton("PowerUp Shield", shieldPowerUp);
            DrawButton("PowerUp Engine", enginePowerUp);
            DrawButton("PowerUp Weapon", weaponPowerUp);
            DrawButton("PowerUp Combo", comboPowerUp);
            GUILayout.Label("Start a race to use power-ups.");
            GUILayout.Label("Benefits last until the next run.");
            GUILayout.EndArea();
        }

        private void DrawButton(string label, PowerUp powerUp)
        {
            bool wasEnabled = GUI.enabled;
            GUI.enabled = wasEnabled && powerUp && _invoker.IsRecording && _bike.IsEngineOn;
            if (GUILayout.Button(label))
                _invoker.ExecuteCommand(new ApplyPowerUp(_bike, powerUp));
            GUI.enabled = wasEnabled;
        }
    }
}
