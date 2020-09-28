using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class ApplesHUD : MonoBehaviour
    {
        //reference
        public static ApplesHUD Instance = null;

        #region private fields
        [SerializeField] private Text _apples;
        [SerializeField] private Transform _panel;
        [SerializeField] private GameObject _appleIcon;
        #endregion private fields

        #region unity event functions
        private void Awake()
        {
            Instance = this;
        }
        #endregion unity event functions

        #region public functions
        public void Refresh()
        {
            _apples.transform.DOComplete();
            _apples.gameObject.SetActive(true);
            _appleIcon.gameObject.SetActive(true);
            _panel.gameObject.SetActive(true);
            _apples.text = GameManager.Instance.Apples.ToString();
            _panel.transform.DOPunchScale(Vector3.one * 2, GameManager.DURATION_0);
        }
        #endregion public functions
    }
}
