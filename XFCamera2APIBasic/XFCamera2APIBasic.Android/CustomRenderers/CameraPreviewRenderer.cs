using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFCamera2APIBasic.CustomRenderers;
using XFCamera2APIBasic.Droid.CustomRenderers;
using XFCamera2APIBasic.Droid.CustomRenderers.Listeners;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace XFCamera2APIBasic.Droid.CustomRenderers
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreview, DroidCameraPreview>
    {
        private readonly Context _context;
        private DroidCameraPreview _droidCameraPreview;

        public CameraPreviewRenderer(Context context) : base(context)
        {
            this._context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            _droidCameraPreview = new DroidCameraPreview(this._context);

            this.SetNativeControl(_droidCameraPreview);

            if (e.NewElement != null && _droidCameraPreview != null)
            {
                if (this.Element == null || this.Control == null)
                    return;

                this.Control.IsPreviewing = this.Element.IsPreviewing;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == nameof(Element.IsPreviewing))
                this.Control.IsPreviewing = this.Element.IsPreviewing;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    public class DroidCameraPreview : FrameLayout
    {
        private readonly Context _context;
        private readonly TextureView _cameraTexture;

        public Android.Widget.LinearLayout _linearLayout { get; }
        public bool OpeningCamera { private get; set; }
        public long FrameNumber { get; private set; }
        public Android.Graphics.Bitmap Frame { get; private set; }

        private bool _isPreviewing;
        public bool IsPreviewing
        {
            get
            {
                return _isPreviewing;
            }
            set
            {
                if (value)
                {
                    _cameraTexture.Visibility = ViewStates.Visible;
                }
                else
                {
                    _cameraTexture.Visibility = ViewStates.Invisible;
                }
                _isPreviewing = value;
            }
        }

        public DroidCameraPreview(Context context) : base(context)
        {
            this._context = context;

            #region プレビュー用のViewを用意する.
            //予め用意しておいたレイアウトファイルを読み込む場合はこのようにする
            //この場合,Resource.LayoutにCameraLayout.xmlファイルを置いている.
            //中身はTextureViewのみ
            var inflater = LayoutInflater.FromContext(context);
            if (inflater == null)
                return;
            var view = inflater.Inflate(Resource.Layout.CameraPreviewLayout, this);
            _cameraTexture = view.FindViewById<TextureView>(Resource.Id.cameraTexture);

            #region リスナーの登録
            var surfaceTextureListener = new CameraSurfaceTextureListener(_cameraTexture);

            _cameraTexture.SurfaceTextureListener = surfaceTextureListener;
            #endregion

            _cameraTexture.Visibility = ViewStates.Invisible;
            #endregion
        }
    }
}