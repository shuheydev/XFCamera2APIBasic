using Xamarin.Forms;

namespace XFCamera2APIBasic.CustomRenderers
{
    public class CameraPreview : View
    {
        /// <summary>
        /// プレビュー中か否かを表すプロパティを追加
        /// </summary>
        public static readonly BindableProperty IsPreviewingProperty = BindableProperty.Create(
            propertyName: "IsPreviewing",
            returnType: typeof(bool),
            declaringType: typeof(CameraPreview),
            defaultValue: false
            );
        public bool IsPreviewing
        {
            get
            {
                return (bool)GetValue(IsPreviewingProperty);
            }
            set
            {
                SetValue(IsPreviewingProperty, value);
            }
        }
    }

    public enum CameraOptions
    {
        Rear,
        Front
    }
}
