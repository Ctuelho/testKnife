using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game {
    public class LevelIcon : MonoBehaviour
    {
        #region private fields
        [SerializeField] private Image _icon;
        #endregion private fields

        #region public functions
        public void Flash()
        {
            transform.DOComplete();
            transform.localScale = Vector3.zero;
            _icon.enabled = true;
            transform.DOScale(Vector3.one, GameManager.DURATION_1).SetEase(Ease.OutBounce);
        }

        public void Reset()
        {
            _icon.enabled = false;
        }
        #endregion public functions
    }
}