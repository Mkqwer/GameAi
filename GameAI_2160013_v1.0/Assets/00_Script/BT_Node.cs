/*
    BT_Node의 용도
        - Selector와 Sequence를 지원하는 간단한 BT 프레임 워크
        - 행동 트리를 구성하는 모든 노드의 기반이 되느 스크립트
        - 모든 노드가 공통으로 가져야 할 기본 구조(추상 클래스)와 노드가 실행된 뒤 결과 상태(열거형)를 정의함
*/

using UnityEngine;
 
/* 행동 트리(Behavior Tree) 노드가 반환할 결과 상태를 정의하는 열거형 : Success, Failure, Running
 
BT_NodeStatus는 각 노드가 자신의 작업을 수행한 뒤 어떤 상태인지를 알려주는 결과표
Success: "내 작업은 성공적으로 끝났어" 라는 의미
Failure: "내 작업은 실패했어" 라는 의미
Running: "작업을 진행중이야. 아직 안 끝났으니 다음 프레임에 다시 확인해줘" 라는 의미
BT 전체에서 공통적으로 사용되며, 각 노드의 Evaluate() 실행 결과로 사용
*/
public enum BT_NodeStatus
{
    Success,   // 성공 : 상위 노드가 다음 단계로 넘어갈 상태
    Failure,   // 실패 : 상위 노드에서 다른 분기를 시도하게 하는 상태
    Running    // 진행 중 : 다음 프레임에서도 계속해서 이 노드를 다시 평가해야 함을 나타내는 상태
}

/* BT_Node는 MonoBehaviour를 지워주고, abstract 클래스로 만들어 줌
 
ㆍ추상 클래스(abstract class)의 정의
추상 함수(abstract function)는 함수의 내용을 구현하지 않는 함수로, 함수의 반환형 앞에 abstract 키워드를 붙여 선언하고,
구현부를 생략하고 세미콜론(;)으로 마무리
이러한 추상 함수를 1개이상 포함하는 클래스를 추상 클래스(abstract class)라고 함
클래스 안에 추상 함수를 선언했다면 해당 클래스는 반드시 추상 클래스로 만들어야 함
추상 클래스는 함수의 구현부가 정의되지 않은 추상 함수를 포함하므로 인스턴스화 할 수 없음
*/

public abstract class BT_Node 
{
    // 현재 노드의 로직을 1프레임 동안 수행하고, 결과를 Success, Failure, Running 중 하나로 반환하는 메소드
    public abstract BT_NodeStatus Evaluate();
}

