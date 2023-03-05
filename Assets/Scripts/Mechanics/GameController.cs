using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public static float timer {get; private set; }  = 5;
        public static float powerTimer { get; private set; } = 0;
        public static float points { get; private set; } = 0;

        [SerializeField] TMPro.TextMeshProUGUI tmpText;
        [SerializeField] TMPro.TextMeshProUGUI pwrText;
        [SerializeField] TMPro.TextMeshProUGUI ptsText;
        [SerializeField] TMPro.TextMeshProUGUI totalPtsText;
        [SerializeField] GameObject gameOverGroup; //drag gameobject into inspector
        [SerializeField] Button restartButton;
        [SerializeField] RectTransform flood;

        float t;


        void Start()
        {
            t = 0;
            restartButton.onClick.AddListener(Restart);
        }
        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
            CountdownTimer();
            if (timer <= 0)
            {
                Flood();
            }
        }

        void Flood()
        {
            t += Time.deltaTime;
            flood.offsetMin = new Vector2(flood.offsetMin.x, Mathf.Lerp(-Screen.height, 0, t));
            flood.offsetMax = new Vector2(flood.offsetMax.x, Mathf.Lerp(-Screen.height, 0, t));
           
        }
        void CountdownTimer()
        {
            if (timer > 60)
            {
                timer = 60;
            }
            timer -= Time.deltaTime; //magic variable!!!!
            tmpText.text = "Timer: " + Mathf.Round(timer).ToString() + "s";
            pwrText.text = "Power Timer: " + Mathf.Round(powerTimer).ToString() + "s";
            ptsText.text = "Points: " + Mathf.Round(points).ToString();
            if (powerTimer > 0)
            {
                if(powerTimer > 30)
                {
                    powerTimer = 30;
                }
                powerTimer -= Time.deltaTime;
            }

            points += 100 * Time.deltaTime;

            if (timer <= -3)
            {
                totalPtsText.text = "Total Points: " + Mathf.Round(points).ToString();
                gameOverGroup.SetActive(true);
                Time.timeScale = 0;
            }

        }

        public static void ReduceTimer(float time)
        {
            if(timer - time < 0) timer = 0;
            else timer -= time;
        }

        public static void PowerUp(float time)
        {
            powerTimer += time;
        }

        public static void IncreasePoints(float value)
        {
            points += value;
        }

        void Restart()
        {
            SceneManager.LoadScene(0);
            timer = 60;
            powerTimer = 0;
            points = 0;
        }
    }
}