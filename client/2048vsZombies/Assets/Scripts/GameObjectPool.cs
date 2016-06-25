// -----------------------------------------------------------------------------
//
//  Author : 	Duke Zhou
//  Data : 		2015/12/22
//
// -----------------------------------------------------------------------------
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SleepyHippo.Util
{
	internal class GameObjectPool
	{
//		public const string PoolObjectPrefix = "[P]";
        private static GameObjectPool instance;
        private Dictionary<string, List<GameObject>> poolList;
		private Dictionary<string, Dictionary<int, bool>> poolUsingFlag;
        private GameObject _emptyGOTemplate;
        public GameObject EmptyGOTemplate
        {
            get
            {
                if(_emptyGOTemplate == null)
                {
                    _emptyGOTemplate = new GameObject("EmptyParent");
                    _emptyGOTemplate.SetActive(false);
                    _emptyGOTemplate.transform.parent = PoolParent;
                }
                return _emptyGOTemplate;
            }
        }
        private UIFollowTarget _uiFollowTemplate;
        public UIFollowTarget UIFollowTemplate
        {
            get
            {
                if(_uiFollowTemplate == null)
                {
                    GameObject go = new GameObject("UIFollowParent");
                    go.SetActive(false);
                    _uiFollowTemplate = go.AddComponent(typeof(UIFollowTarget)) as UIFollowTarget;
                    _uiFollowTemplate.transform.parent = PoolParent;
                }
                return _uiFollowTemplate;
            }
        }
        private Transform _uiPoolParent;
        public Transform UIPoolParent
        {
            get
            {
                if(_uiPoolParent == null)
                {
                    _uiPoolParent = new GameObject("[UIPool]").transform;
                    _uiPoolParent.transform.parent = UIManager.instance.effectLayer.transform;
                    CommonUtil.ResetTransform(_uiPoolParent);
                }
                return _uiPoolParent;
            }
        }
        private Transform _poolParent;
		public Transform PoolParent
        {
            get
            {
                if(_poolParent == null)
                    _poolParent = new GameObject("[Pool]").transform;
                return _poolParent;
            }
        }
		
		public static GameObjectPool Instance {
			get {
				if (instance == null)
					instance = new GameObjectPool ();
				return instance;
			}
		}

        public int SpawnCount
        {
            get;
            private set;
        }

		public GameObjectPool ()
		{
            poolList = new Dictionary<string, List<GameObject>>();
            poolUsingFlag = new Dictionary<string, Dictionary<int, bool>>();
		}
		
		public void Allocate (GameObject go, int count = 10)
		{
            string key = go.name;
			if (!poolList.ContainsKey (key)) {
                poolList.Add (key, new List<GameObject>());
				poolUsingFlag.Add (key, new Dictionary<int, bool>());
			}
			for (int i = 0; i < count; ++i) {
                GameObject newGameObject = GameObject.Instantiate<GameObject>(go);
                newGameObject.gameObject.SetActive(false);
				newGameObject.layer = go.layer;
				newGameObject.name = key;
                newGameObject.transform.SetParent(GetPoolParent(newGameObject));

				poolList [key].Add (newGameObject);
                poolUsingFlag[key].Add(newGameObject.GetInstanceID(), false);
			}
		}
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="go">要创建的模板</param>
        /// <param name="preAllocateCount">预先初始化的对象数量</param>
        /// <param name="autoActive">创建出GameObject之后是否自动显示，有时候不希望自动显示，例如需要把一个Widget从一个Panel移动到另一个Panel时，如果先Active了，就需要Inactive后再次Active才能正确显示</param>
        public GameObject Spawn (GameObject go, int preAllocateCount = 10, bool autoActive = true)
		{
            string key = go.name;
			if (!poolList.ContainsKey (key))
                Allocate (go, preAllocateCount);
            List<GameObject> list = poolList[key];
			Dictionary<int, bool> listUsingFlag = poolUsingFlag[key];
			for (int i = 0; i < list.Count; ++i) {
				GameObject gameObject = list [i];
                if (listUsingFlag[gameObject.GetInstanceID()] == false)
                {
                    listUsingFlag[gameObject.GetInstanceID()] = true;
                    if(gameObject != null)
                    {
                        if(autoActive)
                        {
                            gameObject.SetActive(true);
                        }
                        SpawnCount++;
                        return gameObject;
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogError("[EditorLog]GameObjectPool's GameObject is null");//已修复，但如果外界使用不当会出现这个问题
#endif
                    }
				}
			}
			// goes here means there are no available GameObject
            Allocate (go, preAllocateCount);
			return Spawn (go);
		}

        public GameObject SpawnEmptyGO (int preAllocateCount = 10)
        {
            return Spawn(EmptyGOTemplate, preAllocateCount);        
        }

        public UIFollowTarget SpawnUIFollowTarget (int preAllocateCount = 10)
        {
            UIFollowTarget uiFollowTarget = Spawn(UIFollowTemplate.gameObject, preAllocateCount).GetComponent(typeof(UIFollowTarget)) as UIFollowTarget;
            uiFollowTarget.target = null;
            uiFollowTarget.gameCamera = UIManager.instance.mainCamera;
            uiFollowTarget.uiCamera = UIManager.instance.uiCamera;
            return uiFollowTarget;
        }
		
		public void Recycle (GameObject go, bool moveToPoolParent = true)
		{
            SpawnCount--;
            string key = go.name;
            if(poolUsingFlag.ContainsKey(key) && poolUsingFlag[key].ContainsKey(go.GetInstanceID()))
            {
                if(moveToPoolParent)
                {
                    go.transform.SetParent(GetPoolParent(go));
                }
                go.SetActive(false);
                poolUsingFlag[key][go.GetInstanceID()] = false;
            }
		}

		public void Clear()
		{
            Dictionary<string, List<GameObject>>.Enumerator iter = poolList.GetEnumerator();
            while(iter.MoveNext())
            {
                List<GameObject> list = iter.Current.Value;
                for(int i = 0; i < list.Count; ++i)
                {
                    GameObject.Destroy(list[i]);
                }
            }
            poolList = new Dictionary<string, List<GameObject>>();
            poolUsingFlag.Clear();
            poolUsingFlag = new Dictionary<string, Dictionary<int, bool>>();
		}

        private Transform GetPoolParent(GameObject go)
        {
            if(go.layer == Layers.NGUI)
                return UIPoolParent;
            else
                return PoolParent;
        }

        public void LogUsingObject()
        {
#if UNITY_EDITOR
            var poolIter = poolList.GetEnumerator();
            while(poolIter.MoveNext())
            {
                Dictionary<int, bool> usingFlag = poolUsingFlag[poolIter.Current.Key];
                var objectIter = poolIter.Current.Value.GetEnumerator();
                while(objectIter.MoveNext())
                {
                    GameObject go = objectIter.Current;
                    if(usingFlag[go.GetInstanceID()])
                    {
                        if(go != null)
                        {
                            Debug.LogWarning(string.Format("[EditorLog]GameObjectPool2's {0} is in using", go.name));
                        }
                        else
                        {
                            Debug.LogError("[EditorLog]GameObjectPool2's gameobject is null");
                        }
                    }
                }
            }
#endif
        }

	}
}
