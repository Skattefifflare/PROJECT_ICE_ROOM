using Godot;
using System;

public partial class Arms : Skeleton2D
{
    SkeletonModification2DCcdik leftIK;
    SkeletonModification2DCcdik rightIK;

    NodePath leftTarget;
    NodePath rightTarget;

    Bone2D spearOffset;
    SkeletonModificationStack2D modStack;
    public override void _Ready()
    {
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

        if ((spearOffset.RotationDegrees < -90 && spearOffset.RotationDegrees > -270) || (spearOffset.RotationDegrees > 90 && spearOffset.RotationDegrees < 270))
        {
            leftIK.TargetNodePath = rightTarget;
            rightIK.TargetNodePath = leftTarget;
            GD.Print("Hej");
        }

        else
        {
            GD.Print(spearOffset.RotationDegrees);
            leftIK.TargetNodePath = leftTarget;
            rightIK.TargetNodePath = rightTarget;
        }

        
    }
}