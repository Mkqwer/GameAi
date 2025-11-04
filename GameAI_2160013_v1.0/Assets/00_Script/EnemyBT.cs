/*
1. Behavior Tree(행동트리) 개념
- 게임 AI가 어떤 행동을 어떤 순서로 수행할지를 트리 구조로 표현한 시스템
- Behavior Tree는 게임 AI에서 널리 사용되는 계층적 의사 결정 구조
- 루트(Root) 노드에서 시작하여 조건(Condition)과 제어 흐름(Composite)을 통해 분기하고
최종적으로 행동(Leaf/Action)을 수행하는 방식
- 트리(Tree) 형태로 구성되며, Sequence(순차 실행),Selector(선택 실행),Decorator(조건 제어) 같은 노드들이 결합되어
  복잡한 AI 의사결정을 단순한 규칙의 조합으로 표현

2. BT의 구성 요소
    - 루트 (Root) : 트리 탐색의 시작점
    - 복합노드 (Composite) : 자식 노드들을 제어(Selector,Sequence)
    - 데코레이터 (Decorator) : 하나의 자식 노드만 제어(반전, 반복 등)
    - 리프 노드 (Leaf Node) : 실제 행동(Action)이나 조건 감사(Condition) 수행

3. BT의 3가지 핵심 내부 노드 : 이 각각의 노드들은 조건을 평가하거나, 행동을 수행하거나, 트리의 흐름을 따라 최종 행동을 결정함
    - 시퀀스(Sequence) : 순차 실행자로싸, 자식 노드들을 순서대로 실행하며, 모두 성공해야 최종 성공을 반환
        자식 노드를 왼쪽부터 순서대로 실행을 하면서 중간에 하나라도 실패하면 실패를 반환
        즉, 모든 자식 노드가 모두 성공해야 성공을 반환하는 노드
    - 선택자 (Selector) : 자식 노드들을 순서대로 평가하며, 하나라도 성공하면 즉시 성공을 반환
        시퀀스 노드와 반대적인 면모가 있음
    -리프(Leaf) : 행동(Action) 또는 조건(Condition)을 실제로 수행하는 가장 말단 노드
        실제 AI 행동과 판단이 일어나고, 예를 들어 공격하거나, 이동을  하거나, 대기하거나 이런식으로 실제 노드에 대한 행동이 파악

4. Behavior Tree를 사용하는 이유
    - 모듈화와 재사용성
    - 같은 행동 노드를 여러 캐릭터가 공유 가능 → 유지보수성과 확장성이 뛰어남
    - 시각적 구조화
    - FSM보다 상태 전이가 복잡하게 얽히지 않고, 읽기 쉬운 계층적 구조를 제공함
    - 복잡한 행동 패턴 표현 용이
    - 단순히 "공격 ↔ 추적" 처럼 정적인 상태 전환이 아니라, 조건 기반 선택(Selector), 순차적 단계(Sequence)
        조건 장식자(Decorator)  등을 이용해 유연하게 동작을 설계할 수 있음
    - 언리얼 엔진, Unity Asset Store 등에서도 Behavior Tree 기반 툴과 패키지가 보편적으로 제공됨

5. Behavior Tree 단점
    - 초기 설계 난이도가 높음
    - 트리 구조를 처음 접하는 사람에게는 추상적이고 이해가 어려울 수 있음
    - 노드 수 증가 → 복잡성 증가
    - 대규모 AI를 설계할 경우 트리 노드가 수백 개에 이를 수 있으며, 관리와 최적화가 어려워질 수 있음
    - Selector, Sequence의 실행 순서를 잘못 설계하면 의도와 다르게 행동할 수 있음

6. 요약
    - Behavior Tree는 모듈화, 시각화, 유연성 측면에서 게임 AI 설계에 매우 강력한 도구임
    - FSM보다 복잡한 행동을 체계적으로 관리할 수 있지만, 트리 구조 관리에 부담이 있음
*/



using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic; // List 사용을 위한 네임스페이스

// 지금까지 만든 Node, Leaf, Sequence, Selector 클래스를 조립하여 실제 적 AI의 행동 트리를 구성하고 실행하는 메인 스크립트
// 적의 상태(공격, 추적, 순찰, 대기)와 그에 따른 행동을 정의하는 클래스 스크립트

public class EnemyBT : MonoBehaviour
{
    /* 필드(멤버 변수) 정의하기
    - root : 행동 트리의 가장 최상위 노드. 모든 로직은 이 root에서 시작됨
    - animatorMonsterState : 적 캐릭터의 애니메이션을 제어하는 컴포넌트
    - characterTarget : 추적하고 공격할 대상(플레이어)의 위치정보
    - fMonsterSpeed, fCahseRange, fAttackRange : 각각 이동속도 추적시작 거리 공격 가능 거리 변수
    */

    public Transform[] waypoints;
    private int nWaypointIndex = 0;

    private BT_Node root = null;              // BT 루트 노드 : 모든 Evaluate() 호출이 시작되는 진입점
    Animator animatorMonsterState = null;    // 애니메이터 상태 변수
    public Transform characterTarget = null; // 추적 대상

