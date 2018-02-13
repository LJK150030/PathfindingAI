using Assets.Scripts.AI;
using Assets.Scripts.Square;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Public veriables
        public GraphSearch AI;
        public SquareBoard SquareTesBoard;
        public Frontier[] DataStructures;

        public GameObject UIPanel;
        public GameObject QueueButton;
        public GameObject StackButton;
        public GameObject PriorityButton;
        public GameObject StartButton;
        public GameObject ResetButton;
        public GameObject TimeSlider;
        public GameObject LimitToggle;
        public GameObject LimitInput;
        public GameObject IdToggle;
        public GameObject HeuristicToggle;
        public GameObject ManhattanButton;
        public GameObject EuclideanButton;
        public GameObject AstarToggle;
        #endregion

        #region Private Veriables
        private bool _searchRunning;
        private bool _updateOnce;
        private Image _queueButtonImage;
        private Image _stackButtonImage;
        private Image _pqButtonImage;
        private Image _manhattanImage;
        private Image _euclideanImage;
        private Button _resetButton;
        private Button _manhattanButton;
        private Button _euclideanButton;
        private Button _clearButton;
        private Button _queueButton;
        private Button _stackButton;
        private Button _pqButton;
        private Toggle _heuristicToggle;
        private Toggle _limitToggle;
        private Toggle _astarToggle;
        private Toggle _idToggle;
        private Text _startButtonText;
        private InputField _inputField;
        #endregion

        private void Awake()
        {
            _searchRunning = false;
            _queueButtonImage = QueueButton.GetComponent<Image>();
            _stackButtonImage = StackButton.GetComponent<Image>();
            _pqButtonImage = PriorityButton.GetComponent<Image>();
            _manhattanImage = ManhattanButton.GetComponent<Image>();
            _euclideanImage = EuclideanButton.GetComponent<Image>();
            _manhattanButton = ManhattanButton.GetComponent<Button>();
            _euclideanButton = EuclideanButton.GetComponent<Button>();
            _queueButton = QueueButton.GetComponent<Button>();
            _stackButton = StackButton.GetComponent<Button>();
            _pqButton = PriorityButton.GetComponent<Button>();
            _clearButton = ResetButton.GetComponent<Button>();
            _limitToggle = LimitToggle.GetComponent<Toggle>();
            _astarToggle = AstarToggle.GetComponent<Toggle>();
            _heuristicToggle = HeuristicToggle.GetComponent<Toggle>();
            _idToggle = IdToggle.GetComponent<Toggle>();
            _inputField = LimitInput.GetComponentInChildren<InputField>();
            _startButtonText = StartButton.GetComponentInChildren<Text>();

            SetFrontQueue();
            _manhattanButton.interactable = false;
            _euclideanButton.interactable = false;
            _astarToggle.interactable = false;
            _idToggle.interactable = false;
            _updateOnce = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                TogglePanel();

            if(Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            _clearButton.interactable = !AI._searchRunning;
            SquareTesBoard.AIRunning = AI._searchRunning;
            
            //If we are surching
            if(AI._searchRunning) //We need to disable buttons once
            {
                ActiveDSButtons(false);
                _inputField.interactable = false;
                _idToggle.interactable = false;
                _heuristicToggle.interactable = false;
                _manhattanButton.interactable = false;
                _euclideanButton.interactable = false;
                _astarToggle.interactable = false;
                _limitToggle.interactable = false;
                _startButtonText.text = "Stop \n Search";
                _searchRunning = true;

            }
            else //Once the search is over, then we need to make the buttons available once
            {
                _startButtonText.text = "Start \n Search";
                _searchRunning = false;
                ActiveDSButtons(true);
                _heuristicToggle.interactable = true;
                _limitToggle.interactable = true;
                _inputField.interactable = true;

                if (_limitToggle.isOn)
                    _idToggle.interactable = true;

                if (_heuristicToggle.isOn)
                {
                    _manhattanButton.interactable = true;
                    _euclideanButton.interactable = true;
                    _astarToggle.interactable = true;
                }

            }

        }

        #region DataStructure Buttons
        public void SetFrontQueue()
        {
            AI.UnexploredList = DataStructures[0];
            _queueButtonImage.color = Color.white;
            _stackButtonImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
            _pqButtonImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
        }

        public void SetFrontStack()
        {
            AI.UnexploredList = DataStructures[1];
            _queueButtonImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
            _stackButtonImage.color = Color.white;
            _pqButtonImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
        }

        public void SetFrontPq()
        {
            AI.UnexploredList = DataStructures[2];
            _queueButtonImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
            _stackButtonImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
            _pqButtonImage.color = Color.white;
        }

        public void ActiveDSButtons(bool var)
        {
            if (AI.UnexploredList != DataStructures[0])
                _queueButton.interactable = var;
            if (AI.UnexploredList != DataStructures[1])
                _stackButton.interactable = var;
            if (AI.UnexploredList != DataStructures[2])
                _pqButton.interactable = var;
        }
        #endregion

        public void StartAiSearch()
        {
            //Start Search
            if (!_searchRunning)
            {
                SquareTesBoard.Rest = true;
                _searchRunning = true;
                AI.StartSearch = true;
                _updateOnce = true;
            }
            else // kill search
            {
                AI.StopAllCoroutines();
                AI.UnexploredList.Clear();
                AI._exploredList.Clear();
                AI._iterations = -1;
                AI._searchRunning = false;
                SquareTesBoard.Rest = true;
                _searchRunning = false;
                _updateOnce = true;
                ActiveDSButtons(true);
            }
        }

        public void ResetSearch()
        {
            SquareTesBoard.Rest = true;
        }
        
        public void UpdateCoroutineTime(Slider timeSlider)
        {
            AI.WaitTime = timeSlider.value;
        }

        public void UpdateLimit(InputField text)
        {
            AI.Limit = text.text != null ? int.Parse(text.text) : 0;
        }

        #region Heuristic Buttons
        public void UseManhattan()
        {
            AI.Manhattan = true;
            _manhattanImage.color = Color.white;
            AI.Euclidean = false;
            _euclideanImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
        }

        public void UseEuclidean()
        {
            AI.Manhattan = false;
            _manhattanImage.color = new Color(0.784313725f, 0.784313725f, 0.784313725f);
            AI.Euclidean = true;
            _euclideanImage.color = Color.white;
        }
        #endregion


        #region Toggles
        public void TogglePanel()
        {
            UIPanel.SetActive(!UIPanel.activeSelf);

        }

        public void ToggleId(Toggle id)
        {
            AI.IterativeDeepening = id.isOn;
        }

        public void ToggleHeuristic(Toggle heu)
        {
            if (heu.isOn)
            {
                _manhattanButton.interactable = true;
                _euclideanButton.interactable = true;
                _astarToggle.interactable = true;
                UseManhattan();
            }
            else
            {
                _manhattanButton.interactable = false;
                _euclideanButton.interactable = false;
                _astarToggle.interactable = false;
            }

            AI.Heuristic = heu.isOn;
        }

        public void ToggleLimit(Toggle limitToggle)
        {
            if (limitToggle.isOn)
            {
                _idToggle.interactable = true;
            }
            else
            {
                _idToggle.interactable = false;
                _idToggle.isOn = false;
            }

            AI.UseLimit = limitToggle.isOn;
        }

        public void ToggleAstar(Toggle A)
        {
            AI.AStar = A.isOn;
        }
        #endregion
    }
}
