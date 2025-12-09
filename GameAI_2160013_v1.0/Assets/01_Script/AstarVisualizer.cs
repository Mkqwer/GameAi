using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic; 


public class AstarVisualizer : MonoBehaviour
{
    // 1. 멤버 변수(필드) 정의
    // A* 경로를 계산하는 스크립트 ( 같은 오브젝트에 붙어 있음 )
    private AStarPathFinder aStarPathFinder = null;

    // 타일 정보를 관리하는 그리드 매니저 ( 같은 오브젝트에 붙어 있음 )
    private GridManager gridManager = null;

    // 경로를 한 칸씩 표시할 때, 각 타일 사이의 대기 시간(초 단위)
    [SerializeField] private float fStepDelaySeconds = 0.1f;

    void Start()
    {
        // 같은 GameObject에 붙어 있는 AStarPathFinder, GridManager를 가져온다.
        aStarPathFinder = GetComponent<AStarPathFinder>();

        gridManager = GetComponent<GridManager>();
    }

    void Update()
    {
        // 새 Input System에서 키보드가 존재하는지 확인
        if (Keyboard.current == null)
        {
            return;
        }

        // A 키가 이번 프레임에 눌렸다면, 경로 표시 코루틴을 실행한다.
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            StartCoroutine(ShowAStarPath());
        }
    }

    private IEnumerator ShowAStarPath()
    {
        // A* 알고리즘을 사용해 계산된 최종 경로를 가져온다.
        List<Vector2Int> path = aStarPathFinder.f_GetAStarPath();

        // 경로가 없으면 아무것도 하지 않고 종료
        if (path == null)
        {
            yield break;
        }

        // 경로에 포함된 각 좌표를 순서대로 처리
        foreach (Vector2Int pos in path)
        {
            // 해당 좌표에 있는 Tile 객체를 GridManager에게서 가져온다.
            Tile tile = gridManager.f_GetTileBounds(pos);

            // 타일이 존재하면 색을 변경한다. (예: 노란색)
            if (tile != null)
            {
                tile.f_SetColor(Color.yellow);
            }

            // fStepDelaySeconds 동안 대기한 뒤, 다음 타일로 넘어간다.
            yield return new WaitForSeconds(fStepDelaySeconds);
        }

    }

}
