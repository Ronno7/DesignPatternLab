using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Replay;
using DesignPatternLab.Systems.Replay.Commands;
using DesignPatternLab.Systems.Weapons;
using UnityEngine;

namespace DesignPatternLab.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BikeWeapon))]
    public class ClientDecorator : MonoBehaviour
    {
        public WeaponAttachment injector;
        public WeaponAttachment stabilizer;
        public WeaponAttachment cooler;
        private BikeWeapon _weapon;
        private Invoker _invoker;
        private int _main = 1, _secondary = 3;
        private static readonly string[] Choices = { "None", "Inj", "Stab", "Cool" };

        private void Awake()
        {
            _weapon = GetComponent<BikeWeapon>();
            _invoker = GetComponent<Invoker>();
        }

        private WeaponAttachment Selected(int index)
        {
            switch (index)
            {
                case 1: return injector;
                case 2: return stabilizer;
                case 3: return cooler;
                default: return null;
            }
        }

        private void Execute(Command command)
        {
            if (_invoker)
                _invoker.ExecuteCommand(command);
            else
                command.Execute();
        }

        private static string AttachmentName(WeaponAttachment attachment)
            => attachment ? attachment.attachmentName : "None";

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(508, 12, 204, 420), GUI.skin.box);
            GUILayout.Label("Attachments - Chapter 12");
            IWeapon stats = _weapon.CurrentWeapon;
            GUILayout.Label("Range: " + stats.Range.ToString("0.#"));
            GUILayout.Label("Strength: " + stats.Strength.ToString("0.#"));
            GUILayout.Label("Rate: " + stats.Rate.ToString("0.#") + " shots/s");
            GUILayout.Label("Cooldown: " + stats.Cooldown.ToString("0.#") + " s");
            GUILayout.Label("Firing: " + _weapon.IsFiring + " | Shots: " + _weapon.ShotsFired);
            GUILayout.Label("Main slot choice");
            _main = GUILayout.Toolbar(_main, Choices);
            GUILayout.Label("Secondary slot choice");
            _secondary = GUILayout.Toolbar(_secondary, Choices);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = wasEnabled && (!_invoker || _invoker.IsRecording);
            if (GUILayout.Button("Decorate / Apply Slots"))
                Execute(new SetWeaponAttachments(_weapon, Selected(_main), Selected(_secondary)));
            if (GUILayout.Button("Reset Weapon"))
                Execute(new SetWeaponAttachments(_weapon, null, null));
            if (GUILayout.Button("Toggle Fire"))
                Execute(new ToggleWeaponFire(_weapon));
            GUI.enabled = wasEnabled;

            GUILayout.Label("Equipped main: " + (_weapon.IsDecorated ? AttachmentName(_weapon.mainAttachment) : "None"));
            GUILayout.Label("Equipped second: " + (_weapon.IsDecorated ? AttachmentName(_weapon.secondaryAttachment) : "None"));
            GUILayout.Label("Start a race to use controls.");
            GUILayout.EndArea();
        }
    }
}
