﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game 
{
    public class Knife : MonoBehaviour
    {
        #region properties
        public bool Broken
        {
            get => _used;
            private set => _used = value;
        }
        #endregion properties

        #region private fields
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ParticleSystem _hit;
        [SerializeField] private ParticleSystem _break;
        private bool _used;
        #endregion private fields

        #region unity event functions
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_used)
            {
                return;
            }

            //checks collision with another knife
            if(other.gameObject.layer == GameManager.LAYER_USED_KNIVES)
            {
                _used = true;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.gravityScale = 1;
                _collider.enabled = false;

                GetComponent<SpriteRenderer>().color = Color.red;
                _break.Play();

                GameEvents.OnKnifeBroken();
            }
            //checks collision with target
            else if (other.gameObject.layer == GameManager.LAYER_TARGET)
            {
                //hit the target
                _used = true;
                _rigidbody.isKinematic = true;
                if (KnifeBlock.Instance.KnivesLeft > 0)
                {   
                    Attach(other.gameObject.transform, other.gameObject.transform.position - Vector3.up * GameManager.DISTANCE_KNIFE);
                    _hit.Play();
                    //the last knife won't be attached, it will go through the target
                }
               
                GameEvents.OnKnifeAttached();
            }
        }
        #endregion unity event functions

        #region public functions
        //sets the knife in motion
        public void SetVelocity(Vector3 veclocity)
        {
            _rigidbody.velocity = veclocity;
        }

        //attachs the knife to the target, and it becomes an obstacle
        public void Attach(Transform parent, Vector3 position)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            transform.SetParent(parent);
            transform.position = position;

            //move the knife to a layer where it can work as an obstacle
            gameObject.layer = GameManager.LAYER_USED_KNIVES;
        }
        #endregion public functions
    }
}