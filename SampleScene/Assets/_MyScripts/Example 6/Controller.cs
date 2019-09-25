using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _MyScripts.Example_6
{
     //数据源以及外部控制器
    public class Controller : MonoBehaviour
    {
        private LifeBar _lifeBar;          //血条对象
        private List<LifeBarData> _barDatas;   //血条数据源
        void Start()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("场景中无Canvas");
                return;
            }
            _barDatas= new List<LifeBarData>()             
            {
                new LifeBarData(null,Color.green),
                new LifeBarData(null,Color.red),
                new LifeBarData(null,Color.yellow),
            };
            SpawnLifeBar(transform,canvas.transform,100,_barDatas);
        }

        /// <summary>
        /// 诞生血条
        /// </summary>
        /// <param name="target">血条对应的物体的Transform</param>
        /// <param name="parent">父物体</param>
        /// <param name="lifeMax">生命最大值</param>
        /// <param name="datas">数据</param>
        private void SpawnLifeBar(Transform target,Transform parent,int lifeMax,List<LifeBarData> datas)
        {
            GameObject prefab=Resources.Load<GameObject>("LifeBar");

            _lifeBar=Instantiate(prefab).AddComponent<LifeBar>();
            _lifeBar.Init(target,parent,lifeMax,datas);
            //   _lifeBar.Init(transform,parent,1000);
            

        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                Move(Vector3.left);
            }
            if (Input.GetKey(KeyCode.W))
            {
                Move(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Move(Vector3.right);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Move(Vector3.back);
            }

            if (Input.GetMouseButtonDown(0))
            {
                //减血
                _lifeBar.ChangeLife(-10);
                
            }

            if (Input.GetMouseButtonDown(2))
            {
                //加血
                _lifeBar.ChangeLife(10);
            }
        }

        /// <summary>
        /// 角色移动
        /// </summary>
        /// <param name="dir"></param>
        private void Move(Vector3 dir)
        {
            transform.Translate(dir*Time.deltaTime);
        }
    }
}
