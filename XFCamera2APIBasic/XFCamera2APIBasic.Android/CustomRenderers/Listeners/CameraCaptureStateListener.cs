using Android.Hardware.Camera2;
using Android.OS;
using System;

namespace XFCamera2APIBasic.Droid.CustomRenderers.Listeners
{
    public class CameraCaptureStateListener : CameraCaptureSession.StateCallback
    {
        private readonly CaptureRequest _previewRequest;
        private readonly Handler _backgroundHandler;

        public long FrameNumber { get; private set; }

        public CameraCaptureStateListener(CaptureRequest previewRequest, Handler backgroundHandler)
        {
            this._previewRequest = previewRequest;
            this._backgroundHandler = backgroundHandler;
        }

        /// <summary>
        /// 準備ができたらリスナーをセットする
        /// </summary>
        /// <param name="session"></param>
        public override void OnConfigured(CameraCaptureSession session)
        {
            var cameraCaptureListener = new CameraCaptureListener();

            session.SetRepeatingRequest(_previewRequest, cameraCaptureListener, _backgroundHandler);
        }
        public override void OnConfigureFailed(CameraCaptureSession session)
        {
        }

        public event EventHandler CaptureCompleted;
        protected virtual void OnCaptureCompleted(EventArgs e)
        {
            CaptureCompleted?.Invoke(this, e);
        }
        private void CameraCaptureListener_CaptureCompleted(object sender, EventArgs e)
        {
            var s = sender as CameraCaptureListener;
            if (s is null)
                return;

            OnCaptureCompleted(e);
        }
    }
}