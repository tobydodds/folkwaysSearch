using Orchard.UI.Resources;

namespace Folkways
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest.DefineStyle("Folkways").SetUrl("folkways.min.css");
            manifest.DefineScript("Folkways").SetUrl("folkways.pkgd.js");
        }
    }
}