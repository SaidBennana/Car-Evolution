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
        bool CanPlaySound = true;

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
            // if (GameManager.instance)
            // {
            //     MusicObj.SetActive(!GameManager.instance.LoadData<bool>(SaveKeys.MusicKey, true));
            //     CanPlaySound = !GameManager.instance.LoadData<bool>(SaveKeys.SoundKey, true);

            //     if (UI_Game.Instance)
            //     {
            //         UI_Game.Instance.MusicUI();
            //         UI_Game.Instance.SoundUI();
            //     }
            // }


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

        // sound Controlle
        public bool SoundControlle()
        {
            CanPlaySound = !CanPlaySound;
            //S3.Save(SaveKeys.SoundKey, CanPlaySound);
            return CanPlaySound;
        }
        public bool MusicControlle()
        {
            MusicObj.SetActive(!MusicObj.activeInHierarchy);
            //ES3.Save(SaveKeys.MusicKey, MusicObj.activeInHierarchy);
            return MusicObj.activeInHierarchy;
        }

        public void InitializeSound()
        {
            Debug.Log("Sound Manager is initialize");

        }
    }
}
