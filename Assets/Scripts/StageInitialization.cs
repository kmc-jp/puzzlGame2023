using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInitialization : MonoBehaviour
{
    [SerializeField] GameObject DefaultStageConfig = null;
    private bool _initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Initialize();
    }

    void Initialize()
    {
        if (_initialized) return;

        if (FindAnyObjectByType<AnimaCanvas>() == null)
        {
            return;
        }

        NetworkGameplayInitializer networkInit = FindAnyObjectByType<NetworkGameplayInitializer>();
        if (networkInit == null || networkInit.LocalPlayerIndex < 0)
        {
            return;
        }

        StageConfig stageConfig = FindAnyObjectByType<StageConfig>();
        if (stageConfig != null)
        {
            InitializeByConfig(stageConfig, networkInit.LocalPlayerIndex);
        }
        else
        {
            InitializeDefault(networkInit.LocalPlayerIndex);
        }

        _initialized = true;
    }

    void InitializeByConfig(StageConfig stageConfig, int localPlayerIndex)
    {
        // Pull settings from config
        AnimaCanvas drawingCanvas = FindAnyObjectByType<AnimaCanvas>();

        CharacterConfig localPlayerConfig = stageConfig.Characters[stageConfig.CharacterIndex[localPlayerIndex]];
        drawingCanvas.AnimaColorPrefabs = localPlayerConfig.AnimaPrefabs.Clone() as GameObject[];

        //TODO: Set/update UI
    }

    void InitializeDefault(int localPlayerIndex)
    {
        // Try to initialize with default stage config
        StageConfig stageConfig = DefaultStageConfig?.GetComponent<StageConfig>();
        if (stageConfig != null)
        {
            InitializeByConfig(stageConfig, localPlayerIndex);
            return;
        }
        
        // Default settings
    }


}
