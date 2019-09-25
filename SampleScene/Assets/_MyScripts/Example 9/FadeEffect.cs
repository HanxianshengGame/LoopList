using System;
using System.Collections;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _MyScripts.Example_9
{
      public class FadeEffect : MonoBehaviour
      {
            private int _id;
            private int _centerId, _lastCenterId;
            private float _radius;
            private PhotoWallItem myItem;
            private Func<int, PhotoWallItem> _getItem;
            private CanvasGroup _canvasGroup;
            public void Init(int id, float radius, PhotoWallItem item, Func<int, PhotoWallItem> GetItem)
            {
                  _id = id;
                  _radius = radius;
                  _getItem = GetItem;
                  myItem = item;
                  _canvasGroup = GetComponent<CanvasGroup>();
            }

            public void Play(int centerId, float targetAlpha)
            {
                  if (centerId == _centerId || _id == centerId||(_lastCenterId==centerId&&_canvasGroup.alpha==targetAlpha)|| myItem.IsCenter) return;
                  _lastCenterId = _centerId;
                  _centerId = centerId;
                  if (_getItem(centerId).GetDistance(myItem) <= _radius)
                  {
                        if (targetAlpha == 0) myItem.MyImage.raycastTarget = false;
                        else myItem.MyImage.raycastTarget = true;
                        _canvasGroup.DOKill();
                        _canvasGroup.DOFade(targetAlpha, 0.5f).OnComplete(() =>
                        {
                              _lastCenterId = _centerId;
                              _centerId = -1;
                        });
                        
                        StartCoroutine(WaitFade(centerId, targetAlpha));
                  }
            }

            private IEnumerator WaitFade(int centerId,float targetAlpha)
            {
                  yield return new WaitForSeconds(0.1f);
                  PlayNeighbor(centerId,targetAlpha);
            }

            public void PlayNeighbor(int centerId,float targetAlpha)
            {
                  foreach (var id in myItem.NeiborIds)
                  {
                        _getItem(id).MyFadeEffect.Play(centerId,targetAlpha);
                  }
            }
      }
}


