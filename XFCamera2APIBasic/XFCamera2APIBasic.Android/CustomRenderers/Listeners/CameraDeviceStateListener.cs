using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System.Collections.Generic;

namespace XFCamera2APIBasic.Droid.CustomRenderers.Listeners
{
    public class CameraDeviceStateListener : CameraDevice.StateCallback
    {
        private readonly SurfaceTexture _surface;
        private readonly Size _previewSize;
        private readonly Handler _backgroundHandler;
        private CameraDevice _camera;

        public CameraDeviceStateListener(SurfaceTexture surface, Android.Util.Size previewSize, Handler backgroundHandler)
        {
            this._surface = surface;
            this._previewSize = previewSize;
            this._backgroundHandler = backgroundHandler;
        }

        public override void OnOpened(CameraDevice camera)
        {
            _surface.SetDefaultBufferSize(_previewSize.Width, _previewSize.Height);
            Surface surface = new Surface(_surface);

            List<Surface> surfaces = new List<Surface>();
            surfaces.Add(surface);

            var previewBuilder = camera.CreateCaptureRequest(CameraTemplate.Preview);
            //オートフォーカスの設定
            //https://qiita.com/ohwada/items/d33cd9c90abf3ec01f9e
            previewBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);

            previewBuilder.AddTarget(surface);
            var previewRequest = previewBuilder.Build();

            //リスナーをセット
            var captureStateListener = new CameraCaptureSessionStateListener(previewRequest, _backgroundHandler);
            camera.CreateCaptureSession(surfaces, captureStateListener, _backgroundHandler);
        }
        public override void OnDisconnected(CameraDevice camera)
        {
            _camera = camera;
            Close();
        }
        public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
        {
            _camera = camera;
            Close();
        }

        public void Close()
        {
            _camera.Close();
            _camera = null;
        }
    }
}