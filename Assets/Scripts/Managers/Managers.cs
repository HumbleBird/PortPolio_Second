using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다

    #region Contents
    ObjectManager _object = new ObjectManager();
    BattleManager _battle = new BattleManager();
    UI_BattleManager _uiBattle = new UI_BattleManager();
    [SerializeField]
    MainCamera _maincamera;
    ZoneManager _zone = new ZoneManager();

    public static ObjectManager Object { get { return Instance._object; } }
    public static BattleManager Battle { get { return Instance._battle; } }
    public static UI_BattleManager UIBattle { get { return Instance._uiBattle; } }
    public static MainCamera Camera { get { return Instance._maincamera; } }
    public static ZoneManager Zone { get { return Instance._zone; } }
    #endregion

    #region Core
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();
    TableManager _table = new TableManager();

    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static TableManager Table { get { return Instance._table; } }
    #endregion

    static public string m_strHttp = "http://58.78.211.147:3000/";

    void Start()
    {
        Init();
	}

    void Update()
    {

    }

    static void Init()
    {
        if (s_instance == null)
        {
			GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._table.Init();
            //s_instance._photon.Init();
        }		
	}

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();

    }
}
