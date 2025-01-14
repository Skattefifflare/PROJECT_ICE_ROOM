using Godot;
using System;

public partial class Arms : Skeleton2D
{
    private SkeletonModification2DCcdik leftIK;
    private SkeletonModification2DCcdik rightIK;

    private NodePath leftTarget;
    private NodePath rightTarget;

    private Bone2D spearOffset;
    private SkeletonModificationStack2D modStack;
    public override void _Ready()
    {
        //Get nodes until reached the inverse kinematic targets
        modStack = GetModificationStack();

        leftIK = (SkeletonModification2DCcdik)modStack.GetModification(0);
        rightIK = (SkeletonModification2DCcdik)modStack.GetModification(1);

        Node root = GetParent();

        Node spearSetup = root.GetChild(2);
        spearOffset = (Bone2D)spearSetup.GetChild(0);

        Node IKBuffer = spearOffset.GetChild(1);
        leftTarget = IKBuffer.GetChild(0).GetPath();
        rightTarget = IKBuffer.GetChild(1).GetPath();
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        //If spear is facing left, change hand position by changing targets for inverse kinematics
        if (Math.Cos(spearOffset.Rotation) < 0)
        {
            leftIK.TargetNodePath = rightTarget;
            rightIK.TargetNodePath = leftTarget;
        }

        else
        {
            leftIK.TargetNodePath = leftTarget;
            rightIK.TargetNodePath = rightTarget;
        }

        
    }
}