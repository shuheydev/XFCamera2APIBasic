using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Views;
using Java.Lang;
using System;
using System.Linq;

namespace XFCamera2APIBasic.Droid.CustomRenderers.Listeners
{
    public class CameraSurfaceTextureListener : Java.Lang.Object, TextureView.ISurfaceTextureListener
    {
        private CameraManager _cameraManager = null;
        private HandlerThread _backgroundThread;
        private Handler _backgroundHandler;
        private TextureView _cameraTexture;

        public CameraSurfaceTextureListener(TextureView cameraTexture)
        {
            this._cameraTexture = cameraTexture;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            this.StartCamera(surface);
        }
        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            this.StopCamera();

            return true;
        }
        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
        }
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            //Frame毎に更新される
            //https://stackoverflow.com/questions/29413431/how-to-get-single-preview-frame-in-camera2-api-android-5-0
            var frame = Android.Graphics.Bitmap.CreateBitmap(_cameraTexture.Width, _cameraTexture.Height, Android.Graphics.Bitmap.Config.Argb8888);
            _cameraTexture.GetBitmap(frame);
        }

        private void StartBackgroundThread()
        {
            _backgroundThread = new HandlerThread("CameraBackground");//名前付きでスレッドを作成
            _backgroundThread.Start();
            _backgroundHandler = new Handler(_backgroundThread.Looper);
        }
        private void StopBackgroundThread()
        {
            _backgroundThread.QuitSafely();
            try
            {
                _backgroundThread.Join();
                _backgroundThread = null;
                _backgroundHandler = null;
            }
            catch (InterruptedException ex)
            {
                ex.PrintStackTrace();
            }
        }

        private void StartCamera(SurfaceTexture surface)
        {
            StartBackgroundThread();

            _cameraManager = (CameraManager)Android.App.Application.Context.GetSystemService(Context.CameraService);
            string cameraId = _cameraManager.GetCameraIdList().FirstOrDefault();
            CameraCharacteristics cameraCharacteristics = _cameraManager.GetCameraCharacteristics(cameraId);
            Android.Hardware.Camera2.Params.StreamConfigurationMap scm = (Android.Hardware.Camera2.Params.StreamConfigurationMap)cameraCharacteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
            var previewSize = scm.GetOutputSizes((int)ImageFormatType.Jpeg)[0];

            var cameraStateListener = new CameraStateListener(surface, previewSize, _backgroundHandler);

            _cameraManager.OpenCamera(cameraId, cameraStateListener, _backgroundHandler);
        }
        private void StopCamera()
        {
            StopBackgroundThread();
        }
    }
}