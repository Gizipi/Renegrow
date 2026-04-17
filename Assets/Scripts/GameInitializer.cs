using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    public CoreData coreData;
    public Camera mainCamera;
    public SeasonData seasonData;
    public ProgressData progressData;
    public LayerMask boardSlotLayerMask;
    public Slider progressBar;
    public GameObject grassWarningObject;
    private Match _match;
    private WarningSystem _warningSystem;

    public void Start()
    {
        BankMusic();
        Board board = new Board(coreData.boardData);
        List<IWarningCondition> warningConditions = new List<IWarningCondition>();
        warningConditions.Add(new GrassWarningOption(grassWarningObject));
        _warningSystem = new WarningSystem(board, warningConditions);
        _match = new Match(board, seasonData, coreData.audioBank, new ProgressBar(progressBar, progressData), coreData);
        _match.AddBehaviour(new MouseTracker(board, mainCamera, boardSlotLayerMask));
        _match.Enable();
    }

    public void Update()
    {
        _match.Update(Time.deltaTime);
    }

    private void BankMusic()
    {
        foreach (SeasonPacket seasonPacket in seasonData.seasonSprites)
        {
            AudioSource audioSource = Instantiate(seasonPacket.audioSource);
            coreData.audioBank.AddAudio(seasonPacket.season.ToString(), audioSource);
        }
    }
}
