using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class KnifeIcon : MonoBehaviour
    {
        #region private fields
        [SerializeField] private Image _availableIcon;
        [SerializeField] private Image _consumedIcon;
        #endregion private fields

        #region public functions
        public void SetAvailable()
        {
            _availableIcon.enabled = true;
            _consumedIcon.enabled = false;
            gameObject.SetActive(true);
        }

        public void Consume()
        {
            _availableIcon.enabled = false;
            _consumedIcon.enabled = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion public functions
    }
}