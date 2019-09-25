using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _MyScripts.Example_6
{
    //整个血条控制器
    public class LifeBar : MonoBehaviour
    {
        // Start is called before the first frame update
        private Transform _target; //血条对应目标
        private Vector3 _offsetY;  //偏移量Y
        private Camera _cameraMain; //主摄像机
        private float curLifeValue;         //生命值最大值
        private List<LifeBarData> _barDatas;  //血条数据
        private LifeBarItem _curBar;           //当前血条
        private LifeBarItem _nextBar;          //下一个血条
        private float _unitLifeScale;        //单位血对应的宽度系数
        private int _currentIndex;            //当前操作的血条角标    

        /// <summary>
        /// 获取偏移，得到血条应该生成在物体的具体偏移位置
        /// </summary>
        /// <param name="target">血条对应的物体的Transform</param>
        /// <returns>返回偏移量</returns>
        private Vector3 GetOffset(Transform target)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer == null)
            {
                return Vector3.zero;
            }
            return Vector3.up*(renderer.bounds.max.y+0.5f);
        }

        private void Start()
        {
            _cameraMain = Camera.main;  //获取单例主摄像机物体
        }

        /// <summary>
        /// 初始化血条信息
        /// </summary>
        /// <param name="target">血条对应的物体的Transform</param>
        /// <param name="parent">生成的父物体</param>
        /// <param name="lifeMax">生命值</param>
        /// <param name="datas">数据</param>
        public void Init(Transform target,Transform parent,int lifeMax,List<LifeBarData> datas)
        {
            _currentIndex = 0;
            
            transform.SetParent(parent);
            _target = target;
            _offsetY=GetOffset(target);
            curLifeValue = lifeMax;
            _barDatas = datas;
            _curBar = transform.Find("CurrentBar").gameObject.AddComponent<LifeBarItem>();
            _nextBar = transform.Find("NextBar").gameObject.AddComponent<LifeBarItem>();
            _curBar.Init();
            _nextBar.Init();

            RectTransform rect = GetComponent<RectTransform>();
            _unitLifeScale = (rect.rect.width * datas.Count) / lifeMax;

            SetBarData(_currentIndex,datas);

        }

        /// <summary>
        /// 改变生命值，
        /// </summary>
        /// <param name="value"></param>
        public void ChangeLife(float value)
        {
            float width=_curBar.ChangeLife(value*_unitLifeScale);
            curLifeValue += value;
            if (width < 0&&ChangeIndex(1))
            {
                ExChangeBar();
                _curBar.transform.SetAsLastSibling();
                _nextBar.ResetToWidth();
                SetBarData(_currentIndex,_barDatas);
                ChangeLife(width/_unitLifeScale);
            }
            else if(width>0&&ChangeIndex(-1))
            {
                ExChangeBar();
                _curBar.transform.SetAsLastSibling();
                _curBar.ResetToZero();
                SetBarData(_currentIndex,_barDatas);
                ChangeLife(width/_unitLifeScale);
            }
            
        }

        /// <summary>
        /// 改变当前血条角标，当前血条角标只可能（0-数据源个数-1）
        /// </summary>
        /// <param name="symbol">symbol符号   只有1和-1</param>
        /// <returns></returns>
        private bool ChangeIndex(int symbol)
        {
            int index = _currentIndex + symbol;
            if (index >= 0 && index < _barDatas.Count)
            {
                _currentIndex = index;
                return true;
            }

            return false;
        }
        

        /// <summary>
        /// 转换血条，将nextBar与CurBar置换
        /// </summary>
        private void ExChangeBar()
        {
            var temp = _curBar;
            _curBar = _nextBar;
            _nextBar = temp;
        }
        
        /// <summary>
        /// 设置血条数据，curBar为角标对应的数据，nextBar进行设置时需注意，应该判断，如果对应的角标已经越界，直接赋予黑色背景颜色
        /// </summary>
        /// <param name="index">血条数据角标</param>
        /// <param name="datas">血条数据</param>
        public void SetBarData(int index,List<LifeBarData> datas)
        {
            if (index < 0 || index >= datas.Count) return;
            _curBar.SetData(datas[index]);
            _nextBar.SetData((index + 1) >= datas.Count ? new LifeBarData(null, Color.black) : datas[index + 1]);
        }
        
        
        /// <summary>
        /// 将血条生成在目标位置，跟随目标
        /// </summary>
        public void Update()
        {
            if(!_target)
                return;
            var position = _target.position;
            transform.position = _cameraMain.WorldToScreenPoint(new Vector3(position.x,_offsetY.y,position.z));

        }
    }
    
}

public struct LifeBarData
{
    public Sprite BarSprite;
    public Color BarMainColor;

    public LifeBarData(Sprite sprite, Color mainColor)
    {
        BarSprite = sprite;
        BarMainColor = mainColor;
    }
}


