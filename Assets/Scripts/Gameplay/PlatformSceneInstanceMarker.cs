using UnityEngine;

[DisallowMultipleComponent]
public class PlatformSceneInstanceMarker : MonoBehaviour
{
    [SerializeField] private string platformId;
    [SerializeField] private int tileX;
    [SerializeField] private int tileY;
    [SerializeField] private Tetromino tetromino;
    [SerializeField] private int rotationTurns;

    public void Apply(string platformIdValue, int tileXValue, int tileYValue, Tetromino tetrominoValue, int rotationTurnsValue)
    {
        platformId = platformIdValue;
        tileX = tileXValue;
        tileY = tileYValue;
        tetromino = tetrominoValue;
        rotationTurns = rotationTurnsValue;
    }
}
