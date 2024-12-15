using System.Windows;

namespace MultiOpenBrowser.Helpers
{
    public static class DependencyObjectEx
    {
        public static void SetDynamicResourceKey(this DependencyObject obj, DependencyProperty prop, object resourceKey)
        {
            var dynamicResource = new DynamicResourceExtension(resourceKey);
            var resourceReferenceExpression = dynamicResource.ProvideValue(null);
            obj.SetValue(prop, resourceReferenceExpression);
        }
    }
}
