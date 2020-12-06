using Android.Content;
using Android.Views;
using Android.Widget;
using System.ComponentModel;
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

            _droidCameraPreview = new DroidCameraPreview(_context);
        }

        /// <summary>
        /// 要素に変更があった場合に呼び出される
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(_droidCameraPreview);

            if (e.NewElement != null && _droidCameraPreview != null)
            {
                if (this.Element == null || this.Control == null)
                    return;

                this.Control.IsPreviewing = this.Element.IsPreviewing;
            }
        }
        /// <summary>
        /// 要素のプロパティに変更があった場合に呼び出される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //予め用意しておいたレイアウトファイルを読み込む場合はこのようにする
            //この場合,Resource.LayoutにCameraLayout.xmlファイルを置いている.
            //中身はTextureViewのみ
            var inflater = LayoutInflater.FromContext(context);
            if (inflater == null)
                return;
            var view = inflater.Inflate(Resource.Layout.CameraPreviewLayout, this);
            _cameraTexture = view.FindViewById<TextureView>(Resource.Id.cameraTexture);

            var surfaceTextureListener = new CameraSurfaceTextureListener(_cameraTexture);
            _cameraTexture.SurfaceTextureListener = surfaceTextureListener;

            _cameraTexture.Visibility = ViewStates.Visible;
        }
    }
}