using Android.Hardware.Camera2;
using System;

namespace XFCamera2APIBasic.Droid.CustomRenderers.Listeners
{
    /// <summary>
    /// フレームごとに発生するイベントに対する処理を担う
    /// </summary>
    public class CameraCaptureListener : CameraCaptureSession.CaptureCallback
    {
        public override void OnCaptureStarted(CameraCaptureSession session, CaptureRequest request, long timestamp, long frameNumber)
        {
            base.OnCaptureStarted(session, request, timestamp, frameNumber);
        }

        /// <summary>
        /// 毎フレームの処理
        /// </summary>
        /// <param name="session"></param>
        /// <param name="request"></param>
        /// <param name="result"></param>
        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            base.OnCaptureCompleted(session, request, result);

            //ここで各フレームの画像に対して処理を行う.
        }

        public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult)
        {
            base.OnCaptureProgressed(session, request, partialResult);
        }
    }
}