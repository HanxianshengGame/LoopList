using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _MyScripts.Example_6
{
    //血条项View
    public class LifeBarItem : MonoBehaviour
    {
        private RectTransform myRect;
        private LifeBarItem _childItem;
        public RectTransform MyRect
        {
            get
            {
                if (myRect == null)
                    myRect=GetComponent<RectTransform>();
                return myRect;
            }
        }

        private Image myImage;

        public Image MyImage
        {
            get
            {
                if (myImage == null)
                    myImage = GetComponent<Image>();
                return myImage;
            }
        }

        private float _defaultWidth;
       /// <summary>
       /// 初始化血条项
       /// </summary>
        public void Init()
        {
            if (transform.Find("AdditionBar")!=null)
            {
                _childItem = transform.Find("AdditionBar").gameObject.AddComponent<LifeBarItem>();
            }

            _defaultWidth = MyRect.rect.width;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        public void SetData(LifeBarData data)
        {
            MyImage.color = data.BarMainColor;
            myImage.sprite=data.BarSprite ;
            if(_childItem!=null)
               _childItem.SetData(data);
        }
        
        /// <summary>
        /// 改变生命值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float ChangeLife(float value)
        {
            if (_childItem != null)
            {
                _childItem.DOKill();
                _childItem.MyImage.color = MyImage.color;
                _childItem.MyRect.sizeDelta = MyRect.sizeDelta;
                _childItem.MyImage.DOFade(0, 0.5f).OnComplete(() => { _childItem.ChangeLife(value); });
            }
            MyRect.sizeDelta += Vector2.right * value;
            return GetOutOfRange();
        }

        /// <summary>
        /// 得到超出范围外的偏移量
        /// </summary>
        /// <returns></returns>
        private float GetOutOfRange()    
        {
            float offset = 0;
            if (MyRect.rect.width < 0)  //当前宽小于0  offset为负数，为MyRect.rect.width
            {
                offset = MyRect.rect.width;
                ResetToZero();
            }
            else if(MyRect.rect.width>_defaultWidth)  //当前宽大于默认宽（超限） 拿现在宽减去默认宽即可   
            {
                offset = MyRect.rect.width-_defaultWidth;
                ResetToWidth();
            }
            return offset;
        }
          
        /// <summary>
        /// 将血条宽度重置为0
        /// </summary>
        public void ResetToZero()        //重置为0
        {
            MyRect.sizeDelta=Vector2.zero;
        }

        /// <summary>
        /// 血条宽度重置为原始宽度
        /// </summary>
        public void ResetToWidth()    //重置为原始宽度
        {
            MyRect.sizeDelta = Vector2.right * _defaultWidth;
        }
    }
}
