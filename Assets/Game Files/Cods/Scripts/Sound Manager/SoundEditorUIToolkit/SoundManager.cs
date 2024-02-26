using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace As_Star
{
    public class SoundManager : MonoBehaviour
    {
        static GameObject go;
        private static SoundManager Instance;

        public static SoundManager instance
        {

            get
            {
                if (Instance == null)
                {
                    GameObject SoundsManagerOg = Resources.Load("SoundsManager", typeof(GameObject)) as GameObject;
                    go = Instantiate(SoundsManagerOg);
                    DontDestroyOnLoad(go);
                    Instance = go.GetComponent<SoundManager>();
                    Instance.Initialize();
                }
                return Instance;
            }
        }

        [SerializeField] bool User_DontDestroyOnLoad;
        [Space(5)]
        private SoundObject Sounds;

        #region  controlleSound Varibals
        public bool CanPlaySound = true;
        public bool CanPlayMusic = true;

        #endregion

        [SerializeField] GameObject MusicObj;



        private void Initialize()
        {
            Sounds = Resources.Load("SoundData", typeof(SoundObject)) as SoundObject;
            foreach (Sound item in Sounds.Sounds)
            {
                if (item.clip)
                {
                    AudioSource audiosurce = gameObject.AddComponent<AudioSource>();
                    audiosurce.volume = item.volume;

                    audiosurce.playOnAwake = false;
                    audiosurce.loop = false;

                    audiosurce.clip = item.clip;
                    item.source = audiosurce;
                }
            }
        }

        private void Start()
        {
            CanPlaySound = GameManager.Instance.LoadData<bool>(SaveKeys.SoundKey, true);
            CanPlayMusic = GameManager.Instance.LoadData<bool>(SaveKeys.MusicKey, true);
            MusicControlle();
            UI_Game.intance.InitButtons(CanPlaySound, CanPlayMusic);
        }
        public void PlayeWithIndex(int index)
        {
            if (!CanPlaySound) return;
            if (Sounds.Sounds.ElementAtOrDefault(index) != null)
            {
                Sounds.Sounds[index].source.PlayOneShot(Sounds.Sounds[index].source.clip);
            }
        }

        public void PlayeWithName(string name)
        {
            if (!CanPlaySound) return;
            Sound _Sound = Sounds.Sounds.Find(x => x.nameClip == name);
            if (_Sound != null)
                _Sound.source.PlayOneShot(_Sound.source.clip);
        }


        public void MusicControlle()
        {
            MusicObj.SetActive(CanPlayMusic);
        }

        public void InitializeSound()
        {
            Debug.Log("Sound Manager is initialize");

        }
    }
}
