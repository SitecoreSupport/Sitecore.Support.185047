using Sitecore.Configuration;
using Sitecore.ExperienceAnalytics.Api.Response.DimensionKeyTransformers;

namespace Sitecore.Support.ExperienceAnalytics.Api.Response.DimensionKeyTransformers
{
    using System.Xml;
    using Sitecore.Collections;
    using Sitecore.Diagnostics;
    using Sitecore.Xml;
    using Sitecore.Globalization;
    using System;

    public class RegionDimensionKeyTransformer : IDimensionKeyTransformer
    {
        private static readonly SafeDictionary<string, string> map = new SafeDictionary<string, string>();

        private static readonly
            Sitecore.ExperienceAnalytics.Api.Response.DimensionKeyTransformers.RegionDimensionKeyTransformer baseTransformer
            ;
        static RegionDimensionKeyTransformer()
        {
            baseTransformer = new Sitecore.ExperienceAnalytics.Api.Response.DimensionKeyTransformers.RegionDimensionKeyTransformer();
            var regionsNode = Factory.GetConfigNode("experienceAnalytics/regions");
            if (regionsNode != null)
            {
                foreach (XmlNode child in regionsNode.ChildNodes)
                {
                    AddRegion(child);
                }
            }
        }
        public virtual string Transform(string key, Language language)
        {
            if (map.ContainsKey(key))
            {
                return map[key];
            }

            return baseTransformer.Transform(key, language);
        }

        public string UnknownLabel
        {
            get { return baseTransformer.UnknownLabel; }
        }

        public static void AddRegion(XmlNode configNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            string code = XmlUtil.GetAttribute("code", configNode);
            string region = XmlUtil.GetAttribute("region", configNode);
            try
            {
                map[code] = region;
            }
            catch (Exception exception)
            {
                Log.Error("Could not register code of region. Code: " + code + ", Region: " + region, exception, typeof(RegionDimensionKeyTransformer));
            }
        }
    }
}
