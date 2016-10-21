using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace GoedWare.Controls.Breadcrumb.Services
{
    static class ResourceService
    {
        private static ResourceDictionary _resourceDictionary;
        
        public static T GetDictionaryValue<T>(string resourceName)
        {
            if (_resourceDictionary == null)
            {
                _resourceDictionary = new ResourceDictionary();
                Application.LoadComponent(_resourceDictionary,
                    new Uri("ms-appx:///GoedWare.Controls.Breadcrumb/ResourceDictionary.xaml",
                        UriKind.RelativeOrAbsolute));
            }
            return (T) _resourceDictionary[resourceName];
        }
    }
}
