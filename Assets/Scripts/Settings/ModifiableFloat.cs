using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public class ModifiableFloat
    {
        [SerializeField] private float baseValue = 1f;
        private bool _shouldDecreaseValue = true;

        public ModifiableFloat(float defaultValue = 1f, bool shouldDecreaseTime = true)
        {
            baseValue = defaultValue;
            _shouldDecreaseValue = shouldDecreaseTime;
        }

        public float Value
        {
            get
            {
                return _shouldDecreaseValue ?
                    baseValue / GameplaySpeedSetting.GameplaySpeed :
                    baseValue * GameplaySpeedSetting.GameplaySpeed;
            }
        }


        public float RawValue
        {
            get => baseValue;
            set => baseValue = value;
        }
    }
}
