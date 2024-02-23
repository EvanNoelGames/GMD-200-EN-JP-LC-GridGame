using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    private List<Vector2Int> _correctPositions = new List<Vector2Int>();
    private bool _patternPlaying;
    private int _playerPatternIndex;

    private void OnEnable()
    {
        _gridManager.TileSelected += OnTileSelected;
    }

    private void OnDisable()
    {
        _gridManager.TileSelected -= OnTileSelected;
    }

    private void OnTileSelected(GridTile obj)
    {
        if (_patternPlaying)
        {
            return;
        }
        if (obj.gridCoords == _correctPositions[_playerPatternIndex])
        {
            Debug.Log("Correct!");
            StartCoroutine(Co_FlashTile(obj, Color.green, 0.25f));
            _playerPatternIndex++;
            if (_playerPatternIndex == _correctPositions.Count)
            {
                NextPattern();
            }
        }
        else
        {
            Debug.Log("Wrong!");
            StartCoroutine(Co_FlashTile(obj, Color.red, 0.25f));
            _correctPositions.Clear();
            NextPattern();
        }
    }

    public void Start()
    {
        NextPattern();
    }

    [ContextMenu("Next Pattern")]
    public void NextPattern()
    {
        _playerPatternIndex = 0;
        _correctPositions.Add(new Vector2Int(Random.Range(0, _gridManager.numColumns), Random.Range(0, _gridManager.numRows)));
        StartCoroutine(Co_PlayPattern(_correctPositions));
    }

    private IEnumerator Co_PlayPattern(List<Vector2Int> positions)
    {
        _patternPlaying = true;
        yield return new WaitForSeconds(1);
        foreach (var pos in positions)
        {
            GridTile tile = _gridManager.GetTile(pos);
            yield return Co_FlashTile(tile, Color.green, 0.25f);
            yield return new WaitForSeconds(0.5f);
        }
        _patternPlaying = false;
    }

    private IEnumerator Co_FlashTile(GridTile tile, Color color, float duration)
    {
        tile.SetColor(color);
        yield return new WaitForSeconds(duration);
        tile.ResetColor();
    }
}
