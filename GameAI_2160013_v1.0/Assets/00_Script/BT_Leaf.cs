/*
BT_Leaf
    - 행동 트리의 가장 마지막에 위치하는 노드로, '실제 행동'이나 '조건 검사'를 담당하는 실행자 역할
    - 예: "공격하라", "플레이어가 범위 내에 있는가?"와 같은 구쳊거인 로직을 수행
    - Leaf(리프) 노드는 행동 트리의 가장 말단에 위치하는 노드
    - 실제 행동(Action)이나 조건 감사(Condition) 수행하는 역할
    - BT_Leaf는 델리게이트(System.Func<BT_NodeStatus>)에 원하는 함수를 넘겨
    - 해당 함수를 Evaluate()에서 호출하는 방식으로 동작

*/
using UnityEngine;

/*
메소드를 담는 변수(델리게이트) Func
    - System.Func는 C#에서 미리 만들어 놓은 '메소드를 담는 변수(델리게이트)'
    - System.Func<BT_NodeStatus> : 반환형이 BT_NodeStatus 인 메소드 레퍼런스를 저장
    - Func : "System.Func<BT_NodeStatus>"
    - <BT_NodeStatus> : "내가 담을 메서드는 반드시 BT_NodeStatus라는 타입을 반환(return)해야 해" 라는 규칙을 의미함

*/

public class BT_Leaf : BT_Node
{
    // Func 델리게이트를사용해 '메소드' 자체를 저장하는 변수
    // BT_NodeStatus(성공, 실패, 실행 중)를 반환하는 어떤 메서드든 담을 수 있는 변수
    // 이 변수는 노드가 수행할 실제 '행동'의 내용의 메소드를 담는다

    private System.Func<BT_NodeStatus> action;

    public BT_Leaf(System.Func<BT_NodeStatus> action)
    {
        this.action = action;
    }

    /*
    Leaf 노드의 Evaluate 메소드 오버라이드
    - 생성자를 통해 action 변수에 저장해 두었던 메소드를 그대로 호출하고,
        그 메소드가 반환하는 BT_NodeStatus 값을 상위 노드에게 전달하는 역할
    - 호출만 담당하므로 심플하고 재사용성 높음
    - Evaluate() : 이 노드가 1프레임 동안 할 일을 실행하고 결과 상태를 반환
    - Leaf는 담아둔 함수(action)을 그대로 호출해 그 반환값을 상위 노드에게 전달함
    */

    public override BT_NodeStatus Evaluate()
    {
        return action();
    }
        
}
