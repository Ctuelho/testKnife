using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //holds and displays the number of knives left/used by the player
    public class KnivesCounterHUD : MonoBehaviour
    {
        //reference
        public static KnivesCounterHUD Instance = null;

        #region private fields
        [SerializeField] private GameObject _knifeIconPrefab;
        [SerializeField] private RectTransform _iconsParent;
        private List<KnifeIcon> _icons;
        private int _currentIconIndex = 0;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
            _icons = new List<KnifeIcon>();
        }
        #endregion unity event functions

        #region public functions
        public void DisplayKnives(int knives)
        {
            _currentIconIndex = 0;

            var currentIconsAmount = _icons.Count;
            if (_icons.Count < knives)
            {
                for(int i = 0; i < knives - currentIconsAmount; i++)
                {
                    var newIcon = Instantiate(_knifeIconPrefab);
                    var knifeIcon = newIcon.GetComponent<KnifeIcon>();
                    newIcon.transform.SetParent(_iconsParent);
                    _icons.Add(knifeIcon);
                }
            }

            for(int i = 0; i < _icons.Count; i++)
            {
                if(i < knives)
                {
                    _icons[i].SetAvailable();
                }
                else
                {
                    _icons[i].Hide();
                }
            }
        }

        public void UseKnife()
        {
            _icons[_currentIconIndex].Consume();
            _currentIconIndex++;
        }

        public void RecoverKnife()
        {
            _currentIconIndex--;
            _icons[_currentIconIndex].SetAvailable();
        }

        public void Show()
        {
            _iconsParent.gameObject.SetActive(true);
        }
        #endregion public functions
    }
}