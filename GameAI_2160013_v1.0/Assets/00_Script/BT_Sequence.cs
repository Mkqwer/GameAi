/*
BT_Sequence
    - Sequence 노드는 "자식 노드들을 순서대로" 평가하며, 하나라도 Failure가 나오면 즉시 Failure를 반환함
    - 모든 자식이 Success여야만 최종 Success
    - 자식 중 하나가 Running이면. 즉시 Running을 반환
    - "AND"에 가까운 노드


*/


using UnityEngine;
using System.Collections.Generic; // List 사용을 위한 네임스페이스

public class BT_Sequence : BT_Node
{
    private List<BT_Node> lstChildren;

    public BT_Sequence(List<BT_Node> arglstChildren) 
    {
        this.lstChildren = arglstChildren;
    }

    public override BT_NodeStatus Evaluate()
    {
        foreach (BT_Node node in lstChildren)
        {
            BT_NodeStatus status = node.Evaluate();

            if (status == BT_NodeStatus.Failure) // 하나라도 실패했다면, Sequence는 즉시 실패
            {
                return BT_NodeStatus.Failure;
            }
            else if (status == BT_NodeStatus.Running) // 어떤 자식이든 진행 중이라면,아직 진행 중 (다음 프레임에 다시 평가할것)
            {
                return BT_NodeStatus.Running;
            }
        }
        
        return BT_NodeStatus.Success; // 위의 반복에서 실패/진행중을 만나지 못했다면, 모든 자식이 성공
    }
    }
    
