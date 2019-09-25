using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _MyScripts.Example_9
{
    public class PhotoWallItem : MonoBehaviour,IPointerDownHandler
    {
        public int ID { get; private set; }
        public bool IsCenter;
        private float _showScale = 5;     //正常显示尺寸
        public bool IsleftBorder { get; set; }
        public bool IsRightBorder { get; set; }
        public RectTransform MyRect { get; private set; }

        public List<int> NeiborIds { get; private set; }
        private FadeEffect myFadeEffect;
        private Image myImage;

        public Image MyImage
        {
            get
            {
                if (myImage == null) myImage = GetComponent<Image>();
                return myImage;
            }
        }

        public FadeEffect MyFadeEffect
        {
            get
            {
                if (myFadeEffect == null)
                {
                    myFadeEffect = GetComponent<FadeEffect>();
                }

                return myFadeEffect;
            }
        }
        private FluctiateEffect myFluctiateEffect;

        public FluctiateEffect MyFluctiateEffect
        {
            get
            {
                if (myFluctiateEffect == null)
                {
                    myFluctiateEffect = GetComponent<FluctiateEffect>();
                }

                return myFluctiateEffect;
            }
        }
        

        /// <summary>
        /// 得到相邻物体的id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="column"></param>
        /// <param name="totalNum"></param>
        private void GetNeighborId(int id,int column,int totalNum)
        {
            int idTemp = id - 1; //左
            if (idTemp >= 0 && !IsleftBorder)
                NeiborIds.Add(idTemp);

            idTemp = id + 1;
            if(idTemp<totalNum&&!IsRightBorder)
                NeiborIds.Add(idTemp);

            idTemp = id - column;
            if(idTemp>=0)
                NeiborIds.Add(idTemp);

            idTemp = id + column;
            if(idTemp<totalNum)
                NeiborIds.Add(idTemp);


        }
   
        public void Init(int id,int column,int totalNum,Func<int,PhotoWallItem> getItem)
        {
            ID = id;
            IsCenter = false;
            NeiborIds=new List<int>();
            
            MyRect = GetComponent<RectTransform>();
            SetName(id);
            GetNeighborId(id,column,totalNum);
            float radius = GetRadius(MyRect, _showScale);
            
            myFadeEffect = gameObject.AddComponent<FadeEffect>();
            myFadeEffect.Init(id,radius,this,getItem);
            myFluctiateEffect = gameObject.AddComponent<FluctiateEffect>();
            myFluctiateEffect.Init(id,this,getItem);


        }

        private void SetName(int id)
        {
            transform.GetComponentInChildren<Text>().text = id.ToString();
        }

        private float GetRadius(RectTransform rect,float scaleSize)
        {
            //直径
            float hypotenus = Mathf.Sqrt(Mathf.Pow(rect.rect.width * scaleSize, 2) + Mathf.Pow(rect.rect.height*_showScale, 2));
            return hypotenus * 0.5f;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!MyImage.raycastTarget) return;
            
            float maxScale = 50;
            float time = 0.1f;
            
            if (!IsCenter)
            {
                IsCenter = true;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(MyRect.DOScale(maxScale,time));
                sequence.Append(MyRect.DOScale(_showScale, time));
                sequence.OnComplete(() =>
                {
                    MyFadeEffect.PlayNeighbor(ID, 0);
                    MyFluctiateEffect.PlayNeighbor(ID);
                });
                transform.SetAsLastSibling();
            }
            else
            {
                IsCenter = false;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(MyRect.DOScale(maxScale,time));
                sequence.Append(MyRect.DOScale(1, time));
                sequence.OnComplete(() => { MyFadeEffect.PlayNeighbor(ID, 1); });
                transform.SetAsFirstSibling();

            }
        }

        public float GetDistance(PhotoWallItem item)
        {
           return Vector2.Distance(MyRect.anchoredPosition, item.MyRect.anchoredPosition);
        }
        
    }
}