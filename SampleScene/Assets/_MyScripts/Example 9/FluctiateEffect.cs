using System.Collections;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _MyScripts.Example_9
{
    public class FluctiateEffect : MonoBehaviour
    {
        private int _id;
        private int _centerId, _lastCenterId;
        private PhotoWallItem myItem;
        private Func<int, PhotoWallItem> _getItem;
        private Vector3 _defaultScale;
        public void Init(int id, PhotoWallItem item, Func<int, PhotoWallItem> GetItem)
        {
            _id = id;
            _getItem = GetItem;
            myItem = item;
            _defaultScale = transform.localScale;
        }

        public void Play(int centerId)
        {
            if (centerId == _centerId || _id == centerId||_lastCenterId==centerId|| myItem.IsCenter) return;
            _lastCenterId = _centerId;
            _centerId = centerId;
            transform.DOKill();
            transform.localScale = _defaultScale;
            transform.DOScale(1.5f, 0.1f).SetLoops(2,LoopType.Yoyo);
            StartCoroutine(WaitFade(centerId));
        }

        private IEnumerator WaitFade(int centerId)
        {
            yield return new WaitForSeconds(0.1f);
            PlayNeighbor(centerId);
        }

        public void PlayNeighbor(int centerId)
        {
            foreach (var id in myItem.NeiborIds)
            {
                _getItem(id).MyFluctiateEffect.Play(centerId);
            }
        }
    }
}