    public float fChaseRange = 5.0f;         // 추적할 수 있는 거리 변수, 초기값은 5m
    public float fAttackRange = 1.5f;        // 공격할 수 있는 거리 변수, 추적변수와 초기값은 달라야 함. 초기값 1.5m
    public float fMonsterSpeed = 2.0f;       // 몬스터가 NPC(캐릭터) 추적할 스피드 값 저장 변수
    public float fPatrolRange = 5.0f;

    /* 루트 = Selector
            - 첫 번째 자식 노드 : 시퀀스(Sequence)
                - 조건 노드 : 플레이어가 공격 범위 내에 있는가?
                - 행동 노드 : 공격하기
            - 두 번째 자식 노드 : 시퀀스(Sequence)
                - 조건 노드 : 플레이어가 추적 범위 내에 있는가?
                - 행동 노드 : 추적하기
            - 세 번째 자식 노드 : 행동 노드
                - 행동 노드 : 대기하기
            우선순위는 리스트 순서로 구현
    */


    void Start()
    {
        animatorMonsterState = GetComponent<Animator>();

        // Root : Selector
        root = new BT_Selector
        (new List<BT_Node>
        {
            new BT_Sequence(new List<BT_Node>          // 공격 시퀀스  : [공격 범위?] -> [공격]
            {
               new BT_Leaf(CheckPlayerAttackRange), // 공격 조건 Leaf
               new BT_Leaf(AttackPlayer)              // 행동 Leaf
            }),
            new BT_Sequence(new List<BT_Node>
            {
               new BT_Leaf(CheckPlayerChaseRange), // 추적 조건 Leaf
               new BT_Leaf(ChasePlayer)               // 행동 Leaf
            }),
/*
            new BT_Sequence(new List<BT_Node>
            {
               new BT_Leaf(CheckPlayerPatrolRange), // 순찰 조건 Leaf
               new BT_Leaf(PatrolPlayer)               // 행동 Leaf
            }),
            */
            new BT_Leaf(IdlePlayer)                   // 아무 조건도 충족하지 못하면 Idle
        });

    }

    // 입력받은 range 값과 플레이어와의 실제 거리를 비교하여 플레이어가 공격 범위 안에 있으면 Success, 밖에 있으면 Failure를 반환하는 메소드
    BT_NodeStatus CheckPlayerAttackRange() // 플레이어가 공격 범위 내에 있는가?
    {
        float fMonsterCharacterDist = Vector3.Distance(transform.position, characterTarget.position);

        return (fMonsterCharacterDist <= fAttackRange) ? BT_NodeStatus.Success : BT_NodeStatus.Failure;
    }

    BT_NodeStatus CheckPlayerChaseRange() // 플레이어가 추적 범위 내에 있는가?
    {
        float fMonsterCharacterDist = Vector3.Distance(transform.position, characterTarget.position);

        return (fMonsterCharacterDist <= fChaseRange) ? BT_NodeStatus.Success : BT_NodeStatus.Failure;
    }
/*
    BT_NodeStatus CheckPlayerPatrolRange() // 플레이어가 순찰 범위 내에 있는가?
    {
        float fMonsterCharacterDist = Vector3.Distance(transform.position, characterTarget.position);

        return (fMonsterCharacterDist > fPatrolRange) ? BT_NodeStatus.Success : BT_NodeStatus.Failure;
    }
*/
    BT_NodeStatus IdlePlayer() // 대기하기
    {
        f_Rotate();
        MonsterAnimatorStateChange("IDLE");
        return BT_NodeStatus.Success;
    }

    BT_NodeStatus AttackPlayer() // 공격하기
    {
        f_Rotate();
        MonsterAnimatorStateChange("ATTACK");
        return BT_NodeStatus.Success;
    }

    BT_NodeStatus ChasePlayer() // 추적하기
    {
        transform.position = Vector3.MoveTowards(transform.position, characterTarget.position, Time.deltaTime * fMonsterSpeed);
        f_Rotate();
        MonsterAnimatorStateChange("CHASE");
        return BT_NodeStatus.Running;
    }
    /*
    BT_NodeStatus PatrolPlayer() // 순찰하기
    {
        MonsterPatrolState();       // 실제 순찰 동작 처리
        return BT_NodeStatus.Running;
    }
    */
    private void MonsterAnimatorStateChange(string strState)
    {
        // 애니메이터상태 false 로 초기화
        animatorMonsterState.SetBool("IDLE", false);
        animatorMonsterState.SetBool("CHASE", false);
        animatorMonsterState.SetBool("ATTACK", false);
        animatorMonsterState.SetBool("PATROL", false);

        // 매개변수로 전달 받은 애니메이터 상태만 true 로 변경
        animatorMonsterState.SetBool(strState, true);
    }

    private void f_Rotate()
    {
        Vector3 vector3Direction = (characterTarget.position - transform.position).normalized;
        vector3Direction.y = 0.0f;
        transform.forward = vector3Direction;
    }

    // root.Evaluate()를 매 프레임 호출해 트리 갱신
    // Start()에서 설계한 행동 트리 전체가 최상위 노드부터 시작하여 매 프레임마다 자신의 상태를 평가하고 적절한 행동을 수행
    void Update()
    {
        // 루트의 Evaluate() 메서드를 호출하여 하위 로직을 일괄 수행
        // Sequence, Selector 노드가 AND/OR, Leaf 노드가 실제 동작을 수행하도록 Update
        root.Evaluate();
    }
}
