using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class KnifeItem : MonoBehaviour
    {
        #region public fields
        public KNIFE_ITEMS KnifeType;
        #endregion public fields

        #region properties
        public bool Unlocked { get; private set; } = false;
        #endregion properties

        #region private fields
        [SerializeField] private ParticleSystem _confetti;
        [SerializeField] private GameObject _lockedIcon;
        [SerializeField] private GameObject _unlockedIcon;
        [SerializeField] private GameObject _selector;
        private Button _button;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(() => {
                if (Unlocked)
                {
                    _selector.SetActive(true);
                    GameEvents.OnKnifeSelected(KnifeType);
                }
            });
        }
        #endregion uniry event functions

        #region public functions
        public void Unlock(bool fromLoad)
        {
            Unlocked = true;
            _lockedIcon.SetActive(false);
            _unlockedIcon.SetActive(true);

            if (!fromLoad)
            {
                _confetti.Play();
            }
        }

        public void Unselect()
        {
            _selector.SetActive(false);
        }
        #endregion public functions
    }
}