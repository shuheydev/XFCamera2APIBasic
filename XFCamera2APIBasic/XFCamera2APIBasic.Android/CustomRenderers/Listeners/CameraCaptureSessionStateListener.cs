using Android.Hardware.Camera2;
using Android.OS;
using System;

namespace XFCamera2APIBasic.Droid.CustomRenderers.Listeners
{
    public class CameraCaptureSessionStateListener : CameraCaptureSession.StateCallback
    {
        private readonly CaptureRequest _previewRequest;
        private readonly Handler _backgroundHandler;

        public CameraCaptureSessionStateListener(CaptureRequest previewRequest, Handler backgroundHandler)
        {
            this._previewRequest = previewRequest;
            this._backgroundHandler = backgroundHandler;
        }

        /// <summary>
        /// 準備完了したから
        /// </summary>
        /// <param name="session"></param>
        public override void OnConfigured(CameraCaptureSession session)
        {
            var cameraCaptureListener = new CameraCaptureSessionListener();

            //キャプチャーし続けるよ!
            session.SetRepeatingRequest(_previewRequest, cameraCaptureListener, _backgroundHandler);
        }
        public override void OnConfigureFailed(CameraCaptureSession session)
        {
        }
    }
}