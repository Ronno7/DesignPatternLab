using System.Collections;
using DesignPatternLab.Systems.PowerUps;
using DesignPatternLab.Systems.Weapons;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    [DisallowMultipleComponent]
    public class BikeWeapon : MonoBehaviour, IBikeElement
    {
        [Header("Range")]
        [Min(0)] public int range = 5;
        [Min(0)] public int maxRange = 25;
        [Header("Strength")]
        [Min(0f)] public float strength = 25f;
        [Min(0f)] public float maxStrength = 50f;

        [Header("Chapter 12 attachments")]
        public WeaponConfig weaponConfig;
        public WeaponAttachment mainAttachment;
        public WeaponAttachment secondaryAttachment;

        public bool IsDecorated { get; private set; }
        public bool IsFiring { get; private set; }
        public int ShotsFired { get; private set; }
        public IWeapon CurrentWeapon
        {
            get { InitializeWeapon(); return _weapon; }
        }

        private IWeapon _weapon;
        private bool _initialized;
        private Coroutine _firing;

        private void Awake() => InitializeWeapon();
        private void OnDisable() => StopFiring();

        // Also called by BikeController before it captures replay's starting values.
        public void InitializeWeapon()
        {
            if (_initialized)
                return;

            _initialized = true;
            if (weaponConfig)
            {
                range = Mathf.Clamp(Mathf.RoundToInt(weaponConfig.Range), 0, maxRange);
                strength = Mathf.Clamp(weaponConfig.Strength, 0f, maxStrength);
            }
            RebuildWeapon();
        }

        private void RebuildWeapon()
        {
            // Rebuild from the undecorated values: reapplying never stacks extra layers.
            _weapon = new Weapon(range, weaponConfig ? weaponConfig.Rate : 2f,
                strength, weaponConfig ? weaponConfig.Cooldown : 2f);
            if (IsDecorated)
            {
                if (mainAttachment)
                    _weapon = new WeaponDecorator(_weapon, mainAttachment);
                if (secondaryAttachment)
                    _weapon = new WeaponDecorator(_weapon, secondaryAttachment);
            }

            // A changed rate starts one fresh firing interval, never a second loop.
            if (IsFiring)
            {
                StopFiring();
                ToggleFire();
            }
        }

        public void Equip(WeaponAttachment main, WeaponAttachment secondary)
        {
            mainAttachment = main;
            secondaryAttachment = secondary;
            Decorate();
        }

        public void Decorate()
        {
            InitializeWeapon();
            IsDecorated = mainAttachment || secondaryAttachment;
            RebuildWeapon();
        }

        public void ResetWeapon()
        {
            InitializeWeapon();
            IsDecorated = false;
            RebuildWeapon();
        }

        public void ResetForRun()
        {
            StopFiring();
            ShotsFired = 0;
            ResetWeapon();
        }

        public void ToggleFire()
        {
            InitializeWeapon();
            if (IsFiring)
            {
                StopFiring();
                return;
            }
            if (!isActiveAndEnabled || _weapon.Rate <= 0f)
                return;

            IsFiring = true;
            _firing = StartCoroutine(FireWeapon());
        }

        public void StopFiring()
        {
            IsFiring = false;
            if (_firing != null)
                StopCoroutine(_firing);
            _firing = null;
        }

        private IEnumerator FireWeapon()
        {
            while (IsFiring && _weapon.Rate > 0f)
            {
                yield return new WaitForSeconds(1f / _weapon.Rate);
                if (IsFiring && _weapon.Rate > 0f)
                    Fire();
            }
            IsFiring = false;
            _firing = null;
        }

        // The chapter demonstrates firing cadence; it does not implement projectiles.
        public void Fire()
        {
            ShotsFired++;
            Debug.Log("Weapon fired!", this);
        }

        public void Accept(IVisitor visitor)
        {
            InitializeWeapon();
            visitor.Visit(this);
            RebuildWeapon();
        }
    }
}
