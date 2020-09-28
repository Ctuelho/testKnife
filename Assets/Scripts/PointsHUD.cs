using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Game 
{
    public class PointsHUD : MonoBehaviour
    {
        //reference
        public static PointsHUD Instance = null;

        #region private fields
        [SerializeField] private Text _pointsText;
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
            _pointsText.transform.DOComplete();
            _pointsText.gameObject.SetActive(true);
            _pointsText.text = GameManager.Instance.Points.ToString();
            _pointsText.transform.DOPunchScale(Vector3.one * 2, GameManager.DURATION_0);
        }
        #endregion public functions
    }
}
