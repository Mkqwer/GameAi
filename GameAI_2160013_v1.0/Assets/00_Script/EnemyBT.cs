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

public class EnemyBT : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
