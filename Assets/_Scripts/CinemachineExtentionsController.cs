using Cinemachine;
using UnityEngine;

namespace Farm2D
{
    public class CinemachineExtentionsController : CinemachineExtension
    {
        [SerializeField] private float _pixelPerUnit = 32f;

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                Vector3 finalPos = state.FinalPosition;
                Vector3 roundedFinalPos = new Vector3(Round(finalPos.x), Round(finalPos.y), finalPos.z);
                state.PositionCorrection += roundedFinalPos - finalPos;
            }
        }

        private float Round(float value) => Mathf.Round(value * _pixelPerUnit) / _pixelPerUnit;
    }
}